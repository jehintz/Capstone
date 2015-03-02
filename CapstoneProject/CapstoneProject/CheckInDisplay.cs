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
        //Private variables
        private int _logID;
        private int _bookID;
        private string _title;
        private string _status;

        //Public properties
        public int LogID { get { return _logID; } set { _logID = value; } }
        public int BookID { get { return _bookID; } set { _bookID = value; } }
        public string Title { get { return _title; } set { _title = value; } }
        public string Status { get { return _status; } set { _status = value; } }

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
