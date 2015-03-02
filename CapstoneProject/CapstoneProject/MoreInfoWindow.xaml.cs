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
    public partial class MoreInfoWindow : Window
    {
        private int incomingBookID;

        public MoreInfoWindow(int bookID)
        {
            InitializeComponent();
            Collections cols = new Collections();
            incomingBookID = bookID;
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
                    foreach (var p in cols.PeopleCollection.PeopleList)
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

            if (Status.IsLibrarian == true)
            {
                editButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            EditWindow ew = new EditWindow(incomingBookID);
            ew.Show();
            this.Close();
        }
    }
}
