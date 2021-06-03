using STDLib.Saveable;
using STDLib.Serializers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace StuffDatabase
{
    public class DB : Saveable
    { 
        public BindingList<PartType> Types { get; } = new BindingList<PartType>();
        public BindingList<PartItem> Items { get; } = new BindingList<PartItem>();


        

    }
}
