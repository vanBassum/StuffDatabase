using STDLib.Misc;
using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

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


        public static double ParseDouble(string page, string pattern)
        {
            Match m = Regex.Match(page, pattern);
            if (!m.Success)
                return 0;
            double d = double.Parse(m.Groups[1].Value.Replace('.',','));
            return d;
        }

        public static string ParseString(string page, string pattern)
        {
            Match m = Regex.Match(page, pattern);
            if (!m.Success)
                return null;
            return m.Groups[1].Value;
        }

        public void Populate(BaseComponent comp)
        {
            if (this.GetType() != comp.GetType())
                throw new Exception("Type mismatch");

            foreach(PropertyInfo pi in comp.GetType().GetProperties())
                pi.SetValue(this, pi.GetValue(comp));
        }
    }

}
