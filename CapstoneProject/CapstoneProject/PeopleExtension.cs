using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{

    //Extends the automatically generated People class to make it abstract, allow for sorting, and override the default ToString()
    abstract partial class People : IComparable<People>
    {
        //Empty constructor
        public People()
        {
        }

        //Constructor
        public People(int personID, string firstName, string lastName)
        {
            this.PersonID = personID;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        //ToString() override
        public override string ToString()
        {
            //returns ' lastName, firstName '
            return this.LastName + ", " + this.FirstName;
        }

        //Implementation of IComparable
        public int CompareTo(People other)
        {
            //If the name being compared is null, the name that does exist is "greater"
            if (other == null)
                return 1;

            //When comparing two People, sort by Last Name, then First Name
            if (LastName.CompareTo(other.LastName) == 0)
                return FirstName.CompareTo(other.FirstName);
            else
                return LastName.CompareTo(other.LastName);
        }
    }
}
