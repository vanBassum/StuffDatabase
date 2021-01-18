using System;

namespace StuffDatabase
{
    /// <summary>
    /// Represents the definition of a dynamic property which can be added to an object at runtime.
    /// </summary>
    /// 
    public class DynamicProperty
    {
        public string PropertyName { get; set; }
        public Types Type { get; set; }

        public Type GetSystemType()
        {
            switch (Type)
            {
                case Types.STRING: return typeof(string);
                case Types.DOUBLE: return typeof(double);
            }
            throw new Exception("Unknown type");
        }

        public enum Types
        {
            STRING,
            DOUBLE,
        }
    }
   
}
