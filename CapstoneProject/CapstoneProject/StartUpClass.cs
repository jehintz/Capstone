using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CapstoneProject
{
    //Application starts here before opening the Main Window. Made this so there can be a last-change error handler around Main()
    class StartUpClass
    {
        [System.STAThreadAttribute()]
        static void Main()
        {
            try
            {
                App.Main();
            }
            //If an exception happens anywhere else in the application, this will execute before termination or to catch and log the error
            catch (Exception ex)
            {
                //Send the exception through to the ExceptionHandler
                ExceptionHandler handler = new ExceptionHandler(ex);

                //Display a message box so the user knows an unexpected error has occured
                MessageBox.Show("An exception has occured. Please see ErrorLog.txt for more information.\n\"" + ex.Message + "\"");
            }
        }
    }
}
