using STDLib.Misc;
using System;
using System.ComponentModel;

namespace StuffDatabase
{
    public class PartItem : PropertySensitiveTypeDescriptor
    {
        [Category("Design")]
        public string Name { get => GetPar(""); set => SetPar(value); }
        [Browsable(false)]
        public PartType Type { get => GetPar<PartType>(); set => SetPar(value); }


        public PartItem(string name, PartType type)
        {
            Name = name;
            Type = type;

            foreach (PartParameter par in type.Parameters)
                SetPar(Activator.CreateInstance(par.Type), par.Name);
        }


        public override string ToString()
        {
            return Name;
        }
    }



}
