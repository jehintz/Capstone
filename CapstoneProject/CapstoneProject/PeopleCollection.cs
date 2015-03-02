using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    public class PeopleCollection
    {
        private List<People> peopleList = new List<People>();
        
        public People this[int index]
        {
            get { return peopleList[index]; }
            set { peopleList.Add(value); }
        }

        public List<People> PeopleList
        {
            get { return peopleList; }
        }
    }
}
