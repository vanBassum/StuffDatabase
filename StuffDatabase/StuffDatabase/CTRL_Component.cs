using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STDLib.Saveable;
using System.IO;
using System.Collections.ObjectModel;
using StuffDatabase.Components;
using System.Reflection;

namespace StuffDatabase
{
    public partial class CTRL_Component : UserControl
    {
        public Type ComponentType { get; set; }
        SaveableBindingList<BaseComponent> _comps;
        public SaveableBindingList<BaseComponent> Components { get { return _comps; } set { _comps = value; Components.ListChanged += Components_ListChanged; RePopulateList(true); } }
        Predicate<BaseComponent> Filter { get; set; }
        string TemplateFolder { get { return CreatePathIfNotExist(Path.Combine(Settings.ComponentData, ComponentType.Name, "Templates")); } }
        string DatasheetFolder { get { return CreatePathIfNotExist(Path.Combine(Settings.ComponentData, ComponentType.Name, "Datasheets")); } }

        public bool ChangePending { get; set; } = false;

        string CreatePathIfNotExist(string path)
        {
            Directory.CreateDirectory(path);
            return path;
        }

        public CTRL_Component(Type compType)
        {
            InitializeComponent();
            ComponentType = compType;

            foreach (string file in Directory.GetFiles(TemplateFolder, "*.label"))
            {
                comboBox1.Items.Add(new Template<BaseComponent>(file));
                if (comboBox1.SelectedIndex == -1)
                    comboBox1.SelectedIndex = 0;
            }

            Filter = (a) => a.GetType() == ComponentType;
        }

        private void CTRL_Components_Load(object sender, EventArgs e)
        {
            SetUIEditMode(false);
        }

        private void Components_ListChanged(object sender, ListChangedEventArgs e)
        {
            ChangePending = true;
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    if (Filter(Components[e.NewIndex]))
                        AddNode(Components[e.NewIndex]);
                    break;

                default:
                    RePopulateList();
                    break;
            }
        }

        void RePopulateList(bool expandAll = false)
        {
            List<string> expanedNodes = new List<string>();

            foreach (TreeNode nt in treeView1.Nodes)
                if (nt.IsExpanded || expandAll)
                    expanedNodes.Add(nt.Text);

            treeView1.Nodes.Clear();
            foreach (BaseComponent component in Components)
            {
                if (Filter(component))
                    AddNode(component);
            }

            foreach (string s in expanedNodes)
            {
                TreeNode tn = treeView1.Nodes[s];
                if (tn != null)
                    tn.Expand();
            }
        }

        void AddNode(BaseComponent component)
        {
            if (treeView1.Nodes.ContainsKey(component.Function))
                treeView1.Nodes[component.Function].Nodes.Add(component.Name, component.ToString()).Tag = component;
            else
                treeView1.Nodes.Add(component.Function, component.Function).Nodes.Add(component.Name, component.ToString()).Tag = component;
        }

        BaseComponent EditCompOrigional;
        BaseComponent EditCompNew;

        void StartEditComponent(BaseComponent comp)
        {
            EditCompOrigional = comp;
            EditCompNew = (BaseComponent)Activator.CreateInstance(EditCompOrigional.GetType());
            EditCompNew.Populate(EditCompOrigional);
            SetUIEditMode(true);
            EditCompNew.PropertyChanged += (a,b) => Render(EditCompNew);
        }

        void StopEditComponent(bool saveChanges)
        {
            EditCompNew.PropertyChanged -= (a, b) => Render(EditCompOrigional);

            if (saveChanges)
            {
                EditCompOrigional.Populate(EditCompNew);
                if(!Components.Contains(EditCompOrigional))
                    Components.Add(EditCompOrigional);
            }
            SetUIEditMode(false);
            Render(EditCompOrigional);
        }

        void SetUIEditMode(bool editMode)
        {
            treeView1.Enabled = !editMode;
            btn_Edit.Enabled = !editMode;
            btn_Cancel.Enabled = editMode;
            btn_Save.Enabled = editMode;
            btn_Print.Enabled = !editMode;
            btn_New.Enabled = !editMode;
            btn_Delete.Enabled = !editMode;
            comboBox1.Enabled = !editMode;
            propertyGrid1.Enabled = editMode;
            propertyGrid1.SelectedObject = editMode?EditCompNew:GetSelectedComponent();
        }

