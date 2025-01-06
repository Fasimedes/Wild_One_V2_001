using System;
using System.IO;
namespace SOSCSRPG.Core
{
    /// <summary>
    /// Static class for logging exceptions to a file.
    /// </summary>
    public static class LoggingService
    {
        // Directory where log files will be stored
        private const string LOG_FILE_DIRECTORY = "Logs";

        /// <summary>
        /// Static constructor to ensure the log directory exists.
        /// </summary>
        static LoggingService()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE_DIRECTORY);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Logs the specified exception to a log file.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="isInnerException">Indicates if the exception is an inner exception.</param>
        public static void Log(Exception exception, bool isInnerException = false)
        {
            using (StreamWriter sw = new StreamWriter(LogFileName(), true))
            {
                sw.WriteLine(isInnerException ? "INNER EXCEPTION" : $"EXCEPTION: {DateTime.Now}");
                sw.WriteLine(new string(isInnerException ? '-' : '=', 40));
                sw.WriteLine($"{exception.Message}");
                sw.WriteLine($"{exception.StackTrace}");
                sw.WriteLine(); // Blank line, to make the log file easier to read
            }

            // Log inner exception if it exists
            if (exception.InnerException != null)
            {
                Log(exception.InnerException, true);
            }
        }

        /// <summary>
        /// Generates the log file name based on the current date.
        /// </summary>
        /// <returns>The log file name.</returns>
        private static string LogFileName()
        {
            // This will create a separate log file for each day.
            // Not that we're hoping to have many days of errors.
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE_DIRECTORY,
                $"SOSCSRPG_{DateTime.Now:yyyyMMdd}.log");
        }
    }
}