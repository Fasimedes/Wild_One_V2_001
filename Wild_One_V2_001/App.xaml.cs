using System.Windows;
using System.Windows.Threading;
using SOSCSRPG.Services;

namespace WPFUI
{
    public partial class App : Application
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageText =
                $"An exception occurred: {e.Exception.Message}\r\n\r\nat: {e.Exception.StackTrace}";
            LoggingService.Log(e.Exception);
            // TODO: Create a Window to display the exception information.
            MessageBox.Show(exceptionMessageText, "Unhandled Exception", MessageBoxButton.OK);
        }
    }
}
//using System;
//using System.Windows;

//namespace Wild_One_V2_001
//{
//    public partial class App : Application
//    {
//        private void App_OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
//        {
//            // Handle the exception here
//            MessageBox.Show(e.Exception.Message, "Unhandled Exception");
//            e.Handled = true;
//        }
//    }
//}
