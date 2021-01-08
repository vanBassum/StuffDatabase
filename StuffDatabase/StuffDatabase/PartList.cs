using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using STDLib.Saveable;
using STDLib.Serializers;

namespace StuffDatabase
{
    public class PartList : BindingList<Part>, ISaveable
    {
        Serializer serializer;
        string file = null;

        public PartList(string file)
        {
            this.serializer = new JSON();
            this.file = file;
            Load();
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            if (file == null)
                throw new Exception("Wrong initializer used, use SaveableBindingList(string file)");
            Save(file);
        }

        public void Load()
        {
            if (file == null)
                throw new Exception("Wrong initializer used, use SaveableBindingList(string file)");
            if (!File.Exists(file))
                return;
            Load(file);
        }

        public void Save(string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            using (Stream stream = File.Open(file, FileMode.Create, FileAccess.Write))
                Save(stream);
        }

        public void Load(string file)
        {
            if (!File.Exists(file))
                return;
            using (Stream stream = File.Open(file, FileMode.Open, FileAccess.Read))
                Load(stream);
        }

        public void Save(Stream stream)
        {
            serializer.Serialize<BindingList<Part>>(this, stream);
        }


        public void Load(Stream stream)
        {
            BindingList<Part> deserializedObject = serializer.Deserialize<BindingList<Part>>(stream);

            this.Clear();
            foreach (Part i in deserializedObject)
            {
                Part p = Part.Create(i.Descriptor, i);
                this.Add(p);
            }
        }
    }
}
