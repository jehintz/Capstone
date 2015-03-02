using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CapstoneProject
{
    //Used to fill in the main window's DataGrid when performing a book search. Only contains the appropriate display properties (which
    //are automatically turned into columns on the DataGrid)
    class BookSearchDisplay : IComparable<BookSearchDisplay>
    {
        //Private variables
        private string _title;
        private int _bookID;
        private string _author;
        private string _publisher;
        private int _year;
        private string _availability;       

        //Public properties
        public string Title { get { return _title; } set { _title = value; } }
        public int ID { get { return _bookID; } set { _bookID = value; } }
        public string Author { get { return _author; } set { _author = value; } }
        public string Publisher { get { return _publisher; } set { _publisher = value; } }
        public int Year { get { return _year; } set { _year = value; } }
        public string Availability { get { return _availability; } set { _availability = value; } }

        //Constructor that takes a Book and a Person
        public BookSearchDisplay(Books b, People a)
        {
            //Set the incoming info to the appropriate properties
            Title = b.Title;
            ID = b.BookID;
            Author = a.ToString();
            Publisher = b.Publisher;
            Year = b.YearPublished;

            //Set the Availability string based on how many books are available
            if (b.Availability == 0)
            {
                //No copies are available
                Availability = "Checked Out";
            }
            else if(b.Availability == 1)
            {
                //Only one copy is available
                Availability = "1 Copy Available";
            }
            else
            {
                //More than one copy is available
                Availability = string.Format("{0} Copies Available", b.Availability);
            }
        }

        //Implementation of IComparable
        public int CompareTo(BookSearchDisplay other)
        {
            //If the title being compared is null, the title that does exist is "greater"
            if (other == null)
                return 1;

            //When comparing two BookSearchDisplay instances, sort by Title
            return Title.CompareTo(other.Title);
        }
    }
}
