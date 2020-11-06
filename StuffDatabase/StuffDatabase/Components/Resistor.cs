using System.ComponentModel;

namespace StuffDatabase.Components
{
    public class Resistor : BaseComponent
    {
        [Category("Specifics")]
        public double Resistance { get { return GetPar<double>(0); } set { SetPar<double>(value); } }

        [Category("Specifics")]
        public double Tolerance { get { return GetPar<double>(0); } set { SetPar<double>(value); } }

        [Category("Specifics")]
        public double Power { get { return GetPar<double>(0); } set { SetPar<double>(value); } }
    }
}
