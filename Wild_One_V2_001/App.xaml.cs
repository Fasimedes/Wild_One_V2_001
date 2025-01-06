using System.Windows;
using System.Windows.Threading;
using SOSCSRPG.Core;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handles unhandled exceptions in the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Create a message with the exception details
            string exceptionMessageText = $"An exception occurred: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";

            // Log the exception using the LoggingService
            LoggingService.Log(e.Exception);

            // TODO: Create a Window to display the exception information.
            // Display the exception message in a message box
            MessageBox.Show(exceptionMessageText, "Unhandled Exception", MessageBoxButton.OK);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}