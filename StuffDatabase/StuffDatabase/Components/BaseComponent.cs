using STDLib.Misc;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;

namespace StuffDatabase.Components
{
    public abstract class BaseComponent : PropertySensitive
    {
        [Category("Base")]
        public string Name { get { return GetPar("new Component"); } set { SetPar(value); MaxNameLength = Math.Max(MaxNameLength, value.Length); } }
        [Category("Base")]
        public string Function { get { return GetPar("new Function"); } set { SetPar(value); } }
        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]
        [Category("Base")]
        public string Description { get { return GetPar(""); } set { SetPar(value); } }
        [Browsable(false)]
        public string Datasheet { get { return GetPar(""); } set { SetPar(value); } }



        private static int MaxNameLength { get; set; }
        public override string ToString()
        {
            string space = (MaxNameLength - Name.Length) > 0 ? new string(' ', MaxNameLength - Name.Length) : " ";
            return Name.TrimEnd(' ') + space + Description.Replace('\n', ' ').TrimStart(' ');
        }
    }

}
