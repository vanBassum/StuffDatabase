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
            listBox1.DataSource = componentDB;

            panel2.AllowDrop = true;
            panel2.DragDrop += Datasheet_DragDrop;
            panel2.DragEnter += Datasheet_DragEnter;

            /*this.AllowDrop = true;
            this.DragDrop += Datasheet_DragDrop;
            this.DragEnter += Datasheet_DragEnter;*/

            listBox1.KeyDown += KeyDown;
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control || e.Modifiers == Keys.RControlKey)
            {
                if(e.KeyCode == Keys.P)
                {
                    Template<Component> template = (Template<Component>)comboBox1.SelectedItem;
                    Component selectedComponent = (Component)listBox1.SelectedItem;
                    if (selectedComponent != null && template != null)
                        Dymo.Print(template, selectedComponent);
                }
            }
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
            componentDB.Save();
            OpenFromComponent(selectedComponent);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null)
                OpenFromComponent(selectedComponent);
        }

        private void Datasheet_DragDrop(object sender, DragEventArgs e)
        {
            
            canLoad = false;
            Component selectedComponent = (Component)listBox1.SelectedItem;
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
            Component selectedComponent = (Component)listBox1.SelectedItem;
            if (selectedComponent != null)
            {
                if (selectedComponent.Datasheet != "")
                {
                    if(File.Exists(selectedComponent.Datasheet))
                        System.Diagnostics.Process.Start(selectedComponent.Datasheet);
                }
            }
        }
    }
}
