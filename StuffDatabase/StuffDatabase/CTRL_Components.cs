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

namespace StuffDatabase
{


    public partial class CTRL_Components : UserControl
    {
        readonly SaveableBindingList<Component> componentDB = new SaveableBindingList<Component>(@"Resources\Components\Database.json");
        Component selectedComponent = null;

        public CTRL_Components()
        {
            InitializeComponent();
        }

        private void CTRL_Components_Load(object sender, EventArgs e)
        {
            if(Directory.Exists(@"Resources\Components\Templates"))
                foreach (string file in Directory.GetFiles(@"Resources\Components\Templates", "*.label"))
                    comboBox1.Items.Add(new Template<Component>(file));


            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            componentDB.SortBy(a => a.Name);
            componentDB.ListChanged += ComponentDB_ListChanged;
            componentDB.SortBy(a => a.Function);
            //listBox1.DataSource = componentDB;

            panel2.AllowDrop = true;
            panel2.DragDrop += Datasheet_DragDrop;
            panel2.DragEnter += Datasheet_DragEnter;

            /*this.AllowDrop = true;
            this.DragDrop += Datasheet_DragDrop;
            this.DragEnter += Datasheet_DragEnter;*/

            //listBox1.KeyDown += KeyDown;

            
        }

        private void ComponentDB_ListChanged(object sender, ListChangedEventArgs e)
        {
            SaveableBindingList<Component> db = sender as SaveableBindingList<Component>;
            bool doCompleteReset = true;
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    AddNode(db[e.NewIndex]);
                    doCompleteReset = false;
                    break;

                case ListChangedType.ItemChanged:
                    //find the node!

                    TreeNode[] tn = GetNodePath(db[e.NewIndex]);

                    if(tn.Length == 2)
                    {
                        if (e.PropertyDescriptor.Name == nameof(Component.Name))
                            tn[1].Text = db[e.NewIndex].ToString();

                        if (e.PropertyDescriptor.Name == nameof(Component.Function))
                        {
                            string newFunc = e.PropertyDescriptor.GetValue(db[e.NewIndex]) as string;
                            if (tn[0].Text != newFunc)
                            {
                                tn[0].Nodes.Remove(tn[1]);
                                AddNode(db[e.NewIndex]);

                                if(tn[0].Nodes.Count == 0)
                                    treeView1.Nodes.Remove(tn[0]);
                            }
                        }
                    }

                    doCompleteReset = false;
                    break;
            }


            if (doCompleteReset)
            {
                treeView1.Nodes.Clear();
                foreach (Component component in componentDB)
                {
                    AddNode(component);
                }
            }
        }


        TreeNode[] GetNodePath(Component component)
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

        void AddNode(Component component)
        {
            if (treeView1.Nodes.ContainsKey(component.Function))
                treeView1.Nodes[component.Function].Nodes.Add(component.Name, component.ToString()).Tag = component;
            else
                treeView1.Nodes.Add(component.Function, component.Function).Nodes.Add(component.Name, component.ToString()).Tag = component;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Component newComponent = new Component();

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
            selectedComponent = newComponent;
            OpenFromComponent(newComponent);

        }

        bool canLoad = true;
        void OpenFromComponent(Component component)
        {
            if (canLoad)
            {
                Template<Component> template = (Template<Component>)comboBox1.SelectedItem;

                textBox1.Text = component.Name;
                textBox2.Text = component.Function;
                richTextBox1.Text = component.Description;
                pictureBox1.Image = Dymo.Render(template, component);
            }
        }

        void SaveToComponent(Component component)
        {
            canLoad = false;
            component.Name = textBox1.Text;
            component.Description = richTextBox1.Text;
            component.Function = textBox2.Text;
            canLoad = true;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedComponent != null)
                SaveToComponent(selectedComponent);
            componentDB.Save();
            OpenFromComponent(selectedComponent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Template<Component> template = (Template<Component>)comboBox1.SelectedItem;
            if (selectedComponent != null && template != null)
                Dymo.Print(template, selectedComponent);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;

            if (node != null)
            {
                if (node.Tag != null)
                {
                    selectedComponent = (Component)node.Tag;
                    componentDB.Remove(selectedComponent);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedComponent != null)
                OpenFromComponent(selectedComponent);
        }

        private void Datasheet_DragDrop(object sender, DragEventArgs e)
        {
            canLoad = false;
            if (selectedComponent != null)
            {
                SaveToComponent(selectedComponent);
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                string newPath = @"Resources\Components\Datasheets\" + selectedComponent.Name + Path.GetExtension(s[0]);
                File.Copy(s[0], newPath, true);
                selectedComponent.Datasheet = newPath;
                OpenFromComponent(selectedComponent);
            }
            canLoad = true;
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
            if (selectedComponent != null)
            {
                if (selectedComponent.Datasheet != "")
                {
                    if(File.Exists(selectedComponent.Datasheet))
                        System.Diagnostics.Process.Start(selectedComponent.Datasheet);
                }
            }
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;

            if (node != null)
            {
                if(node.Tag != null)
                {
                    selectedComponent = (Component)node.Tag;
                    OpenFromComponent(selectedComponent);

                    AutoCompleteStringCollection sourceName = new AutoCompleteStringCollection();

                    foreach (string name in componentDB.Select(c => c.Function))
                    {
                        sourceName.Add(name);
                    }

                    textBox2.AutoCompleteCustomSource = sourceName;
                    textBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
