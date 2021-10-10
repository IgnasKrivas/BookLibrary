using BookLibrary.Core.Interfaces;
using BookLibrary.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace BookLibrary.Core.Services
{
    /// <summary>
    /// Books service class
    /// </summary>
    public class BookServices : IBookServices
    {
        private List<Book> books = new List<Book>();
        public BookServices()
        {
            BooksDatabase();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            string data = JsonSerializer.Serialize<List<Book>>(books);
            File.WriteAllText(Environment.Environment.booksDataPath,data);
        }

        public void DeleteBook(Book book)
        {
            books.Remove(book);
            string data = JsonSerializer.Serialize<List<Book>>(books);
            File.WriteAllText(Environment.Environment.booksDataPath, data);
        }

        public Book GetBook(string ISBN)
        {
            var book = books.Where(x => x.ISBN == ISBN).SingleOrDefault();
            return book;
        }

        public List<Book> GetBooks(string filter,string search)
        {
            List<Book> filteredBooks = new List<Book>();
            switch (filter)
            {
                case ("All"):
                    return books;
                case ("Author"):
                    filteredBooks = books.Where(x => x.Author == search).ToList();
                    return filteredBooks;
                case ("Category"):
                    filteredBooks = books.Where(x => x.Category == search).ToList();
                    return filteredBooks;
                case ("Language"):
                    filteredBooks = books.Where(x => x.Language == search).ToList();
                    return filteredBooks;
                case ("ISBN"):
                    filteredBooks = books.Where(x => x.ISBN == search).ToList();
                    return filteredBooks;
                case ("Name"):
                    filteredBooks = books.Where(x => x.Name == search).ToList();
                    return filteredBooks;
                default:
                    return filteredBooks;
            }
                    

        }
        void BooksDatabase()
        {
            string route = File.ReadAllText(Environment.Environment.booksDataPath);
            books = JsonSerializer.Deserialize<List<Book>>(route).ToList();
           
        }
    }
}
