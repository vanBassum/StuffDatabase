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

namespace StuffDatabase
{
    public partial class CTRL_Chemicals : UserControl
    {
        readonly SaveableBindingList<Component> componentDB = new SaveableBindingList<Component>(@"Resources\components.json");

        public CTRL_Chemicals()
        {
            InitializeComponent();
        }

        private void CTRL_Chemicals_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(@"Resources\Templates\Components"))
                foreach (string file in Directory.GetFiles(@"Resources\Templates\Components", "*.label"))
                    comboBox1.Items.Add(new Template<Component>(file));


            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            listBox1.DataSource = componentDB;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            componentDB.AddNew();
        }

        bool canLoad = true;
        void OpenFromComponent(Component component)
        {
            if (canLoad)
            {
                Template<Component> template = (Template<Component>)comboBox1.SelectedItem;

                textBox1.Text = component.Name;
                richTextBox1.Text = component.Description;

                pictureBox1.Image = Dymo.Render(template, component);
            }
        }

        void SaveToComponent(Component component)
        {
            canLoad = false;
            component.Name = textBox1.Text;
            component.Description = richTextBox1.Text;
            canLoad = true;
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null)
                OpenFromComponent(selectedComponent);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null)
                SaveToComponent(selectedComponent);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Template<Component> template = (Template<Component>)comboBox1.SelectedItem;
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null && template != null)
                Dymo.Print(template, selectedComponent);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null)
                componentDB.Remove(selectedComponent);
        }
    }
}
