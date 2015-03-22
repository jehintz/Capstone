using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneProject
{
    //Provides an extra layer between the program and the database viewing/modifying CheckOutLogs
    public class CheckedOutCollection : IEnumerable<CheckOutLog>
    {
        //Private list of CheckOutLogs
        private List<CheckOutLog> _checkedOutList = new List<CheckOutLog>();

        //Allow for addition of new CheckOutLogs to the public list using indexers
        public CheckOutLog this[int index]
        {
            get { return _checkedOutList[index]; }
            set { _checkedOutList.Add(value); }
        }

        //Add functionality not inherant to IEnumerable
        public int Count
        {
            get { return _checkedOutList.Count; }
        }

        public void Clear()
        {
            _checkedOutList.Clear();
        }

        public void Sort()
        {
            _checkedOutList.Sort();
        }

        public void Remove(CheckOutLog log)
        {
            _checkedOutList.Remove(log);
        }

        //Implementation of IEnumerable<CheckOutLog>
        IEnumerator<CheckOutLog> IEnumerable<CheckOutLog>.GetEnumerator()
        {
            return _checkedOutList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _checkedOutList.GetEnumerator();
        }
    }
}
