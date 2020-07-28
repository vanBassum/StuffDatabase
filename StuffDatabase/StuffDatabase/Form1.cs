﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STDLib.Saveable;

namespace StuffDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Settings.Load("Settings.json");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save("Settings.json");
        }
    }

}
