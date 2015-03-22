using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
    /// Interaction logic for CheckInWindow.xaml
    /// </summary>
    /// 
    //Window for checking books in
    public partial class CheckInWindow : Window
    {
        Collections cols = new Collections();
        LibraryInformation2Entities db = new LibraryInformation2Entities();

        //START: Initializing the Check In Window
        public CheckInWindow()
        {
            InitializeComponent();

            //Place the text cursor in the Card ID entry text box
            cardIDEntryBox.Focus();
        }

        //EVENT: Card ID Search button clicked
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear any error messages
            idErrorLabel.Visibility = System.Windows.Visibility.Hidden;

            //If no Library Card ID was entered, display an error message
            if (string.IsNullOrWhiteSpace(cardIDEntryBox.Text))
            {
                idErrorLabel.Content = "Please enter an ID.";
                idErrorLabel.Visibility = System.Windows.Visibility.Visible;
                cardIDEntryBox.Focus();
                return;
            }

            //Search for the entered Card ID among all Cardholders, then if it is found, get the books they currently have checked out
            List<CheckInDisplay> booksToCheckIn = new List<CheckInDisplay>();
            if (cols.LogSearchByLibraryCardID(cardIDEntryBox.Text, out booksToCheckIn))
            {
                //If the Card ID was found but the list of books that person has checked out is empty, display an error message
                if(booksToCheckIn.Count == 0)
                {
                    idErrorLabel.Content = "No books checked out.";
                    idErrorLabel.Visibility = System.Windows.Visibility.Visible;
                    cardIDEntryBox.Focus();
                    return;
                }

                //Display the patron's checked out books
                checkedOutListbox.ItemsSource = booksToCheckIn;

                //Enable the Check In controls and disable the Card ID search controls
                checkedOutListbox.ItemsSource = booksToCheckIn;
                checkInButton.IsEnabled = true;
                cancelButton.IsEnabled = true;
                cardIDEntryBox.IsEnabled = false;
                searchButton.Visibility = System.Windows.Visibility.Hidden;
                idEntryLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                //If no Cardholder with a matching Card ID was found, display an error message
                idErrorLabel.Content = "ID not found.";
                idErrorLabel.Visibility = System.Windows.Visibility.Visible;
                cardIDEntryBox.Focus();
            }
        }

        //EVENT: Cancel button clicked
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Confirm that the librarian wishes to exit the window WITHOUT checking books in
            MessageBoxResult result = MessageBox.Show("Cancelling check in. No changes will be made. Proceed?", "Close Window", MessageBoxButton.YesNo);

            //If the librarian does wish to exit without performing a check in and selects Yes, close the window
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        //EVENT: Check In button clicked
        private void checkInButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear any error messages
            checkInErrorLabel.Visibility = System.Windows.Visibility.Hidden;
            
            //If no books were selected to check in, display an error message
            if (checkedOutListbox.SelectedItems.Count == 0)
            {
                checkInErrorLabel.Content = "Select at least one book to check in.";
                checkInErrorLabel.Visibility = System.Windows.Visibility.Visible;
                return;
            }

            //Confirm the librarian wants to check in the selected books
            MessageBoxResult result = MessageBox.Show("Checking in " + checkedOutListbox.SelectedItems.Count + " book(s). Proceed?", "Complete Check In", MessageBoxButton.YesNo);

            //If Yes was selected, finalize check in by removing the appropriate Check Out Logs
            if (result == MessageBoxResult.Yes)
            {
                //Since multiple books can be checked in at once, loop through them and check in each individually
                foreach (var item in checkedOutListbox.SelectedItems)
                {
                    CheckInDisplay selectedItem = (CheckInDisplay)item;

                    //Remove Check Out Log entry from the collections
                    foreach (var log in cols.CheckedOutCollection)
                    {
                        if (log.CheckOutLogID == selectedItem.LogID)
                        {
                            cols.CheckedOutCollection.Remove(log);
                            break;
                        }
                    }

                    //Remove Check Out Log Entry from the database
                    var bookToCheckIn = from log in db.CheckOutLog where log.CheckOutLogID == selectedItem.LogID select log;
                    db.CheckOutLog.RemoveRange(bookToCheckIn);
                    db.SaveChanges();
                    this.Close();
                }
            }
        }

    }
}
