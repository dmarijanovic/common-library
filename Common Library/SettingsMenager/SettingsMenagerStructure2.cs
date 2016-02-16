using System;
using System.Collections.Generic;
using System.Text;
using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public class SettingsMenagerStructure2
    {
        SettingsMenager2.Type type;
        string name;
        string hardName;
        string value;
        string defaultValue;

        public SettingsMenagerStructure2(System.Windows.Forms.Control control, string hardName)
        {
            this.name = control.Name;
            this.hardName = hardName;
            this.type = GetType(control);
            this.value = GetValue(control);
            this.defaultValue = "";
        }

        public SettingsMenagerStructure2(string name, string hardName, string value, string defaultValue, SettingsMenager2.Type type)
        {
            this.name = name;
            this.hardName = hardName;
            this.type = type;
            this.value = value;
            this.defaultValue = defaultValue;
        }

        private SettingsMenager2.Type GetType(System.Windows.Forms.Control control)
        {
            if (control is System.Windows.Forms.TextBoxBase)
            {
                return SettingsMenager2.Type.Textbox;
            }
            else if (control.GetType() == typeof(System.Windows.Forms.CheckBox))
            {
                return SettingsMenager2.Type.Checkbox;
            }
            else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
            {
                return SettingsMenager2.Type.Combobox;
            }
            else
            {
                Log.Write("Unknown type: " + control.GetType(), this, "GetType", Log.LogType.WARNING);
                return SettingsMenager2.Type.Other;
            }
        }

        private string GetValue(System.Windows.Forms.Control control)
        {
            if (control is System.Windows.Forms.TextBoxBase)
            {
                return ((System.Windows.Forms.TextBoxBase)control).Text;
            }
            else if (control.GetType() == typeof(System.Windows.Forms.CheckBox))
            {
                return ((System.Windows.Forms.CheckBox)control).Checked.ToString();
            }
            else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
            {
                return ((System.Windows.Forms.ComboBox)control).SelectedIndex.ToString();
            }
            else
            {
                return "";
            }
        }
        public void SetValueToControl(System.Windows.Forms.Control control, string value)
        {
            try
            {
                if (control is System.Windows.Forms.TextBoxBase)
                {
                    control.Text = value;
                }
                else if (control.GetType() == typeof(System.Windows.Forms.CheckBox))
                {
                    if (value.ToLower().Equals("true"))
                    {
                        ((System.Windows.Forms.CheckBox)control).Checked = true;
                    }
                    else
                    {
                        ((System.Windows.Forms.CheckBox)control).Checked = false;
                    }
                }
                else if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                {

                    ((System.Windows.Forms.ComboBox)control).SelectedIndex = int.Parse(value);
                }
                else
                {
                    Log.Write("Type not found", this, "SetValueToControl", Log.LogType.WARNING);
                }
            }
            catch (Exception ex)
            {
                Log.Write(new string[] { "Control name: " + control.Name, "Control type:" + control.GetType().ToString(), "Value: " + value }, this, "SetValueToControl", Log.LogType.DEBUG);
                Log.Write(ex, this, "SetValueToControl", Log.LogType.ERROR);
            }
        }
        public void SetValueToControl(System.Windows.Forms.Control control)
        {
            SetValueToControl(control, this.value);
        }

        public SettingsMenager2.Type Type
        {
            get { return this.type; }
        }
        public string Name
        {
            get { return this.name; }
        }
        public string HardName
        {
            get { return this.hardName; }
        }
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public string DefaultValue
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

    }
}
