using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.Odbc;
using System.IO;

namespace DamirM.CommonLibrary
{
    public partial class Log : Form
    {
        public static int saveDB = 1;
        private static bool saveLocal;
        private static Log activeLog = null;
        private int nextLogIndex = 0;
        private static string logPath;

        private static TextWriter tw = null;

        public delegate void delGenericLogMessage(LogEntery logEntery);
        public delegate void delLogTypeInfo(LogType logType, int number);

        public static event delLogTypeInfo OnLogTypeNumberChange;
        public static event delGenericLogMessage NewMessage;

        private static ArrayList log = new ArrayList();

        public enum LogType
        {
            ALL,DEBUG,INFO,ERROR,WARNING,UNKNOWN
        }


        public Log()
        {
            if (Log.activeLog == null)
            {
                InitializeComponent();
                cbFiltrType.Items.Add(LogType.ALL);
                cbFiltrType.Items.Add(LogType.DEBUG);
                cbFiltrType.Items.Add(LogType.ERROR);
                cbFiltrType.Items.Add(LogType.INFO);
                cbFiltrType.Items.Add(LogType.UNKNOWN);
                cbFiltrType.Items.Add(LogType.WARNING);
                cbFiltrType.SelectedIndex = 0;
                Log.activeLog = this;
            }
            else
            {
                this.Close();
            }
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            ShowLog(LogType.ALL);
        }

        private void ShowLog(LogType logType)
        {
            StringBuilder sb = new StringBuilder();
            txtLog.Text = "";
            foreach (LogEntery line in log)
            {
                if (tbSearch.Text != "")
                {
                    if (!(line.text.IndexOf(tbSearch.Text, StringComparison.InvariantCultureIgnoreCase) != -1 || line.dateTime.ToLongTimeString().IndexOf(tbSearch.Text) != -1 || (line.from != null ? line.from.IndexOf(tbSearch.Text) != -1 : true) || (line.action != null ? line.action.IndexOf(tbSearch.Text) != -1 : true)))
                    {
                        continue;
                    }
                }
                if (logType == line.logType || logType == LogType.ALL)
                {
                    if (line.text.IndexOf("\n") != -1)
                    {
                        sb.Append(string.Format("{0} [{1}] [{2}] [{3}] {4}\r\n", line.dateTime.ToLongTimeString(), line.logType, line.from, line.action, line.text.Trim().Equals("") ? "" : "\r\n" + line.text));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} [{1}] [{2}] [{3}] {4}\r\n", line.dateTime.ToLongTimeString(), line.logType, line.from, line.action, line.text));
                    }
                }
            }
            nextLogIndex = log.Count;
            txtLog.Text = sb.ToString();
            //txtLog.AppendText("Showing " + log.Count + " lines, " + sb.Length + " b");
        }
        private void ShowLog()
        {
            StringBuilder sb = new StringBuilder();
            LogType logType = (LogType)cbFiltrType.Items[cbFiltrType.SelectedIndex];
            LogEntery logEntery = null;
            for (int i = nextLogIndex; i < log.Count; i++)
            {
                logEntery = (LogEntery)log[i];
                if (logType == logEntery.logType || logType == LogType.ALL)
                {
                    if (tbSearch.Text != "")
                    {
                        if (!(logEntery.text.IndexOf(tbSearch.Text, StringComparison.InvariantCultureIgnoreCase) != -1 || logEntery.dateTime.ToLongTimeString().IndexOf(tbSearch.Text) != -1 || (logEntery.from != null ? logEntery.from.IndexOf(tbSearch.Text) != -1 : true) || (logEntery.action != null ? logEntery.action.IndexOf(tbSearch.Text) != -1 : true)))
                        {
                            continue;
                        }
                    }
                    if (logEntery.text.IndexOf("\n") != -1)
                    {
                        sb.Append(string.Format("{0} [{1}] [{2}] [{3}] {4}\r\n", logEntery.dateTime.ToLongTimeString(), logEntery.logType, logEntery.from, logEntery.action, logEntery.text.Trim().Equals("") ? "" : "\r\n" + logEntery.text));
                    }
                    else
                    {
                        sb.Append(string.Format("{0} [{1}] [{2}] [{3}] {4}\r\n", logEntery.dateTime.ToLongTimeString(), logEntery.logType, logEntery.from, logEntery.action,logEntery.text));
                    }
                }                
            }
            nextLogIndex = log.Count;
            txtLog.AppendText(sb.ToString());
        }

        public static void Write(string text)
        {
            LogEntery logEntery = new LogEntery(text);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }
        public static void Write(string text, LogType logType)
        {
            LogEntery logEntery = new LogEntery(text, logType);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }
        public static void Write(string text, string source, LogType logType)
        {
            LogEntery logEntery = new LogEntery(text, source, logType);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }

        public static void Write(object text, object source, string action, LogType logType)
        {
            LogEntery logEntery = new LogEntery(text, source.GetType().ToString(), action, logType);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }
        public static void Write(object text, Type type, string action, LogType logType)
        {
            LogEntery logEntery = new LogEntery(text, type.ToString(), action, logType);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }

        public static void Write(object text, object source, string action, LogType logType, bool showExternal)
        {
            LogEntery logEntery = new LogEntery(text, source.ToString(), action, logType, showExternal);
            log.Add(logEntery);
            NewLogDataEvent(logEntery);
        }
        public static void Write(object text, object source, string action, object logTypeObject, bool showMessage)
        {
            Write(text, source, action, LogTypeFromObject(logTypeObject), showMessage);
        }
        public static void Write(LogEntery logEntery)
        {
            // This call enters LogEntery directili in array, good for calling from external app
            log.Add(logEntery);
        }

