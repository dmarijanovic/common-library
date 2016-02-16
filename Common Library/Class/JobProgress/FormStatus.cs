using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DamirM.CommonLibrary;

namespace DamirM.CommonLibrary
{
    public partial class JobProgress : Form
    {
        private Thread thread;
        private static EventArgsStatusChange e;
        public static JobProgress form = null;
        private static DateTime lastMessageTime;
        private static bool threadActive = false;
        private const string constDefText = "Ready";
        public delegate void delStatusChange(object sender, EventArgsStatusChange e);
        private delegate void EventHandlerVoid();
        public event delStatusChange StatusChange;

        private Form owner;

        public JobProgress(Form owner)
        {
            InitializeComponent();
            this.owner = owner;
            JobProgress.form = this;
            this.CreateControl();
            //this.Show(owner);
        }
        private void ShowMe()
        {
            this.Show(owner);
        }
        private void ShowFormThreadCounter()
        {
            thread = new Thread(TimerShowForm);
            if (threadActive == false)
            {
                thread.Start();
            }
        }
        public static void Start()
        {
            Log.Write("Start JobProgress...", typeof(JobProgress), "Start", Log.LogType.DEBUG);
            e = new EventArgsStatusChange("Starting");
            lastMessageTime = DateTime.Now;
            JobProgress.form.ShowFormThreadCounter();
        }

        public static void Write(string text)
        {
            Log.Write(text, typeof(JobProgress), "Write", Log.LogType.DEBUG);
            //lastMessageTime = DateTime.Now;
            form.label1.Text = text;
            e.Message = text;
            OnStatusChange(e);
        }

        public static void Ready()
        {
            Log.Write("End JobProgress...", typeof(JobProgress), "Ready", Log.LogType.DEBUG);
            e.Message = constDefText;
            OnStatusChange(e);
        }

        private static void OnStatusChange(EventArgsStatusChange e)
        {
            if (JobProgress.form != null)
            {
                if (JobProgress.form.StatusChange != null)
                {
                    JobProgress.form.StatusChange(JobProgress.form, e);
                }
            }
            Application.DoEvents();
        }
        private void TimerShowForm()
        {
            JobProgress.threadActive = true;
            TimeSpan timeSpan = new TimeSpan(0, 0, 2);
            do
            {
                //Log.Write((DateTime.Now - JobProgress.lastMessageTime).ToString(), this, "TimerShowForm", Log.LogType.DEBUG);
                if ((DateTime.Now - JobProgress.lastMessageTime) > timeSpan)
                {
                    if (this.InvokeRequired)
                    {
                        //this.BeginInvoke(new EventHandler(ShowMe));
                        EventHandlerVoid method = new EventHandlerVoid(JobProgress.form.ShowMe);
                        this.Invoke(method);
                        Log.Write("Invoke", this, "TimerShowForm", Log.LogType.DEBUG);
                        //object result = Invoke(method);
                    }
                    else
                    {
                        Log.Write("Starting form", this, "TimerShowForm", Log.LogType.DEBUG);
                        this.Show();
                        Application.DoEvents();
                    }
                    break;
                }
                Application.DoEvents();
                Thread.Sleep(500);
            } while (true);
            JobProgress.threadActive = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }


    public class EventArgsStatusChange: EventArgs
    {
        private string message;
        public EventArgsStatusChange(string message)
        {
            this.message = message;
        }


        public string Message
        {
            get { return message; }
            set
            {
                message = value;
            }
        }

    }
}
