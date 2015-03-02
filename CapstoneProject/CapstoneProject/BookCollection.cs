using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying Books
    public class BookCollection
    {
        //Private list of books
        private List<Books> _bookList = new List<Books>();

        //Public property
        public List<Books> BookList
        {
            get { return _bookList; }
        }

        //Allow for addition of new Books to the public lists using indexers
        public Books this[int index]
        {
            get { return _bookList[index]; }
            set { _bookList.Add(value); }
        }
    }
}
