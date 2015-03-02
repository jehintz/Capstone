using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Extends the automatically generated Authors class to inherit from People and override the default ToString()
    partial class Authors : People
    {
        //Constructor that inherits from base class People
        public Authors(int personID, string firstName, string lastName, string bio) : base(personID, firstName, lastName)
        {
            this.Bio = bio;
        }

        //String override that returns ' firstName, lastName '
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
