using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StuffDatabase
{

    public class DictionaryPropertyDescriptor : PropertyDescriptor
    {
        IDictionary _dictionary;
        object _key;

        public event EventHandler PropertyChanged;

        internal DictionaryPropertyDescriptor(IDictionary d, object key, object o)
            : base(key.ToString(), null)
        {
            _dictionary = d;
            _key = key;
        }

        public override Type PropertyType
        {
            get { return _dictionary[_key].GetType(); }
        }

        public override void SetValue(object component, object value)
        {
            _dictionary[_key] = value;
            PropertyChanged?.Invoke(this, null);
        }

        public override object GetValue(object component)
        {
            return _dictionary[_key];
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type ComponentType
        {
            get { return null; }
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override void ResetValue(object component)
        {
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
