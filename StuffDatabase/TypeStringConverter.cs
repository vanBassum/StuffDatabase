using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace StuffDatabase
{
    public class Ty
    { 
        public string Name { get; set; }
        public Type Type { get; set; }
        public Func<object> Create { get; set; }

        public Ty(string name, Type type, Func<object> create)
        {
            Name = name;
            Type = type;
            Create = create;
        }
    }

    public class TypeStringConverter : StringConverter
    {
        public static List<Ty> Types { get; } = new List<Ty>
        {
            new Ty( "string", typeof(string), ()=>"" ),
            new Ty( "int", typeof(int), ()=>0 ),
            new Ty( "double", typeof(double), ()=>0.0d ),
        };


        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(Types.Select(a=>a.Name).ToArray());
        }



        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            object result = null;

            if (value is string str)
                result = Types.FirstOrDefault(a => a.Name == str).Type;

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
                result = Types.FirstOrDefault(a => a.Type == type).Name;

            return result ?? base.ConvertFrom(context, culture, value);
        }

    }
}
