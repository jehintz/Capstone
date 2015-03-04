using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Used to fill in the check in window's ListBox with relevant information when a patron's Library Card ID is entered (Title/Status).
    //Also contains data useful for the code used when checking each book in (LogID/BookID).
    class CheckInDisplay : IComparable<CheckInDisplay>
    {
        //Public properties
        public int LogID { get; set; }
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        //Constructor
        public CheckInDisplay(int logID, int bookID, string bookTitle, string overdueStatus)
        {
            LogID = logID;
            BookID = bookID;
            Title = bookTitle;
            Status = overdueStatus;
        }

        //Implementation of IComparable
        public int CompareTo(CheckInDisplay other)
        {
            //If the title being compared is null, the title that does exist is "greater"
            if (other == null)
                return 1;

            //When comparing two CheckInDisplay instances, sort by Title
            return Title.CompareTo(other.Title);
        }
    }
}
