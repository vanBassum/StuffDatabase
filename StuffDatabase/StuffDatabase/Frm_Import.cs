using STDLib.Saveable;
using StuffDatabase.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffDatabase
{
    public partial class Frm_Import : Form
    {
        SaveableBindingList<BaseComponent> Components { get; set; }
        Type CompType { get; set; }
        public bool OverrideExisting = false;

        public Frm_Import(SaveableBindingList<BaseComponent> components, Type compType)
        {
            InitializeComponent();
            Components = components;
            CompType = compType;
        }

        private void Frm_Import_Load(object sender, EventArgs e)
        {
            CTRL_Component comp = new CTRL_Component(CompType);
            panel1.Controls.Add(comp);
            comp.Dock = DockStyle.Fill;
            comp.Components = Components;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OverrideExisting = true;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OverrideExisting = false;
            this.DialogResult = DialogResult.OK;
        }
    }
}
