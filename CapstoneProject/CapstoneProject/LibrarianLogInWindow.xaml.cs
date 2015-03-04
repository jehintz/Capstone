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
    public partial class LibrarianLogInWindow : Window
    {
        Collections cols = new Collections();

        public LibrarianLogInWindow()
        {
            InitializeComponent();
            userIDTextBox.Focus();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Status.IsLibrarian = false;
            this.Close();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            bool isLoggedIn = false;
            bool validUserID = false;

            if(string.IsNullOrWhiteSpace(userIDTextBox.Text))
            {
                errorLabel.Content = "Enter a User ID.";
                userIDTextBox.Focus();
                return;
            }

            if(string.IsNullOrWhiteSpace(passwordTextBox.Password))
            {
                errorLabel.Content = "Enter a password.";
                passwordTextBox.Focus();
                return;
            }

            foreach(var p in cols.PeopleCollection.PeopleList)
            {
                if (p is Librarians)
                {
                    Librarians librarian = p as Librarians;
                    if (userIDTextBox.Text.ToUpper() == librarian.UserID.ToUpper() && passwordTextBox.Password == librarian.Password)
                    {
                        isLoggedIn = true;
                        break;
                    }
                    if (userIDTextBox.Text.ToUpper() == librarian.UserID.ToUpper())
                    {
                        validUserID = true;
                    }
                }
            }

            if (isLoggedIn)
            {
                Status.IsLibrarian = true;
                Status.LoggedInLibrarianID = userIDTextBox.Text;
                this.Close();
            }
            else
            {
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                if (validUserID)
                {
                    errorLabel.Content = "Incorrect password.";
                    passwordTextBox.Focus();
                }
                else
                {
                    errorLabel.Content = "User ID not found.";
                    userIDTextBox.Focus();
                }
            }
        }
    }
}
