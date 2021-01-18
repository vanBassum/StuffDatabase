using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DYMO.Label.Framework;
using FRMLib;
using STDLib.Misc;
using STDLib.Saveable;

namespace StuffDatabase
{

    public partial class Form1 : Form
    {
        bool changePending = false;
        SaveableBindingList<Descriptor> descriptors;
        SaveableBindingList<Part> parts;


        public Form1()
        {
            Settings.Load();
            
            InitializeComponent();

            AddMenuItem(menuStrip1, "File/Save", () => { Save(); });
            AddMenuItem(menuStrip1, "File/Import", () => { });
            AddMenuItem(menuStrip1, "File/Export", () => { });
            AddMenuItem(menuStrip1, "Tools/Components", () => { EditComponents(); });
            AddMenuItem(menuStrip1, "Help", () => { });

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"{Application.ProductName} V{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            parts = new SaveableBindingList<Part>(Settings.Items.PartsDatabaseFile);
            descriptors = new SaveableBindingList<Descriptor>(Settings.Items.PartDescriptorsDatabaseFile, false);
            Part.Descriptors = descriptors;
            Descriptor.Descriptors = descriptors;
            DescriptorListStringConverter.Objects = descriptors;
            descriptors.Load();
            descriptors.ListChanged += Descriptors_ListChanged;
            Descriptors_ListChanged(descriptors, new ListChangedEventArgs(ListChangedType.Reset, -1));

            //TODO check for missing descriptors!
        }

        void AddMenuItem(ToolStrip menu, string menuPath, Action action)
        {
            string[] split = menuPath.Split('/');

            ToolStripMenuItem item = null;

            if (menu.Items[split[0]] is ToolStripMenuItem tsi)
                item = tsi;
            else
            {
                item = new ToolStripMenuItem(split[0]);
                item.Name = split[0];
                menu.Items.Add(item);
            }

            for (int i = 1; i < split.Length; i++)
            {
                string name = split[i];

                if (item.DropDownItems[name] is ToolStripMenuItem tsii)
                    item = tsii;
                else
                {
                    ToolStripMenuItem newItem = new ToolStripMenuItem(name);
                    newItem.Name = name;
                    item.DropDownItems.Add(newItem);
                    item = newItem;
                }

            }

            if (action != null)
                item.Click += (a, b) => action.Invoke();


        }

        TabPageCTRL ctrll;

        private void Descriptors_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                default:
                    tabControl1.TabPages.Clear();
                    if(sender is SaveableBindingList<Descriptor> list)
                    {
                        foreach (Descriptor descriptor in list.Where(a=>a.UseAsComponent))
                        {
                            TabPage tabPage = new TabPage(descriptor.Name);
                            TabPageCTRL ctrl = new TabPageCTRL();
                            ctrll = ctrl;
                            tabPage.Tag = ctrl;
                            ctrl.Descriptor = descriptor;
                            ctrl.Dock = DockStyle.Fill;
                            ctrl.DataSource = new FilteredBindingList<Part>(parts) { Filter = new Predicate<Part>(a => a.GetDescriptor().ID == descriptor.ID) };
                            ctrl.ObjectChanged += EditDialog_ObjectChanged;
                            tabPage.Controls.Add(ctrl);
                            tabControl1.TabPages.Add(tabPage);
                        }
                    }
                    break;
            }
        }




        private void EditDialog_ObjectChanged(object sender, object e)
        {
            changePending = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            if(changePending || Settings.PendingChanges)
            {
                if (MessageBox.Show("Do you want to save changes?", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Save();
                }
                changePending = false;
            }
            e.Cancel = false;
        }

        public IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var baseType = typeof(TBaseType);
            var assembly = baseType.Assembly;

            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }

        private void toolStripTextBox1_Enter(object sender, EventArgs e)
        {
            ToolStripTextBox tb = sender as ToolStripTextBox;

            if (tb.Text == "Search")
            {
                tb.Text = "";
                tb.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            ToolStripTextBox tb = sender as ToolStripTextBox;

            if(tabControl1.SelectedTab.Tag is TabPageCTRL ctrl)
            {
                //ctrl.DataSource.Filter = (a) => a.Descriptor.Name == ctrl.Descriptor.Name && a.Name.Contains(tb.Text);
            }
        }


        void Save()
        {
            parts.Save();
            descriptors.Save();
            Settings.Save();
        }

        void EditComponents()
        {
            CollectionEditDialog diag = new CollectionEditDialog();
            diag.DataSource = descriptors;
            diag.CreateObject = () => Descriptor.Create();
            diag.ShowDialog();
            changePending = true;
        }

        /*
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void componentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindingList<Part> list = new BindingList<Part>();

            list.Add(new Part() { Name = "test1" } );
            list.Add(new Part() { Name = "test2" } );

            ctrll.DataSource = list;    
        }
        */
    }

}
