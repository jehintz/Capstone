using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying Books
    public class BookCollection : IEnumerable<Books>
    {
        //Private list of books
        private List<Books> _bookList = new List<Books>();

        //Add a Count property, since this is not inherant to IEnumerable
        public int Count
        {
            get { return _bookList.Count; }
        }

        public void Sort()
        {
            _bookList.Sort();
        }

        public void Clear()
        {
            _bookList.Clear();
        }

        ////Public property
        //public List<Books> BookList
        //{
        //    get { return _bookList; }
        //}

        //Allow for addition of new Books to the public lists using indexers
        public Books this[int index]
        {
            get { return _bookList[index]; }
            set { _bookList.Add(value); }
        }

        //Implementation of IEnumerable<Books>
        IEnumerator<Books> IEnumerable<Books>.GetEnumerator()
        {
            return _bookList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _bookList.GetEnumerator();
        }
    }
}
