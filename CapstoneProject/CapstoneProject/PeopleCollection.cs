using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying People
    public class PeopleCollection : IEnumerable<People>
    {
        //Private list of People
        private List<People> _peopleList = new List<People>();

        //Add functionality not inherant to Inumerable
        public int Count
        {
            get { return _peopleList.Count; }
        }

        public void Clear()
        {
            _peopleList.Clear();
        }

        public void Sort()
        {
            _peopleList.Sort();
        }

        //Allow for addition of new People to the public list using indexers
        public People this[int index]
        {
            get { return _peopleList[index]; }
            set { _peopleList.Add(value); }
        }

        //Implementation of IEnumerable<People>
        IEnumerator<People> IEnumerable<People>.GetEnumerator()
        {
            return _peopleList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _peopleList.GetEnumerator();
        }
    }
}
