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
using STDLib.Misc;

namespace StuffDatabase
{
    public partial class CTRL_Chemicals : UserControl
    {
        readonly SaveableBindingList<Chemical> ChemicalDB = new SaveableBindingList<Chemical>(@"Resources\Chemicals.json");

        public CTRL_Chemicals()
        {
            InitializeComponent();
        }

        private void CTRL_Chemicals_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(@"Resources\Templates\Chemicals"))
                foreach (string file in Directory.GetFiles(@"Resources\Templates\Chemicals", "*.label"))
                    comboBox1.Items.Add(new Template<Chemical>(file));


            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            listBox1.DataSource = ChemicalDB;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ChemicalDB.AddNew();
        }

        bool canLoad = true;
        void OpenFromChemical(Chemical Chemical)
        {
            if (canLoad)
            {
                Template<Chemical> template = (Template<Chemical>)comboBox1.SelectedItem;

                textBox1.Text = Chemical.Name;
                richTextBox1.Text = Chemical.Description;

                pictureBox1.Image = Dymo.Render(template, Chemical);
            }
        }

        void SaveToChemical(Chemical Chemical)
        {
            canLoad = false;
            Chemical.Name = textBox1.Text;
            Chemical.Description = richTextBox1.Text;
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
                ChemicalDB.Remove(selectedChemical);
        }
    }
}
