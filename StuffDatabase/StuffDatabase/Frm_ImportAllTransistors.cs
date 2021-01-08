using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffDatabase
{
    public partial class Frm_ImportAllTransistors : Form
    {
        WebClient wc = new WebClient();
        //public BaseComponent NewComponent { get; private set; }
        public Frm_ImportAllTransistors()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            string page = wc.DownloadString(new Uri(url));


            //if (url.ToLower().Contains("mosfet"))
            //    NewComponent = FET.ParseFromAlltransistor(page);
            //else
            //    NewComponent = Transistor.ParseFromAlltransistor(page);
            //
            //if(NewComponent != null)
            //    propertyGrid1.SelectedObject = NewComponent;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Frm_ImportAllTransistors_Load(object sender, EventArgs e)
        {

        }
    }
}
