using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace StuffDatabase
{

    public class LookupTable<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        Dictionary<T1, T2> dict1 = new Dictionary<T1, T2>();
        Dictionary<T2, T1> dict2 = new Dictionary<T2, T1>();

        public void Add(T1 t1, T2 t2)
        {
            dict1.Add(t1, t2);
            dict2.Add(t2, t1);
        }

        

        public T1 Lookup(T2 t2)
        {
            return dict2[t2];
        }

        public T2 Lookup(T1 t1)
        {
            return dict1[t1];
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            foreach (var kvp in dict1)
                yield return kvp;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var kvp in dict1)
                yield return kvp;
        }
    }


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



    public class TypeSelectionEditor : UITypeEditor
    {
        List<Type> types = new List<Type> { 
            typeof(string),
            typeof(int),
        };

        public override bool IsDropDownResizable => true;
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            RadioButtonPanel cb = new RadioButtonPanel();
            cb.DisplayMember = nameof(Type.Name);
            cb.DataSource = types;
            IWindowsFormsEditorService edSvc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (edSvc != null)
            {
                edSvc.DropDownControl(cb);
                value = cb.SelectedItem;
            }
            return value;
        }
    }


    public class RadioButtonPanel : FlowLayoutPanel
    {
        public object SelectedItem { get; private set; } = null;
        public string DisplayMember { get; set; }
        IList _DataSource;
        public IList DataSource
        {
            get => _DataSource;
            set
            {
                _DataSource = value;
                Populate();
            }
        }

        void Populate()
        {
            this.Controls.Clear();
            foreach(object o in DataSource)
            {
                RadioButton btn = new RadioButton();
                btn.CheckedChanged += Btn_CheckedChanged;
                btn.Tag = o;
                if(o.GetType().GetProperty(DisplayMember) is PropertyInfo pi)
                    btn.Text = pi.GetValue(o).ToString();
                else
                    btn.Text = o.ToString();
                this.Controls.Add(btn);
            }
        }

        private void Btn_CheckedChanged(object sender, EventArgs e)
        {
            if(sender is RadioButton btn)
            {
                if (btn.Checked)
                    SelectedItem = btn.Tag;
                else
                    SelectedItem = null;
            }
        }
    }
}
