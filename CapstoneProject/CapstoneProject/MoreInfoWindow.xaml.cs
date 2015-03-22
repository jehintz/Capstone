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
    /// Interaction logic for MoreInfoWindow.xaml
    /// </summary>
    /// 
    //Window that gives detailed information on a given book
    public partial class MoreInfoWindow : Window
    {
        private int incomingBookID;

        //START: More Info window opened
        public MoreInfoWindow(int bookID)
        {
            InitializeComponent();


            Collections cols = new Collections();
            incomingBookID = bookID;  //Will need this if a librarian wants to edit a book's info
            
            //Get the information associated with the incoming book and display it
            foreach (var b in cols.BookCollection.BookList)
            {
                if (b.BookID == bookID)
                {
                    titleLabel.Content = b.Title;
                    pagesLabel.Content = b.NumPages;
                    subjectLabel.Content = b.Subject;
                    publisherLabel.Content = b.Publisher;
                    yearLabel.Content = b.YearPublished;
                    languagesLabel.Content = b.Language;
                    isbnLabel.Content = b.ISBN;
                    descriptionTextBlock.Text = b.Description;
                    foreach (var p in cols.PeopleCollection)
                    {
                        if (p.PersonID == b.AuthorID)
                        {
                            authorLabel.Content = p;
                        }
                    }
                    availabilityLabel.Content = b.Availability + " out of " + b.NumberOfCopies;
                    break;
                }
            }

            //If the user is logged in as a librarian, display the Edit Book Info button
            if (Status.IsLibrarian == true)
            {
                editButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //EVENT: Edit button clicked
        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            //Open a new Edit Window with the ID of the book presently being displayed as the argument
            EditWindow ew = new EditWindow(incomingBookID);
            ew.Show();

            //Close this window
            this.Close();
        }
    }
}
