using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    class SettingsMenagerStructure
    {
        public Type type;
        public string name;
        private string text;
        public string defaultText;
        public bool isSet;
        public System.Windows.Forms.Control control;

        public SettingsMenagerStructure(System.Windows.Forms.Control control, string defaultText)
        {
            this.type = control.GetType();
            this.name = control.Name;
            this.text = control.Text;
            this.defaultText = defaultText;
            this.control = control;
        }
        public string Text
        {
            get
            {
                if (control.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    return ((System.Windows.Forms.CheckBox)control).Checked.ToString();
                }
                else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    return ((System.Windows.Forms.ComboBox)control).SelectedIndex.ToString();
                }
                else
                {
                    return control.Text;
                }

            }
        }
    }
}