        public static void Append(string text)
        {
            throw new Exception("TODO: nije implementirano");
        }
        private static void NewLogDataEvent(LogEntery logEntery)
        {
            if (activeLog != null)
            {
                activeLog.ShowLog();
            }
            if (saveLocal)
            {
                SaveToFile(logEntery);
            }
            if (NewMessage != null)
            {
                NewMessage(logEntery);
            }
            if (Log.OnLogTypeNumberChange != null)
            {
                Log.OnLogTypeNumberChange(logEntery.logType, LogTypeCounter.Increment(logEntery.logType));
            }
        }
        public static string WriteSQL(object sql)
        {
            Write(sql.ToString(),"NULL","Generic SQL Query", LogType.DEBUG);
            return sql.ToString();
        }
        private static void SaveToFile(LogEntery logEntery)
        {
            // TODO: add methot to handle new day
            if (tw != null)
            {
                tw.WriteLine(string.Format("{0} [{1}] [{2}] [{3}] {4}", logEntery.dateTime.ToLongTimeString(), logEntery.logType, logEntery.from, logEntery.action, logEntery.text.Trim().Equals("") ? "" : "\r\n" + logEntery.text));
            }
        }
        public static void SaveDB(string connectionString)
        {
            OdbcCommand odbcCommand;
            // Imjena za UberTools
            //OdbcConnection odbcConn = new System.Data.Odbc.OdbcConnection(Static.GetConnString());
            OdbcConnection odbcConn = new System.Data.Odbc.OdbcConnection(connectionString);
            if (saveDB == 1)
            {
                Log.Write("Spremam log u bazu... cya", "Log", "SaveDB", Log.LogType.ERROR);
                StringBuilder sb = new StringBuilder();
                sb.Append("insert into log (source, data, time) values ('app','");
                // to do baci u binarry data 
                foreach (LogEntery line in log)
                {
                    sb.Append(string.Format("{0} [{1}] [{2}] [{3}] {4}\r\n", DateTime.Now.ToLongTimeString(), line.logType, line.from, line.action, line.text.Trim().Equals("") ? "" : "\r\n" + line.text).Replace("'","\\'"));
                    Application.DoEvents();
                }
                sb.Append("','");
                sb.Append(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                sb.Append("')");
                odbcCommand = new OdbcCommand(sb.ToString(), odbcConn);
                try
                {
                    if (odbcConn.State != ConnectionState.Open)
                        odbcConn.Open();
                    int rez = odbcCommand.ExecuteNonQuery();

                }
                catch (Exception exc)
                {
                    Log.Write(exc, "Log", "SaveDB", Log.LogType.ERROR);
                }
                finally
                {
                    if (odbcConn.State == ConnectionState.Open)
                        odbcConn.Close();
                }
            }
        }

        private void cbFiltrType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowLog((LogType)cbFiltrType.Items[cbFiltrType.SelectedIndex]);
        }
        private void Log_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.activeLog = null;
        }
        private static LogType LogTypeFromObject(object text)
        {
            LogType logType = LogType.UNKNOWN;
            if (LogType.DEBUG.ToString() == text.ToString())
            {
                logType = LogType.DEBUG;
            }
            else if (LogType.ERROR.ToString() == text.ToString())
            {
                logType = LogType.ERROR;
            }
            else if (LogType.INFO.ToString() == text.ToString())
            {
                logType = LogType.INFO;
            }
            else if (LogType.WARNING.ToString() == text.ToString())
            {
                logType = LogType.WARNING;
            }

            return logType;
        }
        public static void StandardRunInfo()
        {
            Write(Application.ProductName + ", " + Application.ProductVersion, typeof(Log), "StandardRunInfo", Log.LogType.INFO);
            Write(Application.StartupPath, typeof(Log), "StandardRunInfo", Log.LogType.INFO);
            Write(string.Format("Machine: {0}\r\nOS Version: {1}\r\nProcessors: {2}\r\nUser name: {3}", Environment.MachineName, Environment.OSVersion.VersionString, Environment.ProcessorCount.ToString(), Environment.UserName), typeof(Log), "StandardRunInfo", Log.LogType.INFO);
        }
        public static void End()
        {
            if (tw != null)
            {
                tw.Close();
                tw = null;
            }
        }

        /// <summary>
        /// Remove all log enterys
        /// </summary>
        public static void Clear()
        {
            log.Clear();
            if (activeLog != null)
            {
                activeLog.txtLog.Text = "";
            }
        }
        public static bool SaveLocal
        {
            set
            {
                saveLocal = value;
                if (Log.saveLocal)
                {
                    string logName = string.Format("{0}_{1}.log", Application.ProductName, DateTime.Now.ToString("yyyy-MM-dd"));
                    logPath = Common.BuildPath(Application.StartupPath, "logs");
                    if (!Directory.Exists(logPath))
                    {
                        Directory.CreateDirectory(logPath);
                    }
                    if (tw == null)
                    {
                        tw = new StreamWriter(logPath + logName, true, Encoding.UTF8);
                    }
                }
            }
        }


        private void bSearch_Click(object sender, EventArgs e)
        {
            ShowLog((LogType)cbFiltrType.Items[cbFiltrType.SelectedIndex]);
        }




    }
}
