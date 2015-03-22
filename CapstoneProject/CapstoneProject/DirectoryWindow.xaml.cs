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
    /// 
    //Window that displays information on all People (Authors, Cardholders, and Librarians) in the library system
    public partial class DirectoryWindow : Window
    {
        Collections cols = new Collections();

        //START: Directory Window opened
        public DirectoryWindow()
        {
            InitializeComponent();

            //Start with the Librarians radio button selected (so the list of Librarians will be displayed on window launch)
            librariansRadioButton.IsChecked = true;
        }

        //EVENT: Librarians Radio Button selected
        private void librariansRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Create an empty document to be filled with information on Librarians and be displayed in the RichTextBox
            var flowDoc = new FlowDocument();

            //Clear any existing information by selecting all text in the document and replacing it with an empty string
            directoryRichTextBox.SelectAll();
            directoryRichTextBox.Selection.Text = "";
            directoryRichTextBox.AppendText("\n");

            //Go through the People collection finding only Librarians
            foreach (var p in cols.PeopleCollection)
            {
                if (p is Librarians)
                {
                    //For each librarian, create a new paragraph with their information then add it to the document
                    var par = new Paragraph();
                    Librarians lib = p as Librarians;
                    par.Inlines.Add(new Run(lib.ToString()));
                    flowDoc.Blocks.Add(par);
                }
            }

            //Once all the librarians' information has been added, display the document in the RichTextBox
            directoryRichTextBox.Document = flowDoc;
        }

        //EVENT: Cardholders radio button selected
        private void cardholdersRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Create an empty document to be filled with information on Cardholders and be displayed in the RichTextBox
            var flowDoc = new FlowDocument();

            //Clear any existing information by selecting all text in the document and replacing it with an empty string
            directoryRichTextBox.SelectAll();
            directoryRichTextBox.Selection.Text = "";
            directoryRichTextBox.AppendText("\n");

            //Go through the People collection finding only Cardholders
            foreach (var p in cols.PeopleCollection)
            {
                if (p is Cardholders)
                {
                    //For each cardholder, create a new paragraph with their information then add it to the document
                    var par = new Paragraph();
                    Cardholders ch = p as Cardholders;
                    par.Inlines.Add(new Bold(new Run(ch.ToString())));
                    //Include the cardholders' currently checked out books in their information
                    foreach (var co in cols.CheckedOutCollection)
                    {
                        if (co.CardholderID == p.PersonID)
                        {
                            foreach (var b in cols.BookCollection)
                            {
                                if (co.BookID == b.BookID)
                                {
                                    string status;
                                    if (co.IsOverdue)
                                        status = "OVERDUE";
                                    else
                                        status = "On Time";
                                    par.Inlines.Add(new Run("\n\"" + b.Title + "\" (ID " + b.BookID + ")\n    ISBN " + b.ISBN +
                                        "\n    Checked out " + co.CheckOutDate.ToShortDateString() + " (" + status + ")"));
                                }
                            }
                        }
                    }
                    flowDoc.Blocks.Add(par);
                }
            }
            //Once all the cardholders' information has been added, display the document in the RichTextBox
            directoryRichTextBox.Document = flowDoc;
        }

        //EVENT: Authors radio button selected
        private void authorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //Create an empty document to be filled with information on Authors and be displayed in the RichTextBox
            var flowDoc = new FlowDocument();

            //Clear any existing information by selecting all text in the document and replacing it with an empty string
            directoryRichTextBox.SelectAll();
            directoryRichTextBox.Selection.Text = "";
            directoryRichTextBox.AppendText("\n");

            //Go through the People collection finding only Authors
            foreach (var p in cols.PeopleCollection)
            {
                if (p is Authors)
                {
                    //For each author, create a new paragraph with their information then add it to the document
                    var par = new Paragraph();
                    Authors auth = p as Authors;
                    par.Inlines.Add(new Bold(new Run(auth.ToString() + " (ID " + auth.PersonID + ")")));
                    List<Books> booksAuthored = new List<Books>();
                    //Include the authors' written books in their information
                    cols.BookSearchByAuthor(auth.PersonID, out booksAuthored);
                    foreach (var b in booksAuthored)
                    {
                        par.Inlines.Add(new Run("\n\"" + b.Title + "\" (ID " + b.BookID +
                                ")\n    ISBN " + b.ISBN + "\n    Published by " + b.Publisher));
                    }
                    flowDoc.Blocks.Add(par);
                }
            }

            //Once all the authors' information has been added, display the document in the RichTextBox
            directoryRichTextBox.Document = flowDoc;
        }

        //METHOD: Take a string of digits in XXXXXXXXXX format and modify it into the more readable (XXX) XXX-XXXX phone number format
        private string FormatPhoneNumber(string tenDigitNumber)
        {
            //Put number of the incoming numeric string into a char array
            char[] phoneNumber = tenDigitNumber.ToCharArray();

            //Rewrite each number in turn with the appropriate spacing and symbols in between
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
