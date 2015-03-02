using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    partial class Librarians : People
    {
        public Librarians()
        {
        }

        public Librarians(int personID, string firstName, string lastName, string phoneNumber, string userID, string password)
            : base(personID, firstName, lastName)
        {
            this.Phone = phoneNumber;
            this.UserID = userID;
            this.Password = password;
        }

        public override string ToString()
        {
            char[] phoneNumber = this.Phone.ToCharArray();
            return base.ToString() + " (ID " + base.PersonID + ")\nTel. (" + phoneNumber[0] + phoneNumber[1] + phoneNumber[2] + ") " +
                phoneNumber[3] + phoneNumber[4] + phoneNumber[5] + "-" + phoneNumber[6] + phoneNumber[7] + phoneNumber[8] + phoneNumber[9];
        }
    }
}
