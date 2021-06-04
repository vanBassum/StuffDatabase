using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace StuffDatabase
{
    public class TypeStringConverter : StringConverter
    {

        LookupTable<string, Type> t = new LookupTable<string, Type>() { 
            { "string", typeof(string) },
            { "int", typeof(int) },
            { "double", typeof(double) },
        };



        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(t.Select(a=>a.Key).ToArray());
        }



        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object result = null;

            if(value is string str)
                result = t.Lookup(str);

            return result ?? base.ConvertFrom(context, culture, value);
        }


        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            object result = null;

            if (value is Type type && destinationType == typeof(string))
                result = t.Lookup(type);

            return result ?? base.ConvertFrom(context, culture, value);
        }

    }
}
