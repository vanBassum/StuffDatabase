using FRMLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {
        DB database = new DB("db.json");

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            database.Load();
            listBox1.DataSource = database.Items;
            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;



            menuStrip1.AddMenuItem("File/Save", () => database.Save());

            menuStrip1.AddMenuItem("Testing/Manage types", () => {
                CollectionEditDialog diag = new CollectionEditDialog();
                diag.DataSource = database.Types;
                diag.Show();
            });

            menuStrip1.AddMenuItem("Testing/add", () => {
                PartType type = database.Types.FirstOrDefault();
                PartItem item = new PartItem("newItem", type);
                database.Items.Add(item);
            });

        }


        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListBox listbox)
                propertyGrid1.SelectedObject = listbox.SelectedItem;
        }


        
    }



}
