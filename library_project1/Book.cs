namespace AdminHandlesBooks
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public int rowNumber { get; set; }
        public bool Available { get; set; }

        /*public Book(string bookId, string bookName, string bookAuthor, int bookGenre, int bookrowNumber, bool available = true)
        {
            Id = bookId;
            Name = bookName;
            Author = bookAuthor;
            Genre = bookGenre;
            rowNumber = bookrowNumber;
            Available = available;
        }*/

        public Book(string bookId, string bookName, string bookAuthor, string bookGenre, int bookrowNumber, bool available)
        {
            Id = bookId;
            Name = bookName;
            Author = bookAuthor;
            Genre = bookGenre;
            rowNumber = bookrowNumber;
            Available = available;
        }
    }
}
