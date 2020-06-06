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
    public partial class CTRL_Components : UserControl
    {
        readonly SaveableBindingList<BaseComponent> componentDB = new SaveableBindingList<BaseComponent>(@"Resources\Components\Database.json");
        private Predicate<BaseComponent> Filter { get; set; } = (a) => (true);
        public CTRL_Components()
        {
            InitializeComponent();
        }

        private void CTRL_Components_Load(object sender, EventArgs e)
        {
            if(Directory.Exists(@"Resources\Components\Templates"))
                foreach (string file in Directory.GetFiles(@"Resources\Components\Templates", "*.label"))
                    comboBox1.Items.Add(new Template<BaseComponent>(file));

            componentDB.SaveOnDestruction = true;

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            var vs = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(BaseComponent).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
            tabControl1.TabPages.Clear();
            foreach (var v in vs)
            {
                TabPage tab = new TabPage(v.Name);
                tab.Tag = v;
                tabControl1.TabPages.Add(tab);
            }

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            TabControl1_SelectedIndexChanged(null, null);

            componentDB.SortBy(a => a.Name);
            componentDB.ListChanged += ComponentDB_ListChanged;
            componentDB.SortBy(a => a.Function);

            panel2.AllowDrop = true;
            panel2.DragDrop += Datasheet_DragDrop;
            panel2.DragEnter += Datasheet_DragEnter;

            treeView1.AfterSelect += TreeView1_AfterSelect;

            propertyGrid1.HelpVisible = false;
            propertyGrid1.PropertySort = PropertySort.Categorized;

            
            
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null)
            {
                Type compType = (Type)tabControl1.SelectedTab.Tag;
                Filter = (a) => a.GetType() == compType;
                ComponentDB_ListChanged(componentDB, new ListChangedEventArgs(ListChangedType.Reset, 0));
            }

        }

        bool TryGetSelectedComponent(out BaseComponent comp)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag != null)
                {
                    comp = (BaseComponent)treeView1.SelectedNode.Tag;
                    return true;
                }
            }
            comp = null;
            return false;
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent))
            {
                propertyGrid1.SelectedObject = selectedComponent;
                selectedComponent.PropertyChanged += SelectedComponent_PropertyChanged;
                SelectedComponent_PropertyChanged(selectedComponent, null);
            }
        }

        private void SelectedComponent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Render();
        }


        void Render()
        {
            Template<BaseComponent> template = (Template<BaseComponent>)comboBox1.SelectedItem;
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent) && template != null)
            {
                pictureBox1.Image = Dymo.Render(template, selectedComponent);
            }
        }

        private void ComponentDB_ListChanged(object sender, ListChangedEventArgs e)
        {
            SaveableBindingList<BaseComponent> db = sender as SaveableBindingList<BaseComponent>;
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    if(Filter(db[e.NewIndex]))
                        AddNode(db[e.NewIndex]);
                    break;
                    /*
                case ListChangedType.ItemChanged:
                    //find the node!

                    TreeNode[] tn = GetNodePath(db[e.NewIndex]);

                    if(tn.Length == 2)
                    {
                        if (e.PropertyDescriptor != null)
                        {
                            if (e.PropertyDescriptor.Name == nameof(BaseComponent.Name))
                                tn[1].Text = db[e.NewIndex].ToString();

                            if (e.PropertyDescriptor.Name == nameof(BaseComponent.Function))
                            {
                                string newFunc = e.PropertyDescriptor.GetValue(db[e.NewIndex]) as string;
                                if (tn[0].Text != newFunc)
                                {
                                    tn[0].Nodes.Remove(tn[1]);
                                    AddNode(db[e.NewIndex]);

                                    if (tn[0].Nodes.Count == 0)
                                        treeView1.Nodes.Remove(tn[0]);
                                }
                            }
                        }
                    }
                    break;
                    */
                default:
                    treeView1.Nodes.Clear();
                    foreach (BaseComponent component in componentDB)
                    {
                        if (Filter(component))
                            AddNode(component);
                    }
                    break;
            }
        }


        TreeNode[] GetNodePath(BaseComponent component)
        {
            foreach (TreeNode tn1 in treeView1.Nodes)
            {
                foreach (TreeNode tn2 in tn1.Nodes)
                {
                    if (tn2.Tag == component)
                    {
                        return new TreeNode[] { tn1, tn2 };
                    }
                }
            }
            return new TreeNode[] { };
        }

        void AddNode(BaseComponent component)
        {
            if (treeView1.Nodes.ContainsKey(component.Function))
                treeView1.Nodes[component.Function].Nodes.Add(component.Name, component.ToString()).Tag = component;
            else
                treeView1.Nodes.Add(component.Function, component.Function).Nodes.Add(component.Name, component.ToString()).Tag = component;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Type compType = (Type)tabControl1.SelectedTab.Tag;
            BaseComponent newComponent = (BaseComponent)Activator.CreateInstance(compType);

            if(treeView1.SelectedNode != null)
            {
                if(treeView1.SelectedNode.Parent == null)
                {
                    newComponent.Function = treeView1.SelectedNode.Text;
                }
                else
                {
                    newComponent.Function = treeView1.SelectedNode.Parent.Text;
                }
            }

            componentDB.Add(newComponent);
        }


        private void btn_Print_Click(object sender, EventArgs e)
        {
            Template<BaseComponent> template = (Template<BaseComponent>)comboBox1.SelectedItem;
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent) && template != null)
            {
                Dymo.Print(template, selectedComponent);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent))
            {
                componentDB.Remove(selectedComponent);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Render();
        }

        private void Datasheet_DragDrop(object sender, DragEventArgs e)
        {
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                string newPath;
                while (File.Exists(newPath = $@"Resources\Components\Datasheets\{Ext.GetRandomHexNumber(6)}{Path.GetExtension(s[0])}")) ;

                File.Copy(s[0], newPath, true);
                selectedComponent.Datasheet = newPath;
            }
        }

        private void Datasheet_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;

        }

        private void panel2_Click(object sender, EventArgs e)
        {
            BaseComponent selectedComponent;
            if (TryGetSelectedComponent(out selectedComponent))
            {
                if (selectedComponent.Datasheet != "")
                {
                    if (File.Exists(selectedComponent.Datasheet))
                        System.Diagnostics.Process.Start(selectedComponent.Datasheet);
                }
            }
        }
    }
}
