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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace CapstoneProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //START: Tasks to be completed before the Main Window is displayed upon launch
        public MainWindow()
        {
            InitializeComponent();

            //Set initial values to static variables
            Status.IsLibrarian = false;
            Status.LoggedInLibrarianID = "";

            //Place a text cursor in the Search text box
            searchTextBox.Focus();
        }

        //START: Ensure the Main Window starts in Patron mode, so even if something in the the XAML is defaulted incorrectly, code will change it
        //and the window will never launch with Librarian functions visible
        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckIfLibrarian();
        }

        //METHOD: Change functionality and appearance of Main Window depending on whether the user is a Patron or Librarian
        private void CheckIfLibrarian()
        {
            searchResultsGrid.ItemsSource = null;
            moreInfoColumn.Visibility = System.Windows.Visibility.Hidden;
            //If a librarian is logged in, set administrative buttons to visible and allow the librarian to log out if needed
            if (Status.IsLibrarian)
            {
                librarianLinkTextblock.Text = "(Log Out)";
                checkOutButton.Visibility = System.Windows.Visibility.Visible;
                checkInButton.Visibility = System.Windows.Visibility.Visible;
                editBooksButton.Visibility = System.Windows.Visibility.Visible;
                directoryButton.Visibility = System.Windows.Visibility.Visible;
                overdueButton.Visibility = System.Windows.Visibility.Visible;
                adminModeLabel.Content = "***Librarian Mode***";
                adminModeLabel.Foreground = Brushes.Green;
                librarianWelcomeLabel.Visibility = System.Windows.Visibility.Visible;
                librarianWelcomeLabel.Content = string.Format("Logged in as {0}", Status.LoggedInLibrarianID.ToUpper());
                mainWindow.Background = Brushes.MintCream;
            }
            //If the user is a patron, show only the book search functionality with the option to log in as a librarian
            else
            {
                librarianLinkTextblock.Text = "Librarian Log-in";
                checkOutButton.Visibility = System.Windows.Visibility.Hidden;
                checkInButton.Visibility = System.Windows.Visibility.Hidden;
                editBooksButton.Visibility = System.Windows.Visibility.Hidden;
                directoryButton.Visibility = System.Windows.Visibility.Hidden;
                overdueButton.Visibility = System.Windows.Visibility.Hidden;
                adminModeLabel.Content = "***Patron Mode***";
                adminModeLabel.Foreground = Brushes.Navy;
                librarianWelcomeLabel.Visibility = System.Windows.Visibility.Hidden;
                mainWindow.Background = SystemColors.WindowBrush;
            }
        }

        //EVENT: Close button clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            //Exit the application
            this.Close();
        }
        
        //CLOSE: Make one final write from the collections to the database upon exit
        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: Save any changes
        }

        //EVENT: Librarian Log In link clicked
        private void librarianLinkTextblock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Check if a librarian is already logged in; if they are, log them out by setting IsLibrarian to false
            if (librarianLinkTextblock.Text == "(Log Out)")
            {
                Status.IsLibrarian = false;
            }
            //If no librarian is logged in, open the Log In window to allow for entry of librarian user name and password
            else
            {
                LibrarianLogInWindow logInWindow = new LibrarianLogInWindow();
                logInWindow.ShowDialog();
            }

            //Determine which Main Window interface will be displayed (Patron or Librarian) depending on results of above code
            CheckIfLibrarian();
        }

        //EVENT: A View link in the More Info column of a search result was clicked
        private void moreInfoTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Determine from which row the View link was clicked by retrieving its associated DataContext
            TextBlock selectedTextBlock = (TextBlock)sender;
            BookSearchDisplay selectedBook = (BookSearchDisplay)selectedTextBlock.DataContext;

            //Set the Selected Book property to the ID of the book found in the Data Context
            if (selectedBook != null && selectedBook.ID != 0)
            {
                //Open a window displaying detailed information on the selected book by passing in the book's ID
                MoreInfoWindow miw = new MoreInfoWindow(selectedBook.ID);
                miw.Show();
            }
            else
            {
                //TODO: Once error handling class is created, throw an exception here
            }
        }

        //Todo: Pre-fill in book info when the Edit window is opened in such a manner
        //EVENT: An Edit link in the Change column of a search result was clicked
        private void editBookTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Determine from which row the View link was clicked by retrieving its associated Data Context
            TextBlock selectedTextBlock = (TextBlock)sender;
            BookSearchDisplay selectedBook = (BookSearchDisplay)selectedTextBlock.DataContext;

            //Set the Selected Book property to the ID of the book found in the Data Context
            if (selectedBook != null)
            {
                EditWindow ew = new EditWindow(selectedBook.ID);
                ew.ShowDialog();
            }
            else
            {
                throw new NullReferenceException();
            }

            //Open a window displaying detailed information on the book whose ID is now in the Selected Book property
        }

        //EVENT: Check In button clicked
        private void checkInButton_Click(object sender, RoutedEventArgs e)
        {
            //Open a window which allows a librarian to check in a book
            CheckInWindow ciw = new CheckInWindow();
            ciw.Show();
        }

        //EVENT: Search button clicked
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var cols = new Collections();
            //Create a list of data which will hold the results of the user's search
            List<BookSearchDisplay> dc = new List<BookSearchDisplay>();
            List<BookSearchDisplay> resultsList = new List<BookSearchDisplay>();

            //Remove any error display from previous searches
            errorLabel.Visibility = System.Windows.Visibility.Hidden;

            //Perform a search based on what was selected from the Search In menu
            switch(searchInMenu.Text.ToUpper())
            {  
                //Search for matches in Title, Author, Subject, and ISBN
                case "ALL":
                    cols.BookSearchAll(searchTextBox.Text, out resultsList);

                    foreach (var dataCollectionResult in resultsList)
                    {
                        dc.Add(dataCollectionResult);
                    }
                    break;

                //Search for matches in the Title only
                case "TITLE":
                    cols.BookSearchByTitle(searchTextBox.Text, out resultsList);

                    foreach (var dataCollectionResult in resultsList)
                    {
                        dc.Add(dataCollectionResult);
                    }
                    break;
                   
                //Search for matches in the Author's first and last name only
                case "AUTHOR":
                    cols.BookSearchByAuthor(searchTextBox.Text, out resultsList);

                    foreach (var dataCollectionResult in resultsList)
                    {
                        dc.Add(dataCollectionResult);
                    }
                    break;

                //Search for matches in the ISBN only
                case "ISBN":
                    cols.BookSearchByISBN(searchTextBox.Text, out resultsList);

                    foreach (var dataCollectionResult in resultsList)
                    {
                        dc.Add(dataCollectionResult);
                    }
                    break;

                //Search for matches in the Subject only
                case "SUBJECT":
                    cols.BookSearchSubject(searchTextBox.Text, out resultsList);
                    foreach (var dataCollectionResult in resultsList)
                    {
                        dc.Add(dataCollectionResult);
                    }
                    break;

                //If none of the above terms were selected from the Search In drop down menu, throw an error
                default:
                    ExceptionHandler exHandler = new ExceptionHandler(new ArgumentException("The main window's Search In combo box should only contain the items All, Title, Author, Subject, and ISBN. If a new item was added, please remove it or modify code to accommodate."));
                    break;
            }
            //Fill grid with search results
            searchResultsGrid.ItemsSource = dc;

            //Display message if no results found
            if (dc.Count == 0)
            {
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                errorLabel.Content = "No books within the catalog match this criteria.";
            }

            //Code for the More Info column to appear next to each search result
            FrameworkElementFactory moreInfoTextBlock = new FrameworkElementFactory(typeof(TextBlock));
            if (Status.IsLibrarian)
            {
                moreInfoTextBlock.SetValue(TextBlock.TextProperty, "View/Edit");
            }
            else
            {
                moreInfoTextBlock.SetValue(TextBlock.TextProperty, "View");
            }
            moreInfoTextBlock.SetValue(TextBlock.ForegroundProperty, SystemColors.HotTrackBrush);
            moreInfoTextBlock.SetValue(TextBlock.TextDecorationsProperty, TextDecorations.Underline);
            moreInfoTextBlock.SetValue(TextBlock.CursorProperty, Cursors.Hand);
            moreInfoTextBlock.AddHandler(TextBlock.MouseLeftButtonDownEvent, new MouseButtonEventHandler(moreInfoTextBlock_MouseLeftButtonDown));
            DataTemplate moreInfoTemplate = new DataTemplate();
            moreInfoTemplate.VisualTree = moreInfoTextBlock;
            moreInfoColumn.CellTemplate = moreInfoTemplate;
            moreInfoColumn.DisplayIndex = 7;
            moreInfoColumn.Visibility = System.Windows.Visibility.Visible;
        }

        //EVENT: Add/Edit button clicked
        private void editBooksButton_Click(object sender, RoutedEventArgs e)
        {
            //Open a window which allows librarians to add, edit, or remove books from the catalog
            EditWindow ew = new EditWindow();
            ew.Show();
        }

        //EVENT: Check Out button clicked
        private void checkOutButton_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow cow = new CheckOutWindow();
            cow.Show();
        }

        //EVENT: View Overdue button clicked
        private void overdueButton_Click(object sender, RoutedEventArgs e)
        {
            OverdueWindow ow = new OverdueWindow();
            ow.Show();
        }

        private void directoryButton_Click(object sender, RoutedEventArgs e)
        {
            DirectoryWindow dw = new DirectoryWindow();
            dw.Show();
        }
    }
}
