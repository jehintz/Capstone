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
    public partial class OverdueWindow : Window
    {
        public OverdueWindow()
        {
            InitializeComponent();

            var cols = new Collections();

            foreach (var co in cols.CheckedOutCollection.CheckedOutList)
            {
                if (co.Status == "OVERDUE")
                {
                    foreach (var b in cols.BookCollection.BookList)
                    {
                        if (b.BookID == co.BookID)
                        {
                            foreach (var p in cols.PeopleCollection.PeopleList)
                            {
                                if (p.PersonID == co.CardholderID)
                                {
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
