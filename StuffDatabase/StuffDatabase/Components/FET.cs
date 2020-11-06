using DYMO.Label.Framework;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StuffDatabase.Components
{
    public class FET : BaseComponent, ILabelConvertable
    {
        [Category("Specifics")]
        public double Pd { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double Vds { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double Vgs { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double Vgsth { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double Id { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public double Rds { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
        [Category("Specifics")]
        public string Package { get { return GetPar<string>(""); } set { SetPar<string>(value); } }


        string Spacing(double var, int space = 8)
        {
            string hrd = Ext.ToHumanReadable(var, 0);
            int i = space - Regex.Match(hrd, @"[\d\.]+").Value.Length;
            return new string(' ', i) + hrd;
        }

        public void PopulateLabel(ILabel label)
        {
            foreach (DYMO.Label.Framework.ILabelObject labelObj in label.Objects)
            {
                string name = labelObj.Name;
                Match m = Regex.Match(name, @"([^_ \r\n]+)");

                if (m.Success)
                {
                    switch (m.Groups[1].Value)
                    {
                        
                        case "ID":
                            label.FillLabelObject(name, $"ID  {Spacing(Id)}A");
                            break;

                        case "PD":
                            label.FillLabelObject(name, $"PD  {Spacing(Pd)}W");
                            break;

                        case "VDS":
                            label.FillLabelObject(name, $"VDS {Spacing(Vds)}V");
                            break;

                        case "VGS":
                            label.FillLabelObject(name, $"VGS {Spacing(Vgs)}V");
                            break;

                        case "RDS":
                            label.FillLabelObject(name, $"RDS {Spacing(Rds)}Ω");
                            break;
                            
                        default:
                            PropertyInfo propInfo = this.GetType().GetProperty(m.Groups[1].Value);
                            if (propInfo != null)
                            {
                                string txt = propInfo.GetValue(this).ToString();
                                label.FillLabelObject(name, txt);
                            }
                            break;


                    }
                }
            }
        }

        public static FET ParseFromAlltransistor(string page)
        {
            FET fet = new FET();

            fet.Name = ParseString(page, "Type Designator: (.+)");
            string type = ParseString(page, "Type of Transistor: (.+)");
            string channel = ParseString(page, "Type of Control Channel: (.+)");
            fet.Function = $"{type} {channel}";
            fet.Package = ParseString(page, "Package: (.+)");

            fet.Pd = ParseDouble(page, @"Maximum Power Dissipation[^\d]+([\d\.]+)");
            fet.Vds = ParseDouble(page, @"Maximum Drain-Source Voltage[^\d]+([\d\.]+)");
            fet.Vgs = ParseDouble(page, @"Maximum Gate-Source Voltage[^\d]+([\d\.]+)");
            fet.Vgsth = ParseDouble(page, @"Maximum Gate-Threshold Voltage[^\d]+([\d\.]+)");
            fet.Id = ParseDouble(page, @"Maximum Drain Current[^\d]+([\d\.]+)");
            fet.Rds = ParseDouble(page, @"Maximum Drain-Source On-State Resistance[^\d]+([\d\.]+)");




            return fet;
        }
    }
}
