using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {
        DB database = new DB();

        public Form1()
        {
            InitializeComponent();
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            database.Load("db.json");
            listBox1.DataSource = database.Types;

            database.Types.Add(new PartType());
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListBox listbox)
                propertyGrid1.SelectedObject = listbox.SelectedItem;
        }



    }
}
