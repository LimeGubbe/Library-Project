using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AdminHandlesBooks
{
    public class BookSystem
    {
        private static BookSystem? instance = null;
        private static string booksFilePath = "C:\\Users\\emil.lindgren8\\source\\repos\\library_project1\\books.txt"; //books.txt
        private static string userFilePath = "C:\\Users\\emil.lindgren8\\source\\repos\\library_project1\\Users.txt";
        private static string rentedbooksFilePath = "C:\\Users\\emil.lindgren8\\source\\repos\\library_project1\\Rented_books.txt"; //rented_books.txt
        private User loggedInUser;
        private List<string> rentedBooks = new List<string>();
        private List<Book> books = new List<Book>();

        public List<Book> GetBooks() { return books; }
        public bool currentUserIsRentingBook(Book book)
        {
            foreach (string line in rentedBooks)
            {
                string[] tokens = line.Split(" ");
                string rentedBookId = tokens[0];
                string userId = tokens[1];

                if (book.Id == rentedBookId && userId == loggedInUser.Id) return true;
            }

            return false;
        }

        private BookSystem()
        {
            SetLoggedInUser();
            LoadBooks();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            Save();
        }

        public void Save()
        {

            string[] booksStringArr = books.Select(book => $"{book.Id} {book.Name} {book.Author} {book.Genre} {book.rowNumber}" ).ToArray();


            File.WriteAllLines(booksFilePath, booksStringArr);
        }

        public static BookSystem GetInstance()
        {
            if (instance == null)
            {
                instance = new BookSystem();
            }
            return instance;
        }

        void SetLoggedInUser()
        {
            string[] usersFromDb = File.ReadAllLines(userFilePath);

            string[] userLineTokens = usersFromDb[0].Split(" ");
            string userId = userLineTokens[0];
            string userName = userLineTokens[1];

            loggedInUser = new User(userId, userName);
        }

        void LoadRentedBooks()
        {
            string[] rentedBooksFromDb = File.ReadAllLines(rentedbooksFilePath);

            foreach (string line in rentedBooksFromDb)
            {
                rentedBooks.Add(line);
            }
        }

        void LoadBooks()
        {
            LoadRentedBooks();

            string[] booksFromDb = File.ReadAllLines(booksFilePath);

            for (var i = 0; i < booksFromDb.Length; i++)
            {
               
                //namn, författare, genre, radNummer
                
                string bookStr = booksFromDb[i];
                string[] bookLineTokens = bookStr.Split(" ");
                string bookId = bookLineTokens[0];
                string bookName = bookLineTokens[1];   //bok namn
                string bookAuthor = bookLineTokens[2];  //bok författare
                string bookGenre = bookLineTokens[3];  //bok genre
                int bookrowNumber = Int32.Parse(bookLineTokens[4]);  //bok radnummer
                bool available = true;

                foreach (string line in rentedBooks)
                {
                    string[] tokens = line.Split(" ");
                    string rentedBooksId = tokens[0];

                    if (rentedBooksId == bookId) available = false;
                }

                Book book = new Book(bookId, bookName, bookAuthor, bookGenre, bookrowNumber, available);
                books.Add(book);
            }
        }

        public bool ReturnBook(Book book)
        {
            var indexToRemove = -1;
            for (int i = 0; i < rentedBooks.Count; i++)
            {
                var line = rentedBooks[i];
                string[] tokens = line.Split(" ");
                string rentedBookId = tokens[0];
                string userId = tokens[1];

                if (book.Id == rentedBookId && userId == loggedInUser.Id)
                {
                    indexToRemove = i;
                }
            }

            if (indexToRemove == -1)
            {
                return false;
            }
            else
            {
                rentedBooks.RemoveAt(indexToRemove);
                File.WriteAllLines(rentedbooksFilePath, rentedBooks);
                book.Available = true;
                return true;
            }
        }

        public void RentBook(Book book)
        {
            var line = $"{book.Id} {loggedInUser.Id}";
            book.Available = false;

            rentedBooks.Add(line);
            File.WriteAllLines(rentedbooksFilePath, rentedBooks);
        }
    }
}
