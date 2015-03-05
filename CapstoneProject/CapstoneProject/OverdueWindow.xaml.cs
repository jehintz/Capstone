using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CapstoneProject
{
    /// <summary>
    /// Interaction logic for OverdueWindow.xaml
    /// </summary>
    /// 
    //Window which displays a list of all currently overdue books
    public partial class OverdueWindow : Window
    {
        //START: Overdue Window opened
        public OverdueWindow()
        {
            InitializeComponent();

            var cols = new Collections();

            //Loop through each CheckOutLog in the CheckedOutList
            foreach (var co in cols.CheckedOutCollection.CheckedOutList)
            {
                //Check if the Log is overdue
                if (co.Status == "OVERDUE")
                {
                    //Get the book info for that Log if it IS overdue
                    foreach (var b in cols.BookCollection.BookList)
                    {
                        if (b.BookID == co.BookID)
                        {
                            //Finally, get the Cardholder information for that log
                            foreach (var p in cols.PeopleCollection.PeopleList)
                            {
                                if (p.PersonID == co.CardholderID)
                                {
                                    //Add the overdue Log and all relevant information to the ListView
                                    var coExpander = new Expander();
                                    coExpander.Header = co.CheckOutDate.ToShortDateString() + " - \"" + b.Title + "\" (ID " + co.BookID + ")";
                                    coExpander.Content = "ISBN " + b.ISBN + "\nChecked out by " + p;
                                    coExpander.IsExpanded = false;
                                    listView.Items.Add(coExpander);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
