using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Extends the automatically generated CheckOutLog class to allow for sorting, override the default ToString(), and add an extra property.
    partial class CheckOutLog : IComparable<CheckOutLog>
    {
        //New Status property that does not corellate to anything in the database. Returns a string that indicates whether the book in the
        //current log instance is overdue or not.
        public string Status
        {
            get
            {
                if (DateTime.Now > CheckOutDate.AddDays(30))
                    return "OVERDUE";
                else
                    return "On Time";
            }
        }

        //Empty constructor
        public CheckOutLog()
        {
        }

        //Constructor
        public CheckOutLog(int logID, int cardHolderID, int bookID, DateTime checkOutDate)
        {
            this.CheckOutLogID = logID;
            this.CardholderID = cardHolderID;
            this.BookID = bookID;
            this.CheckOutDate = checkOutDate;
            this.CheckInDate = null;
        }

        //Implementation of IComparable
        public int CompareTo(CheckOutLog other)
        {
            //If the date being compared is null, the date that does exist is "greater"
            if (other == null)
                return 1;

            //When comparing two logs, sort by Check Out Date
            return CheckOutDate.CompareTo(other.CheckOutDate);
        }
    }
}
