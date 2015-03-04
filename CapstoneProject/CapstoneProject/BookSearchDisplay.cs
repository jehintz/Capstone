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
        //Public properties
        public string Title { get; private set; }
        public int ID { get; private set; }
        public string Author { get; private set; }
        public string Publisher { get; private set; }
        public int Year { get; private set; }
        public string Availability { get; private set; }

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
