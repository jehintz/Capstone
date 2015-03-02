using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    abstract partial class People : IComparable<People>
    {
        //Empty constructor
        public People()
        {
        }

        // : this() ...? doesn't seem to be necessary
        //Constructor
        public People(int personID, string firstName, string lastName)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public override string ToString()
        {
            return this.LastName + ", " + this.FirstName;
        }

        public int CompareTo(People other)
        {
            if (other == null)
                return 1;

            if (LastName.CompareTo(other.LastName) == 0)
                return FirstName.CompareTo(other.FirstName);
            else
                return LastName.CompareTo(other.LastName);
        }
    }
}