        BaseComponent GetSelectedComponent()
        {
            if (treeView1.SelectedNode?.Tag is BaseComponent comp)
                return comp;
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            BaseComponent editComp = GetSelectedComponent();
            if (editComp != null)
            {
                StartEditComponent(GetSelectedComponent());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StopEditComponent(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopEditComponent(false);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BaseComponent selectedComp = GetSelectedComponent();
            propertyGrid1.SelectedObject = selectedComp;
            Render(selectedComp);
        }

        void Render(BaseComponent component)
        {
            Template<BaseComponent> template = (Template<BaseComponent>)comboBox1.SelectedItem;
            if (component != null && template != null)
            {
                pictureBox1.Image = Dymo.Render(template, component);
            }
        }

        private void btn_New_Click(object sender, EventArgs e)
        {
            BaseComponent newComponent = (BaseComponent)Activator.CreateInstance(ComponentType);

            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Parent == null)
                {
                    newComponent.Function = treeView1.SelectedNode.Text;
                }
                else
                {
                    newComponent.Function = treeView1.SelectedNode.Parent.Text;
                }
            }
            StartEditComponent(newComponent);
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            BaseComponent selectedComponent = GetSelectedComponent();
            if (selectedComponent != null)
            {
                Components.Remove(selectedComponent);
            }
        }

        private void btn_Print_Click(object sender, EventArgs e)
        {
            Template<BaseComponent> template = (Template<BaseComponent>)comboBox1.SelectedItem;
            BaseComponent selectedComponent = GetSelectedComponent();
            if (selectedComponent != null && template != null)
            {
                Dymo.Print(template, selectedComponent);
            }
        }


        public void Search(string searchString)
        {
            if (searchString == "")
                Filter = (a) => a.GetType() == ComponentType;
            else
                Filter = (a) => a.GetType() == ComponentType && a.Name.ToLower().Contains(searchString.ToLower());

            RePopulateList(true);
        }

        public void Export(string file)
        {

            using (StreamWriter writer = new StreamWriter(file))
            {
                PropertyInfo[] pis = ComponentType.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    writer.Write($"\"{pi.Name}\"" + ((pi == pis.Last())?"\r\n":","));
                }

                foreach (BaseComponent component in Components)
                {
                    if (Filter(component))
                    {
                        foreach (PropertyInfo pi in pis)
                        {
                            object val = pi.GetValue(component);
                            switch (val)
                            {
                                case double v:
                                    writer.Write($"\"{v.ToString("E")}\"" + ((pi == pis.Last()) ? "\r\n" : ","));
                                    break;

                                default:
                                    writer.Write($"\"{val}\"" + ((pi == pis.Last()) ? "\r\n" : ","));
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void Import(string file)
        {
            List<PropertyInfo> props = new List<PropertyInfo>();
            SaveableBindingList<BaseComponent> comps = new SaveableBindingList<BaseComponent>();

            using (StreamReader reader = new StreamReader(file))
            {
                //Read header

                string header = reader.ReadLine();

                foreach(string propName in header.Split(','))
                {
                    PropertyInfo pi = ComponentType.GetProperty(propName.Trim('"'));
                    if (pi == null)
                        throw new Exception("Property not found.");
                    props.Add(pi);
                }

                while(!reader.EndOfStream)
                {
                    List<string> values = new List<string>();
                    string line = reader.ReadLine();

                    string val = "";
                    bool b = false;
                    foreach(char c in line)
                    {
                        if (c == '"')
                            b = !b;
                        else
                        {
                            if (b)
                                val += c;
                            else
                            {
                                if (c == ',')
                                {
                                    values.Add(val);
                                    val = "";
                                }

                                else
                                    val += c;
                            }
                        }

                    }

                    

                    BaseComponent comp = (BaseComponent)Activator.CreateInstance(ComponentType);

                    for (int i = 0; i < values.Count; i++)
                    {
                        if(props[i].PropertyType == typeof(double))
                        {
                            props[i].SetValue(comp, double.Parse(values[i]));
                        }
                        else
                        {
                            props[i].SetValue(comp, values[i]);
                        }

                        
                    }
                    comps.Add(comp);
                }

            }

            Frm_Import frm = new Frm_Import(comps, ComponentType);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                bool overrideExisting = frm.OverrideExisting;
                foreach(BaseComponent comp in comps)
                {
                    int ind = Components.IndexOf(Components.FirstOrDefault(c => c.Name == comp.Name && c.Function == comp.Function));
                    if(ind == -1)
                    {
                        Components.Add(comp);           //Add new component
                    }
                    else
                    {
                        if(overrideExisting)
                        {
                            Components.RemoveAt(ind);   //Override existing
                            Components.Add(comp);
                        }
                        else
                        {
                            //TODO ask???
                        }
                    }
                }
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.P)
            {
                btn_Print_Click(null, null);
            }
        }
    }
}
