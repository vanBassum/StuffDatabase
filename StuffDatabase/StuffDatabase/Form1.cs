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
using FRMLib.Controls;
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
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"{Application.ProductName} V{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            parts = new SaveableBindingList<Part>(Settings.Items.PartsDatabaseFile);
            descriptors = new SaveableBindingList<Descriptor>(Settings.Items.PartDescriptorsDatabaseFile);
            descriptors.ListChanged += Descriptors_ListChanged;
            Descriptors_ListChanged(descriptors, new ListChangedEventArgs(ListChangedType.Reset, -1));


            //TODO check for missing descriptors!
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
                        foreach (Descriptor descriptor in list)
                        {
                            TabPage tabPage = new TabPage(descriptor.Name);
                            TabPageCTRL ctrl = new TabPageCTRL();
                            ctrll = ctrl;
                            tabPage.Tag = ctrl;
                            ctrl.Descriptor = descriptor;
                            ctrl.Dock = DockStyle.Fill;
                            ctrl.DataSource = new FilteredBindingList<Part>(parts) { Filter = new Predicate<Part>(a => a.Descriptor.Name == descriptor.Name) };
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
                    descriptors.Save();
                    parts.Save();
                    Settings.Save();
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

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parts.Save();
            descriptors.Save();
            Settings.Save();
        }

        private void componentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionEditDialog diag = new CollectionEditDialog();
            diag.DataSource = descriptors;
            diag.ShowDialog();
            changePending = true;
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindingList<Part> list = new BindingList<Part>();

            list.Add(new Part() { Name = "test1" } );
            list.Add(new Part() { Name = "test2" } );

            ctrll.DataSource = list;


            ctrll.test();
                
        }
    }

}
