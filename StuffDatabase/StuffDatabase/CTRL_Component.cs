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
                Components.Save();
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
            btn_Import.Enabled = !editMode;
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
                Components.Save();
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
                Filter = (a) => a.GetType() == ComponentType;
            else
                Filter = (a) => a.GetType() == ComponentType && a.Name.ToLower().Contains(textBox1.Text.ToLower());

            RePopulateList(true);
        }
    }



}
