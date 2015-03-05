using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Contains the People, Books, and CheckedOutLog collections along with methods to search and manipulate those collections
    public class Collections
    {
        //Connection to the database
        private LibraryInformation2Entities db = new LibraryInformation2Entities();

        //Private variables
        private PeopleCollection _peopleCollection = new PeopleCollection();
        private BookCollection _bookCollection = new BookCollection();
        private CheckedOutCollection _checkedOutCollection = new CheckedOutCollection();

        //Public properties
        public PeopleCollection PeopleCollection { get { return _peopleCollection; } }
        public BookCollection BookCollection { get { return _bookCollection; } }
        public CheckedOutCollection CheckedOutCollection { get { return _checkedOutCollection; } }

        //Constructor
        public Collections()
        {
            //Initialize the collections by loading them with the appropriate data from the database
            LoadData();
        }

        //METHOD: Initialize the People, Books, and CheckedOutLog collections
        private void LoadData()
        {
            try
            {
                //Clear all collections before (re)writing to them so there are no duplicates
                _bookCollection.BookList.Clear();
                _peopleCollection.PeopleList.Clear();
                _checkedOutCollection.CheckedOutList.Clear();

                //Query the database to retrieve all People
                var peopleObjects = from p in db.People select p;
                //Add each person to the PeopleCollection as their child type
                foreach (var p in peopleObjects)
                {
                    if (p is Authors)
                        PeopleCollection[0] = p as Authors;

                    if (p is Librarians)
                        PeopleCollection[0] = p as Librarians;

                    if (p is Cardholders)
                        PeopleCollection[0] = p as Cardholders;
                }
                //Now that all people have been added, perform a sort
                PeopleCollection.PeopleList.Sort();

                //Query the database to retrieve all Books
                var bookObjects = from b in db.Books select b;
                //Add each book to the BookCollection
                foreach (var b in bookObjects)
                {
                    _bookCollection[0] = b;
                }
                //Now that all books have been added, perform a sort
                BookCollection.BookList.Sort();

                //Query the database to retrieve all CheckedOutLogs
                var checkedOutObjects = from co in db.CheckOutLog select co;
                //Add each log to the CheckedOutCollection
                foreach (var co in checkedOutObjects)
                {
                    _checkedOutCollection[0] = co;
                }
                //Now that all the logs have been added, perform a sort
                CheckedOutCollection.CheckedOutList.Sort();
            }
            catch (Exception ex)
            {
                //Write any exceptions to the ErrorLog.txt file
                var exHandler = new ExceptionHandler(ex);
            }
        }

        //METHOD: Searches for a match in each book's Title, Author, Subject, and ISBN, and adds all matches to a BookSearchDisplay List
        internal void BookSearchAll (string query, out List<BookSearchDisplay> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<BookSearchDisplay>();

            //Go through each book and search for a match between the given Query and the book's Title, Subject, or ISBN
            foreach (Books b in BookCollection.BookList)
            {
                if ((b.Title.ToUpper().Contains(query.ToUpper()) || b.Subject.ToUpper().Contains(query.ToUpper()) || b.ISBN.ToString().Contains(query))
                    && b.NumberOfCopies > 0)
                {
                    //If a match is found, get that book's author
                    foreach (var a in PeopleCollection.PeopleList)
                    {
                        if (b.AuthorID == a.PersonID)
                        {
                            //Add the matching result to the list
                            var result = new BookSearchDisplay(b, a);
                            resultsList.Add(result);
                        }
                    }
                }
            }
            //Go through each author and search for a match between the given Query and the author's First or Last Name
            foreach (People p in PeopleCollection.PeopleList)
            {
                if (p is Authors)
                {
                    if (p.FirstName.ToUpper().Contains(query.ToUpper()) || p.LastName.ToUpper().Contains(query.ToUpper()))
                    {
                        foreach (var b in BookCollection.BookList)
                        {
                            //If a match is found, get each book that the author has written
                            if (b.AuthorID == p.PersonID && b.NumberOfCopies > 0)
                            {
                                //Checks if the match already exists in the Results List (from the previously performed Title/Subject/ISBN search
                                bool duplicate = false;
                                foreach (var d in resultsList)
                                {
                                    if (b.BookID == d.ID)
                                        duplicate = true;
                                }
                                if (!duplicate)
                                {
                                    //If the match does NOT already exist (therefore ensuring it is not a duplicate), add the
                                    //result to the list
                                    var result = new BookSearchDisplay(b, p);
                                    resultsList.Add(result);
                                }
                            }
                        }
                    }
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a match in each books Title and adds all matches to a BookSearchDisplay List
        internal void BookSearchByTitle (string query, out List<BookSearchDisplay> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<BookSearchDisplay>();

            //Find any books that contain the incoming query in the Title
            foreach (Books b in BookCollection.BookList)
            {
                if (b.Title.ToUpper().Contains(query.ToUpper()) && b.NumberOfCopies > 0)
                {
                    //If a book is found, get the book's author
                    foreach (var a in PeopleCollection.PeopleList)
                    {
                        if (b.AuthorID == a.PersonID)
                        {
                            //Add the result to the BookSearchDisplay list
                            var result = new BookSearchDisplay(b, a);
                            resultsList.Add(result);
                        }
                    }
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a match in each books Author and adds all matches to a BookSearchDisplay List
        internal void BookSearchByAuthor (string query, out List<BookSearchDisplay> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<BookSearchDisplay>();

            //Find any authors that contain the incoming query in their first or last name
            foreach (People p in PeopleCollection.PeopleList)
            {
                if (p is Authors)
                {
                    if (p.FirstName.ToUpper().Contains(query.ToUpper()) || p.LastName.ToUpper().Contains(query.ToUpper()))
                    {
                        //If an author was found, add each of their books to the results list
                        foreach (var b in _bookCollection.BookList)
                        {
                            if (b.AuthorID == p.PersonID && b.NumberOfCopies > 0)
                            {
                                var result = new BookSearchDisplay(b, p);
                                resultsList.Add(result);
                            }
                        }
                    }
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a match in each books Author ID and adds all matches to a list of Books
        internal void BookSearchByAuthor (int personID, out List<Books> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<Books>();

            //Add each book to the results list where the Author ID is the same as the incoming Person ID
            foreach (var b in BookCollection.BookList)
            {
                if (b.AuthorID == personID)
                {
                    resultsList.Add(b);
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a match in each books ISBN and adds all matches to a BookSearchDisplay List
        internal void BookSearchByISBN (string query, out List<BookSearchDisplay> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<BookSearchDisplay>();

            //Find any books that contain the incoming query in their ISBN
            foreach (Books b in BookCollection.BookList)
            {
                if (b.ISBN.ToString().Contains(query) && b.NumberOfCopies > 0)
                {
                    //If a book match was found, get that book's author
                    foreach (var a in _peopleCollection.PeopleList)
                    {
                        if (b.AuthorID == a.PersonID)
                        {
                            //Create a new BookSearchDisplay with the book and author info and add it to the results list
                            var result = new BookSearchDisplay(b, a);
                            resultsList.Add(result);
                        }
                    }
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a match in each books Subject and adds all matches to a BookSearchDisplay List
        internal void BookSearchSubject (string query, out List<BookSearchDisplay> resultsList)
        {
            resultsList = new List<BookSearchDisplay>();

            foreach (Books b in BookCollection.BookList)
            {
                if (b.Subject.ToUpper().Contains(query.ToUpper()) && b.NumberOfCopies > 0)
                {
                    foreach (var a in PeopleCollection.PeopleList)
                    {
                        if (b.AuthorID == a.PersonID)
                        {
                            var result = new BookSearchDisplay(b, a);
                            resultsList.Add(result);
                        }
                    }
                }
            }

            //Perform a sort on the results list
            resultsList.Sort();
        }

        //METHOD: Searches for a matching book from a given Book ID
        internal Books BookSearchByID (int bookID)
        {
            //Find the book that has the incoming Book ID
            foreach (Books b in BookCollection.BookList)
            {
                if (b.BookID == bookID)
                {
                    return b;
                }
            }
            
            //If no book with that Book ID was found, return null
            return null;
        }

        //METHOD: Get a list of all books a cardholder has checked out using their Library Card ID, and add the results to a list of CheckInDisplays
        internal bool LogSearchByLibraryCardID (string cardID, out List<CheckInDisplay> resultsList)
        {
            //Ensure the given resultsList is clear of any items
            resultsList = new List<CheckInDisplay>();

            bool idFound = false;

            //Find the cardholder with a matching Library Card ID
            foreach (var p in PeopleCollection.PeopleList)
            {
                if (p is Cardholders)
                {
                    Cardholders temp = p as Cardholders;
                    if (cardID.Trim().ToUpper() == temp.LibraryCardID.ToUpper())
                    {
                        idFound = true;
                        //If a matching cardholder was found, find what they have checked out in the CheckOutLog collection
                        foreach (var co in CheckedOutCollection.CheckedOutList)
                        {
                            if (co.CardholderID == temp.PersonID)
                            {
                                //If the cardholder has a book checked out, get that book's information
                                foreach (var b in BookCollection.BookList)
                                {
                                    if (b.BookID == co.BookID)
                                    {
                                        //Create a new CheckInDisplay with the appropriate information, and add it to the results list
                                        resultsList.Add(new CheckInDisplay(co.CheckOutLogID, b.BookID, b.Title, co.Status));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //If the given Library Card ID was found, return true
                    if (idFound)
                        return true;
                }
            }
            //If the given Library Card ID was not found, return false
            return false;
        }
    }
}
