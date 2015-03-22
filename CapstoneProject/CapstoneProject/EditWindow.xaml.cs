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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    /// 

    //TODO: Fix error where setting copies to 0 creates an index exception
    //Window to add new books or edit the information of existing books
    public partial class EditWindow : Window
    {
        LibraryInformation2Entities db = new LibraryInformation2Entities();
        Collections cols = new Collections();
        private int copiesInt = 0;
        private int numberCheckedOutInt = 0;

        //START: Edit Window opened
        public EditWindow()
        {
            InitializeComponent();

            //Populate the Subject combo box
            PopulateSubjects();

            //Put the text cursor on the ISBN entry text box when the window is opened
            isbnEntryTextBox.Focus();
        }

        //START: Edit Window opened with a Book ID as an argument
        public EditWindow(int bookID)
        {
            InitializeComponent();

            //Populate the Subject combo box
            PopulateSubjects();
            EnableControls();
            isbnLabel.Content = "ISBN";
            searchButton.Visibility = System.Windows.Visibility.Hidden;

            //Display the incoming book's ISBN in the appropriate text box
            Books incomingBook = cols.BookSearchByID(bookID);
            isbnEntryTextBox.Text = incomingBook.ISBN;

            //Display the rest of the book's data
            LoadBookData(incomingBook);
        }

        //EVENT: ISBN Search button has been clicked
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            //Clear any existing data in the text boxes
            DisableControls();

            //Clear any existing error messages
            isbnErrorLabel.Visibility = System.Windows.Visibility.Hidden;

            //Check that the enter ISBN is less than 25 characters. If longer, display an error message
            if (isbnEntryTextBox.Text.Length > 25)
            {
                isbnErrorLabel.Visibility = System.Windows.Visibility.Visible;
                isbnErrorLabel.Content = "The ISBN cannot exceed 25 numbers.";
                isbnEntryTextBox.Focus();
                return;
            }

            //Check that the entered ISBN only includes numbers. If not, display an error message
            foreach (char c in isbnEntryTextBox.Text.ToCharArray())
            {
                if (!char.IsDigit(c))
                {
                    isbnErrorLabel.Visibility = System.Windows.Visibility.Visible;
                    isbnErrorLabel.Content = "The ISBN can only contain numbers. Do not include dashes.";
                    isbnEntryTextBox.Focus();
                    return;
                }
            }

            //If text has been entered in the ISBN Search text box, compare the entered ISBN to all existing books within the catalog
            if (!string.IsNullOrWhiteSpace(isbnEntryTextBox.Text))
            {
                foreach (var b in cols.BookCollection.BookList)
                {
                    //If a book with a matching ISBN has been found in the catalog, load that book's information into the appropriate
                    //text boxes
                    if (b.ISBN == isbnEntryTextBox.Text.Trim())
                    {
                        MessageBoxResult result = MessageBox.Show("A book with the entered ISBN already exists in the catalog. Would you like to edit its information?",
                            "Edit Info", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            LoadBookData(b);
                            EnableControls();
                        }
                        return;
                    }
                }

                if (string.IsNullOrEmpty(titleEntryTextBox.Text))
                {
                    MessageBoxResult result = MessageBox.Show("No book with the entered ISBN currently exists in the catalog. Would you like to add it?",
                        "ISBN Not Found", MessageBoxButton.YesNo);

                    if(result == MessageBoxResult.Yes)
                    {
                        isbnLabel.IsEnabled = false;
                        isbnEntryTextBox.IsEnabled = false;
                        searchButton.IsEnabled = false;
                        EnableControls();
                        titleEntryTextBox.Focus();
                    }
                }

            //If no text was entered into the ISBN text box, prompt the user to enter an ISBN before searching again
            }
            else
            {
                isbnErrorLabel.Visibility = System.Windows.Visibility.Visible;
                isbnErrorLabel.Content = "Please enter an ISBN.";
                isbnEntryTextBox.Focus();
            }
        }

        //METHOD: Enable most controls and labels so a new book can be added when an ISBN that is not already in the catalog has
        //been entered
        private void EnableControls()
        {
            titleLabel.IsEnabled = true;
            titleEntryTextBox.IsEnabled = true;
            languageLabel.IsEnabled = true;
            languageEntryBox.IsEnabled = true;
            firstNameLabel.IsEnabled = true;
            firstNameEntryBox.IsEnabled = true;
            lastNameLabel.IsEnabled = true;
            lastNameEntryBox.IsEnabled = true;
            bioLabel.IsEnabled = true;
            bioEntryBox.IsEnabled = true;
            publisherLabel.IsEnabled = true;
            publisherEntryBox.IsEnabled = true;
            yearLabel.IsEnabled = true;
            yearEntryBox.IsEnabled = true;
            pagesLabel.IsEnabled = true;
            pagesEntryBox.IsEnabled = true;
            copiesLabel.IsEnabled = true;
            copiesEntryBox.IsEnabled = true;
            subjectLabel.IsEnabled = true;
            subjectComboBox.IsEnabled = true;
            descriptionLabel.IsEnabled = true;
            descriptionEntryBox.IsEnabled = true;
            confirmButton.IsEnabled = true;
            cancelButton.IsEnabled = true;
        }

        //METHOD: Disable most controls and labels so a book cannot be added without first providing an ISBN -OR- so the only variable
        //that can be changed for a book that already exists in the catalog is the number of copies
        private void DisableControls()
        {
            titleLabel.IsEnabled = false;
            titleEntryTextBox.IsEnabled = false;
            titleEntryTextBox.Text = "";
            languageLabel.IsEnabled = false;
            languageEntryBox.IsEnabled = false;
            languageEntryBox.Text = "";
            firstNameLabel.IsEnabled = false;
            firstNameEntryBox.IsEnabled = false;
            firstNameEntryBox.Text = "";
            lastNameLabel.IsEnabled = false;
            lastNameEntryBox.IsEnabled = false;
            lastNameEntryBox.Text = "";
            bioLabel.IsEnabled = false;
            bioEntryBox.IsEnabled = false;
            bioEntryBox.Text = "";
            publisherLabel.IsEnabled = false;
            publisherEntryBox.IsEnabled = false;
            publisherEntryBox.Text = "";
            yearLabel.IsEnabled = false;
            yearEntryBox.IsEnabled = false;
            yearEntryBox.Text = "";
            pagesLabel.IsEnabled = false;
            pagesEntryBox.IsEnabled = false;
            pagesEntryBox.Text = "";
            copiesLabel.IsEnabled = false;
            copiesEntryBox.IsEnabled = false;
            copiesEntryBox.Text = "";
            subjectLabel.IsEnabled = false;
            subjectComboBox.IsEnabled = false;
            subjectComboBox.Text = "";
            descriptionLabel.IsEnabled = false;
            descriptionEntryBox.IsEnabled = false;
            descriptionEntryBox.Text = "";
            confirmButton.IsEnabled = false;
            cancelButton.IsEnabled = false;
        }

        //EVENT: A subject has been selected from the Subject combo box
        private void subjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //If the last item in the Subject combo box has been selected (this will always be the 'Add new...' option), set the
            //Subject Entry text box to visible
            if (subjectComboBox.SelectedIndex == (subjectComboBox.Items.Count - 1))
            {
                subjectEntryBox.Visibility = System.Windows.Visibility.Visible;
                newSubjectLabel.Visibility = System.Windows.Visibility.Visible;
                subjectLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            //If any other item is selected (meaning it is a pre-existing subject), hide the Subject Entry text box
            else
            {
                subjectEntryBox.Visibility = System.Windows.Visibility.Hidden;
                newSubjectLabel.Visibility = System.Windows.Visibility.Hidden;
                subjectLabel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //EVENT: Cancel button clicked
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            //If the ISBN entry box is disabled (meaning the book editing/entry stage has been entered), confirm that the user means to exit
            //without saving
            if (!isbnEntryTextBox.IsEnabled)
            {
                MessageBoxResult result = MessageBox.Show("Exit without saving? All changes will be lost.", "Close Window", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    this.Close();
                }
            }
        }

        //EVENT: Confirm button clicked
        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateData())
            {
                if (copiesEntryBox.Text.Trim() == "0")
                {
                    MessageBoxResult result2 = MessageBox.Show("Setting the number of copies to 0 will remove this book from the catalog. It will no longer appear in searches and will be unavailable for check out. Proceed?",
                        "Remove Book", MessageBoxButton.YesNo);
                    if (result2 == MessageBoxResult.Yes)
                    {
                        foreach (var b in cols.BookCollection.BookList)
                        {
                            if (isbnEntryTextBox.Text.Trim() == b.ISBN)
                            {
                                int ID = b.BookID;
                                AddBook(ID);
                                break;
                            }
                        }
                    }
                    else
                        return;
                }
                else
                {
                    //If all the above checks have passed, confirm with the librarian that all changes will be saved and final
                    MessageBoxResult result = MessageBox.Show("Save and exit? Once saved, all changes will be final.",
                            "Save and Exit", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        bool existingBook = false;
                        foreach (var b in cols.BookCollection.BookList)
                        {
                            if (isbnEntryTextBox.Text.Trim() == b.ISBN)
                            {
                                int ID = b.BookID;
                                existingBook = true;
                                AddBook(ID);
                                break;
                            }
                        }
                        if (!existingBook)
                        {
                            AddBook();
                        }
                    }
                    else
                        return;
                }
                this.Close();
            }
        }

        //EVENT: Letter added or deleted from the First Name entry box
        private void firstNameEntryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            firstNameComboBox.Items.Clear();
            if (!string.IsNullOrWhiteSpace(firstNameEntryBox.Text))
            {
                foreach (var p in cols.PeopleCollection)
                {
                    if (p is Authors)
                    {
                        if (p.FirstName.ToUpper().StartsWith(firstNameEntryBox.Text.Trim().ToUpper()))
                        {
                            if (p.FirstName.ToUpper() == firstNameEntryBox.Text.Trim().ToUpper())
                            {
                                firstNameComboBox.Visibility = System.Windows.Visibility.Collapsed;
                                break;
                            }
                            firstNameComboBox.Items.Add(p.FirstName);
                            firstNameComboBox.Visibility = System.Windows.Visibility.Visible;
                            firstNameComboBox.IsDropDownOpen = true;
                        }
                    }
                }
            }
            if (firstNameComboBox.Items.Count == 0)
            {
                firstNameComboBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        //EVENT: Letter added or deleted from the Last Name entry box
        private void lastNameEntryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            lastNameComboBox.Items.Clear();
            if (!string.IsNullOrWhiteSpace(lastNameEntryBox.Text))
            {
                foreach (var p in cols.PeopleCollection)
                {
                    if (p is Authors)
                    {
                        if (p.LastName.ToUpper().StartsWith(lastNameEntryBox.Text.Trim().ToUpper()))
                        {
                            if (p.LastName.ToUpper() == lastNameEntryBox.Text.Trim().ToUpper())
                            {
                                lastNameComboBox.Visibility = System.Windows.Visibility.Collapsed;
                                break;
                            }
                            lastNameComboBox.Items.Add(p.LastName);
                            lastNameComboBox.Visibility = System.Windows.Visibility.Visible;
                            lastNameComboBox.IsDropDownOpen = true;
                        }
                    }
                }
            }
            if (lastNameComboBox.Items.Count == 0)
            {
                lastNameComboBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        //EVENT: Existing author's first name selected from the First Name combo box
        private void firstNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (firstNameComboBox.SelectedItem != null)
            {
                firstNameEntryBox.Text = firstNameComboBox.SelectedItem.ToString();
            }
            CheckIfNewAuthor();
        }

        //EVENT: Existing author's last name selected from the Last Name combo box 
        private void lastNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lastNameComboBox.SelectedItem != null)
            {
                lastNameEntryBox.Text = lastNameComboBox.SelectedItem.ToString();
            }
            CheckIfNewAuthor();
        }

        //EVENT: Text cursor moved away from Last Name entry box
        private void lastNameEntryBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckIfNewAuthor();
        }

        //EVENT: Text cursor moved away from First Name entry box
        private void firstNameEntryBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckIfNewAuthor();
        }

        //METHOD: If the given author already exists in the database, disable the Author Bio section; otherwise, enable it to allow for
        //the entry of a bio when a new author is being added
        private void CheckIfNewAuthor()
        {
            if (!string.IsNullOrWhiteSpace(firstNameEntryBox.Text) || !string.IsNullOrWhiteSpace(lastNameEntryBox.Text))
            {
                foreach (var p in cols.PeopleCollection)
                {
                    if (p is Authors)
                    {
                        if ((firstNameEntryBox.Text.Trim().ToUpper() == p.FirstName.ToUpper()) &&
                            (lastNameEntryBox.Text.Trim().ToUpper() == p.LastName.ToUpper()))
                        {
                            bioLabel.Visibility = System.Windows.Visibility.Hidden;
                            bioEntryBox.Visibility = System.Windows.Visibility.Hidden;
                            break;
                        }
                        else
                        {
                            bioLabel.Visibility = System.Windows.Visibility.Visible;
                            bioEntryBox.Visibility = System.Windows.Visibility.Visible;
                        }
                    }
                }
            }
            else
            {
                bioLabel.Visibility = System.Windows.Visibility.Visible;
                bioEntryBox.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //Fill the Subject combo box with all existing book subjects and a final item that allows the librarian to add a new subject
        private void PopulateSubjects()
        {
            //Load the subject of every book into a temporary list, sort the list alphabetically, then load all non-duplicates
            //into the Subject combo box
            List<string> temp = new List<string>();
            foreach (var b in cols.BookCollection.BookList)
            {
                temp.Add(b.Subject);
            }
            temp.Sort();
            subjectComboBox.Items.Add(temp[0]);
            for (int i = 1; i < temp.Count; i++)
            {
                if (temp[i].ToUpper() != temp[i - 1].ToUpper())
                {
                    subjectComboBox.Items.Add(temp[i]);
                }
            }

            //Append an Add New option to the end of the Subject combo box, in case an appropriate subject is not found among
            //the pre-existing choices when entering new books into the catalog
            subjectComboBox.Items.Add("Add New...");
        }

        //METHOD: Load an existing book's data into the text boxes
        private void LoadBookData(Books b)
        {
            confirmButton.IsEnabled = true;
            cancelButton.IsEnabled = true;
            isbnEntryTextBox.IsEnabled = false;
            searchButton.IsEnabled = false;
            isbnLabel.IsEnabled = false;
            titleEntryTextBox.Text = b.Title;
            languageEntryBox.Text = b.Language;
            foreach (var p in cols.PeopleCollection)
            {
                if (b.AuthorID == p.PersonID)
                {
                    firstNameEntryBox.Text = p.FirstName;
                    lastNameEntryBox.Text = p.LastName;
                    bioLabel.Visibility = System.Windows.Visibility.Hidden;
                    bioEntryBox.Visibility = System.Windows.Visibility.Hidden;
                    firstNameComboBox.Visibility = System.Windows.Visibility.Hidden;
                    lastNameComboBox.Visibility = System.Windows.Visibility.Hidden;
                    break;
                }
            }
            publisherEntryBox.Text = b.Publisher;
            yearEntryBox.Text = b.YearPublished.ToString();
            pagesEntryBox.Text = b.NumPages.ToString();
            copiesInt = b.NumberOfCopies;
            copiesEntryBox.Text = copiesInt.ToString();
            for (int i = 0; i < subjectComboBox.Items.Count; i++)
            {
                if (b.Subject == subjectComboBox.Items[i].ToString())
                {
                    subjectComboBox.SelectedIndex = i;
                    break;
                }
            }
            descriptionEntryBox.Text = b.Description;
            numberCheckedOutInt = b.NumberOfCopies - b.Availability;
        }

        //METHOD: Ensure all data entered by the librarian is valid/exists
        private bool ValidateData()
        {
            if (string.IsNullOrWhiteSpace(titleEntryTextBox.Text))
            {
                errorLabel.Content = "Please enter a title.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                titleEntryTextBox.Focus();
                return false;
            }
            else if (titleEntryTextBox.Text.Length > 30)
            {
                errorLabel.Content = "The book's title cannot exceed 30 characters.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                titleEntryTextBox.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(languageEntryBox.Text))
            {
                errorLabel.Content = "Please enter the book's language.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                languageEntryBox.Focus();
                return false;
            }

            //Make sure the author's first and last name was entered
            if (string.IsNullOrWhiteSpace(firstNameEntryBox.Text) || string.IsNullOrWhiteSpace(lastNameEntryBox.Text))
            {
                errorLabel.Content = "Please enter the author's full name (first and last).";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                firstNameEntryBox.Focus();
                return false;
            }
            else if (firstNameEntryBox.Text.Length > 25 || lastNameEntryBox.Text.Length > 25)
            {
                errorLabel.Content = "The author's first/last name cannot exceed 25 characters.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                firstNameEntryBox.Focus();
                return false;
            }

            if (bioEntryBox.Visibility == System.Windows.Visibility.Visible && string.IsNullOrWhiteSpace(bioEntryBox.Text))
            {
                errorLabel.Content = "To add a new author to the system, a brief author bio must be included.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                bioEntryBox.Focus();
                return false;
            }
            else if (bioEntryBox.Text.Length > 100)
            {
                errorLabel.Content = "The author bio cannot be more than 100 characters.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                bioEntryBox.Focus();
                return false;
            }

            //Make sure a publisher was entered
            if (string.IsNullOrEmpty(publisherEntryBox.Text))
            {
                errorLabel.Content = "Please enter a publisher.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                publisherEntryBox.Focus();
                return false;
            }

            //Make sure a published year was entered
            if (string.IsNullOrEmpty(publisherEntryBox.Text))
            {
                errorLabel.Content = "Please enter the year the book was published.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                yearEntryBox.Focus();
                return false;
            }

            //Make sure the entered year is an int
            int yearInt = 0;
            if (!int.TryParse(yearEntryBox.Text, out yearInt))
            {
                errorLabel.Content = "The published year can only contain numbers. (Ex: 1999)";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                yearEntryBox.Focus();
                return false;
            }
            //Make sure the entered year falls between 0 and the current year
            else if (yearInt < 1400 || yearInt > DateTime.Now.Year)
            {
                errorLabel.Content = "The published year must be between 1400 and " + DateTime.Now.Year + ".";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                yearEntryBox.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(pagesEntryBox.Text))
            {
                errorLabel.Content = "Please enter the number of pages.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                pagesEntryBox.Focus();
                return false;
            }

            int pagesInt = 0;
            if (!int.TryParse(pagesEntryBox.Text, out pagesInt))
            {
                errorLabel.Content = "The number of pages must be a positive, whole number. (Ex: 375)";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                pagesEntryBox.Focus();
                return false;
            }
            else if (pagesInt < 1)
            {
                errorLabel.Content = "The number of pages cannot be negative and must be greater than 1.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                pagesEntryBox.Focus();
                return false;
            }

            //Make sure the number of copies is an int
            int copiesInt = 0;
            if (!int.TryParse(copiesEntryBox.Text, out copiesInt))
            {
                errorLabel.Content = "Please enter the number of copies. If the book is being removed from the catalog, enter 0.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                copiesEntryBox.Focus();
                return false;
            }
            //Make sure the number of copies is not negative or over 100 (being a small library, 100+ copies of one book will not be needed)
            else if (copiesInt < 0 || copiesInt > 100)
            {
                errorLabel.Content = "The number of copies must be between 0 and 100.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                copiesEntryBox.Focus();
                return false;
            }
            else if (numberCheckedOutInt > int.Parse(copiesEntryBox.Text.Trim()))
            {
                if (numberCheckedOutInt == 1)
                    errorLabel.Content = "1 copy of this book is currently checked out and cannot be removed from the system. Enter a number greater than 1.";
                else
                    errorLabel.Content = numberCheckedOutInt + " copies of this book are currently checked out and cannot be removed from the system. Enter a number greater than " + numberCheckedOutInt + ".";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                copiesEntryBox.Focus();
                return false;
            }

            //Make sure a subject was chosen or entered
            if (subjectComboBox.SelectedIndex < 0 || (subjectComboBox.SelectedIndex == subjectComboBox.Items.Count - 1 && string.IsNullOrWhiteSpace(subjectEntryBox.Text)))
            {
                errorLabel.Content = "Please select or enter a subject.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                subjectEntryBox.Focus();
                return false;
            }

            //Make sure a description was entered
            if (string.IsNullOrWhiteSpace(descriptionEntryBox.Text))
            {
                errorLabel.Content = "Please enter a brief description of the book.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                descriptionEntryBox.Focus();
                return false;
            }
            else if (descriptionEntryBox.Text.Length > 100)
            {
                errorLabel.Content = "The book's description cannot exceed 100 characters.";
                errorLabel.Visibility = System.Windows.Visibility.Visible;
                descriptionEntryBox.Focus();
                return false;
            }

            return true;
        }

        //METHOD: Create a new book by retrieving the data entered by the librarian, then add it to the catalog and collection
        private void AddBook()
        {
            //If the book's author already exists in the database, find that author's Person ID
            int newAuthorID = 0;
            foreach (var p in cols.PeopleCollection)
            {
                if ((firstNameEntryBox.Text.Trim().ToUpper() == p.FirstName.ToUpper()) && (lastNameEntryBox.Text.Trim().ToUpper() == p.LastName.ToUpper()))
                {
                    newAuthorID = p.PersonID;
                    break;
                }
            }
            //If the book's author is new to the database, create a new Person ID and add it, with all their info, to the database
            if (newAuthorID == 0)
            {
                newAuthorID = cols.PeopleCollection.Count + 1;
                Authors newAuthor = new Authors(newAuthorID, firstNameEntryBox.Text.Trim(), lastNameEntryBox.Text.Trim(), bioEntryBox.Text.Trim());
                cols.PeopleCollection[0] = newAuthor;
                db.People.Add(newAuthor);
                db.SaveChanges();
            }
            //Get the book's subject, either from the drop-down menu (pre-existing subject) or text box (if the subject is being newly added)
            string newSubject;
            if (subjectComboBox.SelectedIndex >= 0 && subjectComboBox.SelectedIndex < subjectComboBox.Items.Count - 1)
            {
                newSubject = subjectComboBox.SelectedItem.ToString();
            }
            else
            {
                newSubject = subjectEntryBox.Text.Trim();
            }
            Books newBook = new Books(cols.BookCollection.BookList.Count + 1, isbnEntryTextBox.Text.Trim(), titleEntryTextBox.Text.Trim(),
                newAuthorID, int.Parse(pagesEntryBox.Text.Trim()), newSubject, descriptionEntryBox.Text.Trim(), publisherEntryBox.Text.Trim(),
                int.Parse(yearEntryBox.Text.Trim()), languageEntryBox.Text.Trim(), int.Parse(copiesEntryBox.Text.Trim()));
            cols.BookCollection.BookList[0] = newBook;
            db.Books.Add(newBook);
            db.SaveChanges();
        }

        private void AddBook (int bookID)
        {
            //If the book's author already exists in the database, find that author's Person ID
            int newAuthorID = 0;
            foreach (var p in cols.PeopleCollection)
            {
                if ((firstNameEntryBox.Text.Trim().ToUpper() == p.FirstName.ToUpper()) && (lastNameEntryBox.Text.Trim().ToUpper() == p.LastName.ToUpper()))
                {
                    newAuthorID = p.PersonID;
                    break;
                }
            }
            //If the book's author is new to the database, create a new Person ID and add it, with all their info, to the database
            if (newAuthorID == 0)
            {
                newAuthorID = cols.PeopleCollection.Count + 1;
                Authors newAuthor = new Authors(newAuthorID, firstNameEntryBox.Text.Trim(), lastNameEntryBox.Text.Trim(), bioEntryBox.Text.Trim());
                cols.PeopleCollection[0] = newAuthor;
                db.People.Add(newAuthor);
                db.SaveChanges();
            }
            //Get the book's subject, either from the drop-down menu (pre-existing subject) or text box (if the subject is being newly added)
            string newSubject;
            if (subjectComboBox.SelectedIndex >= 0 && subjectComboBox.SelectedIndex < subjectComboBox.Items.Count - 1)
            {
                newSubject = subjectComboBox.SelectedItem.ToString();
            }
            else
            {
                newSubject = subjectEntryBox.Text.Trim();
            }
            Books newBook = new Books(bookID, isbnEntryTextBox.Text.Trim(), titleEntryTextBox.Text.Trim(),
                newAuthorID, int.Parse(pagesEntryBox.Text.Trim()), newSubject, descriptionEntryBox.Text.Trim(), publisherEntryBox.Text.Trim(),
                int.Parse(yearEntryBox.Text.Trim()), languageEntryBox.Text.Trim(), int.Parse(copiesEntryBox.Text.Trim()));
            for (int i = 0; i <= cols.BookCollection.BookList.Count; i++)
            {
                if (cols.BookCollection.BookList[i].BookID == newBook.BookID)
                {
                    cols.BookCollection.BookList[i] = newBook;
                    break;
                }
            }

            Books dbBook = (from b in db.Books where b.BookID == newBook.BookID select b).FirstOrDefault();
            dbBook.Title = newBook.Title;
            dbBook.AuthorID = newBook.AuthorID;
            dbBook.NumPages = newBook.NumPages;
            dbBook.Subject = newBook.Subject;
            dbBook.Description = newBook.Description;
            dbBook.Publisher = newBook.Publisher;
            dbBook.YearPublished = newBook.YearPublished;
            dbBook.Language = newBook.Language;
            dbBook.NumberOfCopies = newBook.NumberOfCopies;
            db.SaveChanges();
        }
    }
}
