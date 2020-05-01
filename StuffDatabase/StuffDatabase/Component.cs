using STDLib.Misc;

namespace StuffDatabase
{
    public class Component : PropertySensitive
    {
        public string Name { get { return GetPar("new"); } set { SetPar(value); } }
        public string Description { get { return GetPar(""); } set { SetPar(value); } }

        public override string ToString()
        {
            return Name;
        }
    }
}
