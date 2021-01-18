using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DYMO.Label.Framework;
using STDLib.Saveable;
using STDLib.Serializers;
using StuffDatabase.Components;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {
        SaveableBindingList<BaseComponent> componentDB;


        public Form1()
        {
            Settings.Serializer.Settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
            Settings.Serializer.Settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            Settings.Serializer.Settings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;         //https://stackoverflow.com/questions/13394401/json-net-deserializing-list-gives-duplicate-items


            Settings.Load();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();


            JSON serializer = new JSON();
            serializer.Settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
            serializer.Settings.TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
            serializer.Settings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace;         //https://stackoverflow.com/questions/13394401/json-net-deserializing-list-gives-duplicate-items

            componentDB = new SaveableBindingList<BaseComponent>(serializer, Settings.ComponentDB);

            foreach (Type compType in FindSubClassesOf<BaseComponent>())
            {
                TabPage tabPage = new TabPage(compType.Name);
                tabControl2.TabPages.Add(tabPage);

                CTRL_Component ctrl = new CTRL_Component(compType);
                ctrl.Components = componentDB;
                ctrl.Dock = DockStyle.Fill;
                tabPage.Controls.Add(ctrl);
                tabPage.Tag = ctrl;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            

            bool save = false;
            foreach (TabPage tp in tabControl2.TabPages)
            {
                CTRL_Component ctrl = tp.Tag as CTRL_Component;
                if (ctrl.ChangePending)
                {
                    if (MessageBox.Show("Do you want to save changes?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        e.Cancel = true;
                        save = true;
                    }
                    break;
                }
            }

            if (save)
                componentDB.Save();
            

            Settings.Save();
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
            if (GetSelectedControl() is CTRL_Component ctrl)
                ctrl.Search(tb.Text);
        }



        object GetSelectedControl()
        {
            if (tabControl2.SelectedTab?.Tag is CTRL_Component comp)
                return comp;
            return null;
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog diag = new OpenFileDialog();
            diag.Filter = "CSV file (*.csv)|*.csv";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                ToolStripTextBox tb = sender as ToolStripTextBox;
                if (GetSelectedControl() is CTRL_Component ctrl)
                    ctrl.Import(diag.FileName);
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog diag = new SaveFileDialog();
            diag.Filter = "CSV file (*.csv)|*.csv";
            if (diag.ShowDialog() == DialogResult.OK)
            {
                ToolStripTextBox tb = sender as ToolStripTextBox;
                if (GetSelectedControl() is CTRL_Component ctrl)
                    ctrl.Export(diag.FileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            componentDB.Save();

        }
    }

}
