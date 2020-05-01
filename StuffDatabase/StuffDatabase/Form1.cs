using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STDLib.Saveable;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {

        SaveableBindingList<Component> chemicals = new SaveableBindingList<Component>(@"Resources\components.json");


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chemicals.Save();
        }
    }
}
