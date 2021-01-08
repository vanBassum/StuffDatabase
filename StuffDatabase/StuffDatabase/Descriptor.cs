using System;
using System.Collections.Generic;

namespace StuffDatabase
{
    public class Descriptor
    {
        public string Name { get; set; }
        public List<DynamicProperty> Properties { get; set; } = new List<DynamicProperty>();

        public override string ToString()
        {
            return Name;
        }
    }

}
