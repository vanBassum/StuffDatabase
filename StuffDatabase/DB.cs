using STDLib.Saveable;
using STDLib.Serializers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace StuffDatabase
{
    public class DB : Saveable
    {
        public string Filename { get; } = "db.json";
        public BindingList<PartType> Types { get; } = new BindingList<PartType>();
        public BindingList<PartItem> Items { get; } = new BindingList<PartItem>();

        public DB(string file)
        {
            Filename = file;
            //Types.ListChanged += Types_ListChanged;
            //Items.ListChanged += Items_ListChanged;
        }


        public void Save()
        {
            this.Save(Filename);
        }

        public void Load()
        {
            this.Load(Filename);
        }
    }
}
