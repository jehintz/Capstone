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
    /// Interaction logic for CheckOutWindow.xaml
    /// </summary>
    /// 
    //Window for checking books out
    public partial class CheckOutWindow : Window
    {
        LibraryInformation2Entities db = new LibraryInformation2Entities();
        Collections cols = new Collections();
        private List<CheckOutLog> tempCheckOutLog = new List<CheckOutLog>();
        private int cardHolderID = 0;
        private int logID = 0;

        //START: Initialize the Check In Window
        public CheckOutWindow()
        {
            InitializeComponent();

            //Place the text cursor in the Card ID entry text box
            cardIDEntryBox.Focus();
        }

        //EVENT: Cancel button clicked
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Confirm that the librarian wishes to exit the window WITHOUT checking books out
            MessageBoxResult result = MessageBox.Show("No books will be checked out. Proceed?", "Close Window", MessageBoxButton.YesNo);

            //If the librarian does wish to exit without performing a check in and selects Yes, close the window
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        //EVENT: ISBN Add button was clicked.
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //Remove any error message if one is already being displayed
            errorLabel.Visibility = System.Windows.Visibility.Hidden;
            bool isbnFound = false;

            //If the ISBN Entry Box is empty, display an error message
            if (string.IsNullOrWhiteSpace(isbnEntryBox.Text))
            {
                errorLabel.Content = "Please enter an ISBN.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                isbnEntryBox.Focus();
                return;
            }

            //If the entered ISBN contains anything other than numbers, display an error message
            foreach (var c in isbnEntryBox.Text)
            {
                if (!char.IsDigit(c))
                {
                    errorLabel.Content = "The ISBN can only contain numbers. Do not include dashes.";
                    errorLabel.Visibility = System.Windows.Visibility.Visible;
                    isbnEntryBox.Focus();
                    return;
                }
            }

            //Search the books for a matching ISBN
            foreach(var b in cols.BookCollection)
            {
                //Only counts as a match if the number of copies is greater than 0 (otherwise the book is 'retained info only' and no longer
                //exists in the library catalog to be checked out)
                if (isbnEntryBox.Text.Trim() == b.ISBN && b.NumberOfCopies > 0)
                {
                    isbnFound = true;

                    //Check that there is at least one copy available for check out
                    if (b.Availability < 1)
                    {
                        errorLabel.Content = "All copies of this book are already checked out.";
                        errorLabel.Visibility = System.Windows.Visibility.Visible;
                        isbnEntryBox.Focus();
                        return;
                    }
                    else
                    {
                        //Ensure the cardholder does not already have another copy of the same book checked out
                        foreach (var co in cols.CheckedOutCollection.CheckedOutList)
                        {
                            if (co.CardholderID == cardHolderID && co.BookID == b.BookID)
                            {
                                errorLabel.Content = "This patron already has a copy of \"" + b.Title + "\" checked out.";
                                errorLabel.Visibility = System.Windows.Visibility.Visible;
                                isbnEntryBox.Focus();
                                return;
                            }
                        }
                        //Prevent the librarian from adding the same book twice in the same check out session (again so the cardholder cannot
                        //have two copies of the same book)
                        foreach (var co in tempCheckOutLog)
                        {
                            if (co.CardholderID == cardHolderID && co.BookID == b.BookID)
                            {
                                errorLabel.Content = "Cannot check out two copies of the same book.";
                                errorLabel.Visibility = System.Windows.Visibility.Visible;
                                isbnEntryBox.Focus();
                                return;
                            }
                        }

                        //If all checks are passed, display the book's title in the listbox
                        checkOutList.Items.Add(b.Title);

                        //Get a unique Log ID for the new CheckOutLog by finding the smallest available int among existing Log IDs
                        for (int i = logID + 1; i <= 1000; i++)
                        {
                            bool alreadyExists = false;
                            foreach (var co in cols.CheckedOutCollection.CheckedOutList)
                            {
                                if (i == co.CheckOutLogID)
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }
                            if (!alreadyExists)
                            {
                                logID = i;
                                break;
                            }
                        }

                        //Create a CheckOutLog with the appropriate information and add it to the temporary list
                        CheckOutLog checkOut = new CheckOutLog(logID, cardHolderID, b.BookID, DateTime.Now);
                        tempCheckOutLog.Add(checkOut);

                        //Empty the ISBN Entry textbox of all text and place the text cursor there in case the librarian wishes to add
                        //another book
                        isbnEntryBox.Clear();
                        isbnEntryBox.Focus();
                    }
                    break;
                }
            }

            //If no matching ISBN was found, display an error message
            if (!isbnFound)
            {
                errorLabel.Content = "ISBN not found.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                isbnEntryBox.Focus();
            }
        }

        //EVENT: Library card ID Search button was clicked
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            bool idFound = false;

            //Clear any error messages
            errorLabel.Visibility = System.Windows.Visibility.Hidden;

            //If no Card ID was entered, display an error message
            if (string.IsNullOrWhiteSpace(cardIDEntryBox.Text))
            {
                errorLabel.Content = "Please enter a card ID.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                cardIDEntryBox.Focus();
                return;
            }

            //Check for a matching Library Card ID among the cardholders
            foreach (var p in cols.PeopleCollection)
            {
                if (p is Cardholders)
                {
                    Cardholders temp = p as Cardholders;
                    if (temp.LibraryCardID.ToUpper() == cardIDEntryBox.Text.Trim().ToUpper())
                    {
                        idFound = true;

                        //If the found cardholder currently has overdue books, display an error message
                        if (CheckForOverdueBooks(temp.PersonID))
                        {
                            errorLabel.Content = "This patron has overdue books and cannot check out more at this time.";
                            errorLabel.Visibility = System.Windows.Visibility.Visible;
                            cardIDEntryBox.Focus();
                        }
                        //If the cardholder was found and they have no overdue books, disable the button that allows librarians to search
                        //for cardholders, and instead enable the button that allows librarians to add books to check out
                        else
                        {
                            cardHolderID = temp.PersonID;
                            isbnInstructionLabel.Visibility = System.Windows.Visibility.Visible;
                            isbnEntryBox.Visibility = System.Windows.Visibility.Visible;
                            addButton.Visibility = System.Windows.Visibility.Visible;
                            checkOutButton.IsEnabled = true;
                            cancelButton.IsEnabled = true;
                            idInstructionLabel.Visibility = System.Windows.Visibility.Hidden;
                            cardIDEntryBox.IsEnabled = false;
                            searchButton.Visibility = System.Windows.Visibility.Hidden;
                            isbnEntryBox.Focus();
                        }
                        break;
                    }
                }
            }

            //If no cardholder with a matching Library Card ID was found, display an error message
            if (!idFound)
            {
                errorLabel.Content = "ID not found.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                cardIDEntryBox.Focus();
            }
        }

        //EVENT: Check out button clicked
        private void checkOutButton_Click(object sender, RoutedEventArgs e)
        {
            //Display a dialog box so the librarian can confirm they want to check out the entered books
            MessageBoxResult result = MessageBox.Show("Checking out " + tempCheckOutLog.Count + " book(s). Proceed?", "Finalize Check Out", MessageBoxButton.YesNo);

            //If the librarian selected 'Yes' and does want to finalize check out, add all books in the temporary check out log to both the
            //collections and database
            if (result == MessageBoxResult.Yes)
            {
                foreach (var col in tempCheckOutLog)
                {
                    cols.CheckedOutCollection.CheckedOutList[0] = col;
                    db.CheckOutLog.Add(col);
                    db.SaveChanges();
                }

                //Now that the checkouts have been finalized, close the window
                this.Close();
            }
        }

        //METHOD: Check if a patron currently has any overdue books
        private bool CheckForOverdueBooks(int personID)
        {
            foreach (var co in cols.CheckedOutCollection.CheckedOutList)
            {
                if (co.CardholderID == personID)
                {
                    if (co.Status == "OVERDUE")
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
