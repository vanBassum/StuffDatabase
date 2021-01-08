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
        SaveableBindingList <Descriptor> descriptors;
        PartList parts;


        public Form1()
        {
            Settings.Load();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"{Application.ProductName} V{Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";
            parts = new PartList(Settings.Items.PartsDatabaseFile);
            descriptors = new SaveableBindingList<Descriptor>(Settings.Items.PartDescriptorsDatabaseFile);
            descriptors.ListChanged += Descriptors_ListChanged;
            Descriptors_ListChanged(descriptors, new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        
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
                            CollectionEditControl editDialog = new CollectionEditControl();
                            editDialog.CreateObject = () => Part.Create(descriptor);
                            editDialog.Dock = DockStyle.Fill;
                            editDialog.DataSource = new FilteredBindingList<Part>(parts) { Filter = new Predicate<Part>(a=>a.Descriptor.Name == descriptor.Name) };
                            editDialog.DisplayMember = nameof(Part.Name);
                            editDialog.ObjectChanged += EditDialog_ObjectChanged;
                            tabPage.Controls.Add(editDialog);
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
            //if (GetSelectedControl() is CTRL_Component ctrl)
            //    ctrl.Search(tb.Text);
        }



        object GetSelectedControl()
        {
            //if (tabControl2.SelectedTab?.Tag is CTRL_Component comp)
            //    return comp;
            return null;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "CSV file (*.csv)|*.csv";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                ToolStripTextBox tb = sender as ToolStripTextBox;
                //if (GetSelectedControl() is CTRL_Component ctrl)
                //    ctrl.Import(diag.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "CSV file (*.csv)|*.csv";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                ToolStripTextBox tb = sender as ToolStripTextBox;
                //if (GetSelectedControl() is CTRL_Component ctrl)
                //    ctrl.Export(diag.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parts.Save();
            descriptors.Save();
            //componentDB.Save();

        }

        private void componentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CollectionEditDialog diag = new CollectionEditDialog();
            diag.DataSource = descriptors;
            diag.ShowDialog();
            changePending = true;
        }
    }

}
