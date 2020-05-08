using STDLib.Misc;
using System;

namespace StuffDatabase
{
    public class Component : PropertySensitive
    {
        public string Name { get { return GetPar("new Component"); } set { SetPar(value); MaxNameLength = Math.Max(MaxNameLength, value.Length); } }
        public string Function { get { return GetPar("new Function"); } set { SetPar(value); } }
        public string Description { get { return GetPar(""); } set { SetPar(value); } }
        public string Datasheet { get { return GetPar(""); } set { SetPar(value); } }

        private static int MaxNameLength { get; set; }


        public override string ToString()
        {
            string space = (MaxNameLength - Name.Length) > 0 ? new string(' ', MaxNameLength - Name.Length) : " ";
            return Name.TrimEnd(' ') + space + Description.Replace('\n', ' ').TrimStart(' ');
        }
    }
}
