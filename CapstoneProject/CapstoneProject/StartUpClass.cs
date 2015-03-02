using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CapstoneProject
{
    class StartUpClass
    {
        [System.STAThreadAttribute()]
        static void Main()
        {
            try
            {
                App.Main();
            }
            catch (Exception ex)
            {
                ExceptionHandler handler = new ExceptionHandler(ex);
                MessageBox.Show("An exception has occured. Please see ErrorLog.txt for more information.\n\"" + ex.Message + "\"");
            }
        }
    }
}
