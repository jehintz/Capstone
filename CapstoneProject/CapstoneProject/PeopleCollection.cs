using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying People
    public class PeopleCollection
    {
        //Private list of People
        private List<People> _peopleList = new List<People>();

        //Public property
        public List<People> PeopleList
        {
            get { return _peopleList; }
        }

        //Allow for addition of new People to the public list using indexers
        public People this[int index]
        {
            get { return _peopleList[index]; }
            set { _peopleList.Add(value); }
        }
    }
}
