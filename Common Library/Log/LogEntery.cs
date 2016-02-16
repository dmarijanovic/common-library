using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace DamirM.CommonLibrary
{
    public class LogEntery
    {
        public Log.LogType logType;
        public string from;
        public string action;
        public string text;
        private string shortText = null;
        public DateTime dateTime;
        public bool showExternal;
        public LogEntery(object text, string from, string action, Log.LogType logType, bool showExternal)
        {
            this.text = ParseText(text, showExternal);
            this.from = from;
            this.action = action;
            this.logType = logType;
            this.dateTime = DateTime.Now;
            this.showExternal = showExternal;
            // Provjerava dali se tko prijavio na event, event vraca broj zapisa za odredeni LogType
        }
        public LogEntery(object text, string from, string action, Log.LogType logType) : this(text, from, action, logType, false) { }
        public LogEntery(object text, string from, Log.LogType logType) : this(text, from, null, logType, false) { }
        public LogEntery(object text, Log.LogType logType) : this(text, null, null, logType, false) { }
        public LogEntery(object text) : this(text, null, null, Log.LogType.UNKNOWN, false) { }

        private string ParseText(object obj, bool getShortText)
        {
            string result = "";
            if (obj == null)
            {
                result = "";
            }
            else if (typeof(string) == obj.GetType())
            {
                result = obj.ToString();
            }
            else if (typeof(ArrayList) == obj.GetType())
            {
                result = "Count: " + ((ArrayList)obj).Count + Environment.NewLine;
                foreach (object line in (ArrayList)obj)
                {
                    //if (typeof(string) == line.GetType())
                    //{
                    //    result += line.ToString() + Environment.NewLine;
                    //}
                    //else
                    //{
                    result += line.ToString() + Environment.NewLine;
                    //result += "No parser of type " + line.GetType() + Environment.NewLine;

                    //}
                }
            }
            else if (typeof(string[]) == obj.GetType())
            {
                foreach (string line in (string[])obj)
                {
                    result += line + Environment.NewLine;
                }
            }
            else if ((typeof(System.Data.Odbc.OdbcException) == obj.GetType()) || (obj is Exception))
            {
                Exception exc = (Exception)obj;
                result = result = string.Format("\t{0}\t\r\n{1}\r\n\r\n{2}", exc.Message, exc.GetType(), exc.StackTrace);
            }
            else if (obj is IEnumerable)
            {
                foreach (object item in (IEnumerable)obj)
                {
                    result = string.Concat(result, item, Environment.NewLine);
                }
            }
            else
            {
                //try
                //{
                //    Exception exc = (Exception)obj;
                //    if (obj != null)
                //    {
                //        result = string.Format("\t{0}\t\r\n{1}\r\n\r\n{2}", exc.Message, exc.GetType(), exc.StackTrace);
                //        if (getShortText)
                //        {
                //            this.shortText = exc.Message;
                //        }
                //    }
                //    else
                //    {
                //        result = "No parser of type " + obj.GetType() + obj.Equals(new Exception());
                //    }
                //}
                //catch (Exception exc)
                //{
                //    result = exc.Message;
                //}
                result = "No parser of type " + obj.GetType();
            }
            return result;
        }
        public string GetShortText()
        {
            if (shortText != null)
            {
                return this.shortText;
            }
            return this.text;
        }
        public string[] GetTextAsArray()
        {
            return this.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

}
