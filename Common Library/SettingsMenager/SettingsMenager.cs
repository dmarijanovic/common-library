using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public class SettingsMenager
    {
        ArrayList list;
        private string path;
        private string groupOf;

        public SettingsMenager(string path, string groupOf)
        {
            list = new ArrayList();
            this.path = path;
            this.groupOf = groupOf;
        }

        public SettingsMenager()
        {
            list = new ArrayList();
        }

        public void Add(System.Windows.Forms.Control control, string defaultText)
        {
            //if (typeof(System.Windows.Forms.TextBox).Equals(control.GetType()))
            list.Add(new SettingsMenagerStructure(control, defaultText));
        }
        public void Add(System.Windows.Forms.Control control)
        {
            //if (typeof(System.Windows.Forms.TextBox).Equals(control.GetType()))
            list.Add(new SettingsMenagerStructure(control, ""));
        }
        public void SaveSettings()
        {
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(this.path, writerSettings);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Templates");
            xmlWriter.WriteStartElement("Template");
            // Add new attribute
            xmlWriter.WriteStartAttribute("Name");
            xmlWriter.WriteString(this.groupOf);
            xmlWriter.WriteEndAttribute();
            foreach (SettingsMenagerStructure node in list)
            {
                // Template element
                xmlWriter.WriteStartElement("Data");
                // Add new attribute
                xmlWriter.WriteStartAttribute("Type");
                xmlWriter.WriteString(node.type.ToString());
                xmlWriter.WriteEndAttribute();
                // Add new attribute
                xmlWriter.WriteStartAttribute("Name");
                xmlWriter.WriteString(node.name);
                xmlWriter.WriteEndAttribute();
                // Add new attribute
                xmlWriter.WriteStartAttribute("Description");
                xmlWriter.WriteString("");
                xmlWriter.WriteEndAttribute();
                // Add body text to elemnt
                xmlWriter.WriteString(node.Text);
                xmlWriter.WriteEndElement();

            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();


            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }
        public bool LoadSettings()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodeList = xmlDoc.SelectNodes("Templates/Template/Data");
            bool result = false;
            try
            {

                xmlDoc.Load(path);
                foreach (XmlNode node in nodeList)
                {
                    foreach (SettingsMenagerStructure item in list)
                    {
                        if (node.Attributes["Name"].Value == item.name)
                        {
                            if (item.control.GetType() == typeof(System.Windows.Forms.CheckBox))
                            {
                                // Is checkBox, then is checked or not 
                                if (node.InnerText.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    ((System.Windows.Forms.CheckBox)item.control).Checked = true;
                                }
                                else
                                {
                                    ((System.Windows.Forms.CheckBox)item.control).Checked = false;
                                }
                            }
                            else if (item.control.GetType() == typeof(System.Windows.Forms.ComboBox))
                            {
                                // Is listBox, then select index
                                int i;
                                Log.Write(node.InnerText);
                                i = int.Parse(node.InnerText);
                                ((System.Windows.Forms.ComboBox)item.control).SelectedIndex = i;

                            }
                            else
                            {
                                item.control.Text = node.InnerText;
                            }
                            item.isSet = true;
                        }
                    }
                }
                if (nodeList.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex, this, "LoadSettings", Log.LogType.ERROR);
            }
            foreach (SettingsMenagerStructure settingMenagerStructure in list)
            {
                if (!settingMenagerStructure.isSet)
                {
                    if (settingMenagerStructure.control.GetType() == typeof(System.Windows.Forms.CheckBox))
                    {
                    }
                    else if (settingMenagerStructure.control.GetType() == typeof(System.Windows.Forms.ComboBox))
                    {

                    }
                    else
                    {
                        settingMenagerStructure.control.Text = settingMenagerStructure.defaultText;
                    }
                }
            }

            return result;
        }

        public string GroupOf
        {
            set
            {
                this.groupOf = value;
            }
        }

        public string FilePath
        {
            set
            {
                this.path = value;
            }
        }
    }
}
