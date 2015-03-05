using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Static class to be used throughout the application that determines WHETHER a librarian is signed in, and WHICH librarian
    static class Status
    {
        public static bool IsLibrarian { get; set; }
        public static string LoggedInLibrarianID { get; set; }
    }
}
