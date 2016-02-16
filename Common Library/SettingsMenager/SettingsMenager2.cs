using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public class SettingsMenager2
    {
        const string XPATH_SELECT_ROOT = "/Settings";
        const string XPATH_SELECT_APPLICATION = "/Settings/Application";
        const string XPATH_SELECT_MODULE = "/Settings/Module";
        const string XPATH_SELECT_SETTING = "/Settings/Module/Setting";
        const string XPATH_SELECT_SETTING_VALUE = "Value";
        const string XPATH_SELECT_SETTING_DEFAULT = "Default";

        public const int VERSION = 1;

        ObjectCollections objectCollections;
        string moduleName;
        string moduleHardName;
        string moduleVarsion;

        /// <summary>
        /// This event is run when new data is available. In this method put all the settings that you want to save
        /// </summary>
        public event EventHandler Refresh;
        /// <summary>
        /// This event is run before all the data stored in file. In this method put all the settings that you want to save
        /// </summary>
        public event EventHandler Update;


        public enum Type
        {
            Text, Integer, Bool, Textbox, Combobox, ListBox, Checkbox, Other
        }

        public SettingsMenager2(string moduleName, string moduleHardName, string moduleVersion)
        {
            this.objectCollections = new ObjectCollections();
            this.moduleName = moduleName;
            this.moduleHardName = moduleHardName;
            this.moduleVarsion = moduleVersion;

        }

        public void LoadSetting(System.Windows.Forms.Control control, string hardName, string defaultValue)
        {
            SettingsMenagerStructure2 defaultSetting;
            foreach (SettingsMenagerStructure2 setting in Items)
            {
                if (setting.HardName == hardName)
                {
                    setting.SetValueToControl(control);
                    return;
                }
            }

            if (defaultValue != null)
            {
                // control not find in collection, set default value
                defaultSetting = new SettingsMenagerStructure2(control, hardName);
                defaultSetting.SetValueToControl(control, defaultValue);
            }
        }
        public string LoadSetting(string hardName, string defaultValue)
        {
            foreach (SettingsMenagerStructure2 setting in Items)
            {
                if (setting.HardName == hardName)
                {
                    return setting.Value;
                }
            }

            return defaultValue;
        }

        public void SaveToFile(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration;
            XmlComment xmlCommentElement;
            XmlElement rootNode;
            XmlElement applicationElement;
            XmlElement moduleNode;
            XmlElement settingNode;
            XmlElement settingValueElement;
            XmlElement settingDefaultValueElement;


            try
            {
                Log.Write(new string[] { "Loading settings...", path }, this, "SaveToFile", Log.LogType.DEBUG);
                if (Update != null)
                {
                    Update(this, new EventArgs());
                }

                xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);

                // create comment elemnt
                xmlCommentElement = xmlDocument.CreateComment("Generic settings file");
                xmlDocument.InsertBefore(xmlCommentElement, xmlDocument.DocumentElement);

                // create root element and add it to xml document
                rootNode = xmlDocument.CreateElement("Settings");
                rootNode.SetAttribute("version", VERSION.ToString());
                xmlDocument.AppendChild(rootNode);


                // create application element
                applicationElement = xmlDocument.CreateElement("Application");
                applicationElement.SetAttribute("version", System.Windows.Forms.Application.ProductVersion);
                applicationElement.InnerText = System.Windows.Forms.Application.ProductName;
                rootNode.AppendChild(applicationElement);

                // create module noode
                moduleNode = xmlDocument.CreateElement("Module");
                moduleNode.SetAttribute("name", this.moduleName);
                moduleNode.SetAttribute("hardname", this.moduleHardName);
                moduleNode.SetAttribute("version", this.moduleVarsion);

                foreach (SettingsMenagerStructure2 setting in Items)
                {
                    // create setting noode
                    settingNode = xmlDocument.CreateElement("Setting");
                    settingNode.SetAttribute("name", setting.Name);
                    settingNode.SetAttribute("hardname", setting.HardName);
                    settingNode.SetAttribute("type", setting.Type.ToString());
                    moduleNode.AppendChild(settingNode);

                    // create value element
                    settingValueElement = xmlDocument.CreateElement("Value");
                    settingValueElement.InnerText = setting.Value;
                    settingNode.AppendChild(settingValueElement);

                    // create default element
                    settingDefaultValueElement = xmlDocument.CreateElement("Default");
                    settingDefaultValueElement.InnerText = setting.DefaultValue;
                    settingNode.AppendChild(settingDefaultValueElement);
                }
                rootNode.AppendChild(moduleNode);

                xmlDocument.Save(path);
            }
            catch (Exception ex)
            {
                Log.Write(ex, this, "SaveToFile", Log.LogType.ERROR);
            }

        }

        /// <summary>
        /// Load settings in memory and then call refresh event
        /// </summary>
        /// <param name="path">Path to xml file</param>
        public void LoadFromFile(string path)
        {
            XmlDocument xmlDocument;
            XmlNodeList xmlNodeList;
            XmlNode xmlNode;
            XmlNode valueElement;
            XmlNode defaultValueElement;
            SettingsMenagerStructure2 setting;
            SettingsMenager2.Type type;

            int fileVersion;

            try
            {

                Log.Write(new string[] { "Loading settings...", path }, this, "LoadFromFile", Log.LogType.DEBUG);

                // load file in xml document DOM
                xmlDocument = new XmlDocument();
                xmlDocument.Load(path);

                // select root node, get settings version
                xmlNode = xmlDocument.SelectSingleNode(XPATH_SELECT_ROOT);
                fileVersion = int.Parse(xmlNode.Attributes["version"].Value);
                // log warning
                if (fileVersion != VERSION)
                {
                    Log.Write("Version of the files different than expected, attempting to load...", this, "LoadFromFile", Log.LogType.WARNING);
                }

                xmlNodeList = xmlDocument.SelectNodes(XPATH_SELECT_SETTING);

                // clear items before load
                Items.Clear();
                foreach (XmlNode settingNode in xmlNodeList)
                {
                    valueElement = settingNode.SelectSingleNode(XPATH_SELECT_SETTING_VALUE);
                    defaultValueElement = settingNode.SelectSingleNode(XPATH_SELECT_SETTING_DEFAULT);
                    type = GetTypeFromString(settingNode.Attributes["type"].Value);
                    setting = new SettingsMenagerStructure2(settingNode.Attributes["name"].Value, settingNode.Attributes["hardname"].Value, valueElement.InnerText, defaultValueElement.InnerText, type);

                    Items.Add(setting);
                }

                // Call refresh event to reflect settings on control
                if (Refresh != null)
                {
                    Refresh(this, new EventArgs());
                }
                
            }
            catch (Exception ex)
            {
                Log.Write(ex, this, "LoadFromFile", Log.LogType.ERROR);
            }
        }

        public Type GetTypeFromString(string typetext)
        {
            if (typetext.ToLower().Equals(Type.Bool))
            {
                return Type.Bool;
            }
            else if (typetext.ToLower().Equals(Type.Checkbox))
            {
                return Type.Checkbox;
            }
            else if (typetext.ToLower().Equals(Type.Combobox))
            {
                return Type.Combobox;
            }
            else if (typetext.ToLower().Equals(Type.Integer))
            {
                return Type.Integer;
            }
            else if (typetext.ToLower().Equals(Type.ListBox))
            {
                return Type.ListBox;
            }
            else if (typetext.ToLower().Equals(Type.Text))
            {
                return Type.Text;
            }
            else if (typetext.ToLower().Equals(Type.Textbox))
            {
                return Type.Textbox;
            }
            else
            {
                return Type.Other;
            }
        }

        public ObjectCollections Items
        {
            get { return this.objectCollections; }
        }



        public class ObjectCollections
        {
            ArrayList list;
            public ObjectCollections()
            {
                this.list = new ArrayList();
            }

            public void Add(SettingsMenagerStructure2 setting)
            {
                SettingsMenagerStructure2 itemSetting;

                for (int i = 0; i < list.Count; i++)
                {
                    itemSetting = (SettingsMenagerStructure2)list[i];
                    if (itemSetting.HardName == setting.HardName)
                    {
                        list[i] = setting;
                        return;
                    }
                }
                list.Add(setting);
            }

            public void Clear()
            {
                list.Clear();
            }

            public IEnumerator<SettingsMenagerStructure2> GetEnumerator()
            {
                foreach (SettingsMenagerStructure2 setting in list)
                {
                    yield return setting;
                }
            }

        }


    }
}
