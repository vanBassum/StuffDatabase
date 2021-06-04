using STDLib.Misc;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace StuffDatabase
{
    public class PartParameter : PropertySensitive
    {
        public string Name { get => GetPar(""); set => SetPar(value); }

        [TypeConverter(typeof(TypeStringConverter))]
        public Type Type { get => GetPar<Type>(); set => SetPar(value); }




        public override string ToString()
        {
            return Name;
        }
    }



}
