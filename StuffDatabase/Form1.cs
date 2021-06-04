using FRMLib;
using System;
using System.Collections;
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


            /*
            PartType pt = new PartType { Name = "Tor" };
            pt.Parameters.Add(new PartParameter { Name = "HFE", Type = typeof(int) });
            database.Types.Add(pt);

            PartItem pi = new PartItem("sometor", pt);
            database.Items.Add(pi);

            database.Save();
            */
        }


        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ListBox listbox)
                propertyGrid1.SelectedObject = listbox.SelectedItem;
        }


        
    }


    public class Engine : ORMObject
    { 
    
    }


    public class Car : ORMObject
    {
        public string Name { get; set; }
        public Engine Engine { get; set; }

    }

    public class MyDB : ORM
    {
        public DBList<Car> Cars { get; set; } = new DBList<Car>();
    }


    public class ORM
    { 


        void Save()
        {
            Dictionary<Type, Table> tables = new Dictionary<Type, Table>();

            foreach(var pi in this.GetType().GetProperties())
            {
                object o = pi.GetValue(this);
                if(o is DBList list)
                {
                    Do(tables, list);
                }
            }
        }

        void Do(Dictionary<Type, Table> tables, DBList list)
        {
            Table t;
            if (!tables.TryGetValue(list.GenericType, out t))
                tables.Add(list.GenericType, t = new Table());

            foreach (ORMObject obj in list)
            {
                t.Insert(obj);

                foreach (var pi in obj.GetType().GetProperties())
                {
                    object o = pi.GetValue(this);

                    switch(o)
                    {
                        case DBList l:
                            Do(tables, l);
                            break;
                        case ORMObject ob:
                            Do(tables, ob);
                            break;
                    }
                }
            }
        }


        void Do(Dictionary<Type, Table> tables, ORMObject obj)
        {
            Table t;
            if (!tables.TryGetValue(obj.GetType(), out t))
                tables.Add(obj.GetType(), t = new Table());

            t.Insert(obj);

            foreach (var pi in obj.GetType().GetProperties())
            {
                object o = pi.GetValue(this);

                switch (o)
                {
                    case DBList l:
                        Do(tables, l);
                        break;
                    case ORMObject ob:
                        Do(tables, ob);
                        break;
                }
            }
        }
    }


    public interface DBList : IEnumerable
    {
        Type GenericType { get; }
    }


    public class DBList<T> : DBList where T : ORMObject
    {
        List<T> list = new List<T>();
        public Type GenericType { get => typeof(T); }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }


    public class Table
    {
        List<ORMObject> objects = new List<ORMObject>();
        public void Insert(ORMObject obj)
        {
            if (obj.id < 0)
            {
                obj.id = objects.Count();
                objects.Add(obj);
            }
        }
    }
    /*
    public class Table<T> : Table where T : ORMObject
    {
    }
    */
    public class ORMObject
    {
        public int id = -1;
    }








}
