using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CapstoneProject
{
    class ExceptionHandler
    {
        //Constructor which takes an exception and prints useful information about it to a text file
        public ExceptionHandler(Exception ex)
        {
            //Write all exception messages and the StackTrace to a file for future reference
            using (StreamWriter sw = new StreamWriter("ErrorLog.txt", true))
            {
                sw.WriteLine();
                sw.WriteLine(DateTime.Now + ":");
                sw.WriteLine(ex.Message + GetInnerExceptions(ex));
                sw.WriteLine("--------------------------------");
                sw.WriteLine(ex.StackTrace);
            }
        }

        //METHOD: Retrieve and all InnerException messages from the incoming error
        private string GetInnerExceptions(Exception ex)
        {
            //If there is no InnerException, return an empty string. Since this method isn't called again, this will end any further recursion.
            if (ex.InnerException == null)
            {
                return "";
            }
            //Add the InnerException Message to the return string, then recursively call this method. When the final InnerException is reached,
            //each InnerException Message in the stack will be added and finally returned.
            else
            {
                return " > " + ex.InnerException.Message + GetInnerExceptions(ex.InnerException);
            }
        }
    }
}
