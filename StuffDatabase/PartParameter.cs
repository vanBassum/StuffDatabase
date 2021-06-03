using STDLib.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace StuffDatabase
{
    public class PartParameter : PropertySensitive
    {
        public string Name { get => GetPar(""); set => SetPar(value); }

        [Editor(typeof(ListEditor), typeof(UITypeEditor))]
        public Type Type { get => GetPar<Type>(); set => SetPar(value); }




        public override string ToString()
        {
            return Name;
        }
    }

    internal class ListEditor : UITypeEditor
    {
        List<Type> types = new List<Type> { 
            typeof(string),
            typeof(int),
        };

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            ComboBox cb = new ComboBox();
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



}
