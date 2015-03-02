using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Extends the automatically generated Books class to allow for sorting, override the default ToString(), and add an extra property
    partial class Books : IComparable<Books>
    {
        //New Availability property that does not corellate to anything in the database. Returns the number of books that are available
        //(ie NOT checked out)
        public int Availability
        {
            get
            {
                Collections cols = new Collections();
                int count = 0;

                //Each time the BookID of the current instance of Books appears in the CheckedOutList, increase the Count by 1
                foreach (var co in cols.CheckedOutCollection.CheckedOutList)
                {
                    if (co.BookID == this.BookID)
                    {
                        count++;
                    }
                }

                //Return the total number of copies minus the number that have been checked out (Count)
                return this.NumberOfCopies - count;
            }
        }

        //Constructor
        public Books (int bookID, string isbn, string title, int authorID, int pages, string subject, string description, string publisher,
            int year, string language, int copies)
        {
            this.BookID = bookID;
            this.ISBN = isbn;
            this.Title = title;
            this.AuthorID = authorID;
            this.NumPages = pages;
            this.Subject = subject;
            this.Description = description;
            this.Publisher = publisher;
            this.YearPublished = year;
            this.Language = language;
            this.NumberOfCopies = copies;
        }

        //Implementation of IComparable
        public int CompareTo(Books other)
        {
            //If the title being compared is null, the title that does exist is "greater"
            if (other == null)
                return 1;

            //When comparing two Books, sort by Title
            return Title.CompareTo(other.Title);
        }
    }
}
