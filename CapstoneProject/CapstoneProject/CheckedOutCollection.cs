using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying CheckOutLogs
    public class CheckedOutCollection
    {
        //Private list of CheckOutLogs
        private List<CheckOutLog> _checkedOutList = new List<CheckOutLog>();

        //Public property
        public List<CheckOutLog> CheckedOutList
        {
            get { return _checkedOutList; }
        }

        //Allow for addition of new CheckOutLogs to the public list using indexers
        public CheckOutLog this[int index]
        {
            get { return _checkedOutList[index]; }
            set { _checkedOutList.Add(value); }
        }
    }
}
