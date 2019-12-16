using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PODUtils
{
    public class Logger
    {
        private static bool _isInitialized;
        private static String _logFileName;

        public enum LOGGING_LEVEL { ERROR_HARD, ERROR_SOFT, WARNING_HIGH, WARNING_LOW, INFO };

        public static void Initialize(String logfileName)
        {
            _logFileName = logfileName;
            _isInitialized = true;
        }

        public static void logMessage(LOGGING_LEVEL logLevel, String message)
        {

        }


    }
}
