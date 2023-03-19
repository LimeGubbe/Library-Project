﻿namespace AdminHandlesBooks
{
    public class Book
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Genre { get; set; }
        public bool Available { get; set; }

        public Book(string bookId, string bookName, string bookAuthor, int bookGenre, bool available = true)
        {
            Id = bookId;
            Name = bookName;
            Author = bookAuthor;
            Genre = bookGenre;
            Available = available;
        }

        public Book(string bookId, string bookName, string bookAuthor, string bookGenre, int bookrowNumber, bool available)
        {
            Available = available;
        }
    }
}
