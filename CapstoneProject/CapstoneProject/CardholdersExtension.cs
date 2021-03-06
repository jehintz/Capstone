﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Extends the automatically generated Cardholders class to inherit from People and override the default ToString()
    partial class Cardholders : People
    {
        //Constructor that inherits from base class People
        public Cardholders(int personID, string firstName, string lastName, string phoneNumber, string libraryCardID)
            : base(personID, firstName, lastName)
        {
            this.Phone = phoneNumber;
            this.LibraryCardID = libraryCardID;
        }

        //ToString override
        public override string ToString()
        {
            //Seperate each char of the phone number string into an array to make it easier to print the phone number in (123) 456-7890 format below
            //(the original string is formatted like: 1234567890)
            char[] phoneNumber = this.Phone.ToCharArray();

            //Returns ' lastName, firstName (ID X, libraryCardID)
            //          Tel. (XXX) XXX-XXXX '
            return base.ToString() + " (ID " + base.PersonID + ", " + this.LibraryCardID + ")\nTel. (" + phoneNumber[0] + phoneNumber[1] + phoneNumber[2] + ") " +
                phoneNumber[3] + phoneNumber[4] + phoneNumber[5] + "-" + phoneNumber[6] + phoneNumber[7] + phoneNumber[8] + phoneNumber[9];
        }
    }
}
