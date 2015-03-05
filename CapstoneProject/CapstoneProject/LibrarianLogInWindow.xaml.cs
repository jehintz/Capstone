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
    /// Interaction logic for LibrarianLogInWindow.xaml
    /// </summary>
    /// 
    //Window where librarians can enter a user id and password to login and access librarian-specific functionality
    public partial class LibrarianLogInWindow : Window
    {
        Collections cols = new Collections();

        //Start: Librarian Log In window opened
        public LibrarianLogInWindow()
        {
            InitializeComponent();

            //Place a text cursor in the User ID entry textbox
            userIDTextBox.Focus();
        }

        //EVENT: Cancel button clicked
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //Ensure that if the librarian is choosing to cancel login, the main window will stay in Patron Mode
            Status.IsLibrarian = false;

            //Close the window
            this.Close();
        }

        //EVENT: Log In button clicked
        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoggedIn = false;
            bool validUserID = false;

            //If no User ID was entered, display an error message
            if(string.IsNullOrWhiteSpace(userIDTextBox.Text))
            {
                errorLabel.Content = "Enter a User ID.";
                userIDTextBox.Focus();
                return;
            }

            //If no password was entered, display an error message
            if(string.IsNullOrWhiteSpace(passwordTextBox.Password))
            {
                errorLabel.Content = "Enter a password.";
                passwordTextBox.Focus();
                return;
            }

            //Search for a librarian with a User ID matching the one entered
            foreach(var p in cols.PeopleCollection.PeopleList)
            {
                if (p is Librarians)
                {
                    //If a matching User ID is found, compare the password assocatied with that ID to the password entered in this window's passwordbox
                    Librarians librarian = p as Librarians;
                    if (userIDTextBox.Text.ToUpper() == librarian.UserID.ToUpper() && passwordTextBox.Password == librarian.Password)
                    {
                        //If both the User ID and Password match, mark the librarian as logged in
                        isLoggedIn = true;
                        break;
                    }
                    if (userIDTextBox.Text.ToUpper() == librarian.UserID.ToUpper())
                    {
                        //If only the User ID matches but the entered password is incorrect, indicate that at least the User ID was found
                        validUserID = true;
                    }
                }
            }

            //If the entered User ID and Password were matched and the librarian logged in, set the Status properties to indicate that
            if (isLoggedIn)
            {
                Status.IsLibrarian = true;  //This activates librarian-only functionality
                Status.LoggedInLibrarianID = userIDTextBox.Text;  //This indicates WHICH librarian is logged in

                //Now that the librarian has been successfully logged in, close the window
                this.Close();
            }
            //If there was some problem with logging the librarian in, display an appropriate error message
            else
            {
                errorLabel.Visibility = System.Windows.Visibility.Visible;

                //Display this message if the User ID was found but the password did not match
                if (validUserID)
                {
                    errorLabel.Content = "Incorrect password.";
                    passwordTextBox.Focus();
                }
                //Otherwise the User ID must have been incorrect or not found, so display this message
                else
                {
                    errorLabel.Content = "User ID not found.";
                    userIDTextBox.Focus();
                }
            }
        }
    }
}
