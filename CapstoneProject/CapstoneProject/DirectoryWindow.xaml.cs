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
    /// Interaction logic for DirectoryWindow.xaml
    /// </summary>
    public partial class DirectoryWindow : Window
    {
        Collections cols = new Collections();

        public DirectoryWindow()
        {
            InitializeComponent();

            //Start with the Librarians radio button selected (so the list of Librarians will be displayed on window launch)
            librariansRadioButton.IsChecked = true;
        }

        //EVENT: Librarians Radio Button selected
        private void librariansRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var flowDoc = new FlowDocument();

                //Clear any existing information by selecting all text in the document and replacing it with an empty string
                directoryRichTextBox.SelectAll();
                directoryRichTextBox.Selection.Text = "";
                directoryRichTextBox.AppendText("\n");

                foreach (var p in cols.PeopleCollection.PeopleList)
                {
                    if (p is Librarians)
                    {
                        var par = new Paragraph();
                        Librarians lib = p as Librarians;
                        par.Inlines.Add(new Run(lib.ToString()));
                        flowDoc.Blocks.Add(par);
                    }
                }
                directoryRichTextBox.Document = flowDoc;
            }
            catch (Exception ex)
            {
                var handler = new ExceptionHandler(ex);
            }
        }

        //EVENT: Cardholders radio button selected
        private void cardholdersRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var flowDoc = new FlowDocument();

            //Clear any existing information by selecting all text in the document and replacing it with an empty string
            directoryRichTextBox.SelectAll();
            directoryRichTextBox.Selection.Text = "";
            directoryRichTextBox.AppendText("\n");

            foreach (var p in cols.PeopleCollection.PeopleList)
            {
                if (p is Cardholders)
                {
                    var par = new Paragraph();
                    Cardholders ch = p as Cardholders;
                    par.Inlines.Add(new Bold(new Run(ch.ToString())));
                    foreach (var co in cols.CheckedOutCollection.CheckedOutList)
                    {
                        if (co.CardholderID == p.PersonID)
                        {
                            foreach (var b in cols.BookCollection.BookList)
                            {
                                if (co.BookID == b.BookID)
                                {
                                    par.Inlines.Add(new Run("\n\"" + b.Title + "\" (ID " + b.BookID + ")\n    ISBN " + b.ISBN +
                                        "\n    Checked out " + co.CheckOutDate.ToShortDateString() + " (" + co.Status + ")"));
                                }
                            }
                        }
                    }
                    flowDoc.Blocks.Add(par);
                    //TODO
                }
            }
            directoryRichTextBox.Document = flowDoc;
        }

        //EVENT: Authors radio button selected
        private void authorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var flowDoc = new FlowDocument();

            //Clear any existing information by selecting all text in the document and replacing it with an empty string
            directoryRichTextBox.SelectAll();
            directoryRichTextBox.Selection.Text = "";
            directoryRichTextBox.AppendText("\n");

            foreach (var p in cols.PeopleCollection.PeopleList)
            {
                if (p is Authors)
                {
                    var par = new Paragraph();
                    Authors auth = p as Authors;
                    par.Inlines.Add(new Bold(new Run(auth.ToString() + " (ID " + auth.PersonID + ")")));
                    List<Books> booksAuthored = new List<Books>();
                    cols.BookSearchByAuthor(auth.PersonID, out booksAuthored);
                    foreach (var b in booksAuthored)
                    {
                        par.Inlines.Add(new Run("\n\"" + b.Title + "\" (ID " + b.BookID +
                                ")\n    ISBN " + b.ISBN + "\n    Published by " + b.Publisher));
                    }
                    flowDoc.Blocks.Add(par);
                }
            }
            directoryRichTextBox.Document = flowDoc;
        }

        //METHOD: Take a string of digits in XXXXXXXXXX format and modify it into the more readable (XXX) XXX-XXXX phone number format
        private string FormatPhoneNumber(string tenDigitNumber)
        {
            char[] phoneNumber = tenDigitNumber.ToCharArray();

            if (phoneNumber.Length == 10)
            {
                return "(" + phoneNumber[0] + phoneNumber[1] + phoneNumber[2] + ") " + phoneNumber[3] + phoneNumber[4] +
                    phoneNumber[5] + "-" + phoneNumber[6] + phoneNumber[7] + phoneNumber[8] + phoneNumber[9];
            }

            //Given string not in the correct format
            return null;
        }
    }
}
