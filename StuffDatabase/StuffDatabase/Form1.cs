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
using STDLib.Saveable;
using StuffDatabase.Components;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {
        SaveableBindingList<BaseComponent> componentDB;


        public Form1()
        {
            Settings.Load("Settings.json");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            componentDB = new SaveableBindingList<BaseComponent>(Settings.ComponentDB);

            foreach (Type compType in FindSubClassesOf<BaseComponent>())
            {
                TabPage tabPage = new TabPage(compType.Name);
                tabControl2.TabPages.Add(tabPage);

                CTRL_Component ctrl = new CTRL_Component(compType);
                ctrl.Components = componentDB;
                ctrl.Dock = DockStyle.Fill;
                tabPage.Controls.Add(ctrl);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save("Settings.json");
        }

        public IEnumerable<Type> FindSubClassesOf<TBaseType>()
        {
            var baseType = typeof(TBaseType);
            var assembly = baseType.Assembly;

            return assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));
        }
    }

}
