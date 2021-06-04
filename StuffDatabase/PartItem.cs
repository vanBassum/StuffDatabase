using STDLib.Misc;
using System;
using System.ComponentModel;
using System.Linq;

namespace StuffDatabase
{
    public class PartItem : PropertySensitiveTypeDescriptor
    {
        [Category("Design")]
        public string Name { get => GetPar(""); set => SetPar(value); }
        //[Browsable(false)]
        //public PartType Type { get => GetPar<PartType>(); set => SetPar(value); }


        public PartItem()
        {

        }


        public PartItem(string name, PartType type)
        {
            Name = name;
            //Type = type;

            foreach (PartParameter par in type.Parameters)
                SetPar(TypeStringConverter.Types.FirstOrDefault(a => a.Type == par.Type).Create(), par.Name);
        }


        public override string ToString()
        {
            return Name;
        }
    }



}
