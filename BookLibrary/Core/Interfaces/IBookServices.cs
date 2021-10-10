using BookLibrary.Data.Models;
using System.Collections.Generic;

namespace BookLibrary.Core.Interfaces
{
    public interface IBookServices
    {
        List<Book> GetBooks(string filter,string search);
        Book GetBook(string ISBN);
        void AddBook(Book book);
        void DeleteBook(Book book);

    }
}
