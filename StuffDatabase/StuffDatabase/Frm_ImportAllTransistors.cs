using StuffDatabase.Components;
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
        public Transistor NewTransistor { get; } = new Transistor();
        public Frm_ImportAllTransistors()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string page = wc.DownloadString(new Uri( textBox1.Text));


            Match m = Regex.Match(page, "Type Designator: (.+)");
            if (!m.Success)
                return;
            NewTransistor.Name = m.Groups[1].Value;

            m = Regex.Match(page, "Polarity: (.+)");
            if (!m.Success)
                return;
            NewTransistor.Function = m.Groups[1].Value;

            NewTransistor.PD = ParseVal(page, @"Collector Power Dissipation[^\d]+([\d\.]+)");
            NewTransistor.VCE = ParseVal(page, @"Collector-Emitter Voltage[^\d]+([\d\.]+)");
            NewTransistor.IC = ParseVal(page, @"Collector Current[^\d]+([\d\.]+)");
            NewTransistor.FT = ParseVal(page, @"Transition Frequency[^\d]+([\d\.]+)");
            NewTransistor.HFE = ParseVal(page, @"Forward Current Transfer Ratio[^\d]+([\d\.]+)");
            propertyGrid1.SelectedObject = NewTransistor;
        }



        double ParseVal(string page, string pattern)
        {
            Match m = Regex.Match(page, pattern);
            if (!m.Success)
                return 0;
            return double.Parse(m.Groups[1].Value);
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
