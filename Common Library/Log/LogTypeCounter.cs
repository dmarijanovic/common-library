using System;
using System.Collections.Generic;
using System.Text;

namespace DamirM.CommonLibrary
{
    public static class LogTypeCounter
    {
        static int ErrorCounter = 0;
        static int WarningCounter = 0;
        public static int Increment(Log.LogType logType)
        {
            int number = 0;
            if (logType == Log.LogType.ERROR)
            {
                number = ++ErrorCounter;
            }
            else if (logType == Log.LogType.WARNING)
            {
                number = ++WarningCounter;
            }
            return number;
        }
        public static int Error
        {
            get { return ErrorCounter; }
        }
        public static int Warning
        {
            get { return WarningCounter; }
        }

    }

}
