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

    public partial class CTRL_Chemical : UserControl
    {
        SaveableBindingList<Chemical> chemicalDB;

        public CTRL_Chemical()
        {
            InitializeComponent();
            
        }

        private void CTRL_Chemical_Load(object sender, EventArgs e)
        {
            chemicalDB = new SaveableBindingList<Chemical>(Settings.ChemicalDB);
            if (Directory.Exists(Settings.ChemicalTemplates))
                foreach (string file in Directory.GetFiles(Settings.ChemicalTemplates, "*.label"))
                    comboBox1.Items.Add(new Template<Chemical>(file));

            try
            {
                foreach (string ghsFile in Directory.GetFiles(Settings.ChemicalSymbols, "*.png"))
                {
                    CheckBox cb = new CheckBox() { Text = Path.GetFileNameWithoutExtension(ghsFile), Tag = ghsFile };
                    flowLayoutPanel1.Controls.Add(cb);
                }
            }
            catch(Exception ex)
            { }

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            chemicalDB.SortBy(a => a.Name);
            listBox1.DataSource = chemicalDB;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            chemicalDB.AddNew();
        }

        bool canLoad = true;
        void OpenFromChemical(Chemical chemical)
        {
            if (canLoad)
            {
                Template<Chemical> template = (Template<Chemical>)comboBox1.SelectedItem;

                textBox1.Text = chemical.Name;
                textBox2.Text = chemical.Formula;
                richTextBox1.Text = chemical.Description;

                foreach (var v in flowLayoutPanel1.Controls)
                {
                    if (v.GetType() == typeof(CheckBox))
                    {
                        CheckBox cb = v as CheckBox;
                        cb.Checked = chemical.GHSitems.FindIndex(a => a.Name == cb.Text) > -1;
                    }
                }

                pictureBox1.Image = Dymo.Render(template, chemical);
            }
        }

        void SaveToChemical(Chemical chemical)
        {
            canLoad = false;
            chemical.Name = textBox1.Text;
            chemical.Formula = textBox2.Text;
            chemical.Description = richTextBox1.Text;
            chemical.GHSitems.Clear();
            foreach (var v in flowLayoutPanel1.Controls)
            {
                if (v.GetType() == typeof(CheckBox))
                {
                    CheckBox cb = v as CheckBox;
                    if (cb.Checked)
                        chemical.GHSitems.Add(new GHS() { Name = cb.Text, ImageFile = cb.Tag as string });
                }
            }
            chemicalDB.Save();
            canLoad = true;
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chemical selectedChemical = (Chemical)listBox1.SelectedItem;
            if (selectedChemical != null)
                OpenFromChemical(selectedChemical);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Chemical selectedChemical = (Chemical)listBox1.SelectedItem;
            if (selectedChemical != null)
                SaveToChemical(selectedChemical);
            chemicalDB.Save();
            OpenFromChemical(selectedChemical);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Template<Chemical> template = (Template<Chemical>)comboBox1.SelectedItem;
            Chemical selectedChemical = (Chemical)listBox1.SelectedItem;
            if (selectedChemical != null && template != null)
                Dymo.Print(template, selectedChemical);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Chemical selectedChemical = (Chemical)listBox1.SelectedItem;
            if (selectedChemical != null)
                chemicalDB.Remove(selectedChemical);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Chemical selectedChemical = (Chemical)listBox1.SelectedItem;
            if (selectedChemical != null)
                OpenFromChemical(selectedChemical);
        }
    }
}
