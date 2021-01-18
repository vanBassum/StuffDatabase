using STDLib.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StuffDatabase
{
    public partial class TabPageCTRL : UserControl
    {
        public event EventHandler<object> ObjectChanged;

        IBindingList _DataSource;
        public IBindingList DataSource { get => _DataSource; set 
            { 
                _DataSource = value; 
                collectionEditControl1.DataSource = _DataSource;
                Draw();
            } }

        Descriptor _Descriptor;
        public Descriptor Descriptor { get => _Descriptor; set 
            {
                _Descriptor = value;
                collectionEditControl1.CreateObject = () => Part.Create(_Descriptor);
                string templateDir = Path.Combine(Settings.Items.DataFolder, _Descriptor.Name, "Dymo");
                Directory.CreateDirectory(templateDir);
                comboBox1.Items.Clear();
                foreach(string file in Directory.GetFiles(templateDir, "*.label"))
                {
                    comboBox1.Items.Add(new Template(file));
                }
                if (comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                    Draw();
                }
            } }

        public TabPageCTRL()
        {
            InitializeComponent();
        }

        private void TabPageCTRL_Load(object sender, EventArgs e)
        {
            collectionEditControl1.DisplayMember = nameof(Part.Name);
            collectionEditControl1.ObjectChanged += CollectionEditControl1_ObjectChanged;
            collectionEditControl1.SelectedItemChanged += (a,b) => Draw();
            comboBox1.SelectedIndexChanged += (a, b) => Draw();
        }

        void Draw()
        {

            if(collectionEditControl1.SelectedObject is Part part && comboBox1.SelectedItem is Template template)
            {
                pictureBox1.Image = Dymo.Render(template, part);
            }
        }


        private void CollectionEditControl1_ObjectChanged(object sender, object e)
        {
            Draw();
            ObjectChanged(this, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (collectionEditControl1.SelectedObject is Part part && comboBox1.SelectedItem is Template template)
            {
                Dymo.Print(template, part);
            }
        }
    }

}
