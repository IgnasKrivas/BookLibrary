using BookLibrary.Controller;
using BookLibrary.Core.Interfaces;
using BookLibrary.Core.Services;
using BookLibrary.Data.Models;
using System;
using Xunit;

namespace BookLibrary.Tests
{
    public class BookLibraryUnitTests
    {

        IBookServices _IBookServices = new BookServices();
        IPersonServices _IPersonServices = new PersonServices();


        [Fact]
        public void AddSameBook()
        {

            BookController controller = new BookController(_IBookServices, _IPersonServices);
            Book book = new Book();
            book.Author = "author";
            book.Name = "name";
            book.ISBN = "ISBN";
            book.Language = "language";
            book.Category = "category";
            book.PublicationDate = DateTime.Parse("2012-10-12");

            controller.AddBook(book);

            var countBefore = controller.GetBooks("All", "").Count;
            controller.AddBook(book);
            var countAfter = controller.GetBooks("All", "").Count;

            Assert.Equal(countBefore, countAfter);
            controller.RemoveBook(book.ISBN);
        }
        [Fact]
        public void DeleteBook()
        {
            BookController controller = new BookController(_IBookServices, _IPersonServices);
            Book book = new Book();
            book.Author = "author";
            book.Name = "name";
            book.ISBN = "ISBN";
            book.Language = "language";
            book.Category = "category";
            book.PublicationDate = DateTime.Parse("2012-10-12");
            var countBefore = controller.GetBooks("All", "").Count;
            controller.AddBook(book);

            controller.RemoveBook(book.ISBN);
            var countAfter = controller.GetBooks("All", "").Count;
            Assert.Equal(countBefore, countAfter);

        }
        [Fact]
        public void ReturnBook()
        {
            BookController controller = new BookController(_IBookServices, _IPersonServices);
            Person person = new Person();
            Book book = new Book();
            book.Author = "author";
            book.Name = "name";
            book.ISBN = "ISBN";
            book.Language = "language";
            book.Category = "category";
            book.PublicationDate = DateTime.Parse("2012-10-12");
            controller.AddBook(book);

            person.Name = "name";
            person.ISBN = "ISBN";
            person.ReturnDate = DateTime.Parse("2021-10-15");
            var countBefore = _IPersonServices.GetTakenBooks().Count;
            controller.TakeBook(person);
            controller.ReturnBook(person.ISBN);
            var countAfter = _IPersonServices.GetTakenBooks().Count;
            Assert.Equal(countBefore, countAfter);
            controller.RemoveBook(book.ISBN);
        }
        [Fact]
        public void TakeTakenBook()
        {
            BookController controller = new BookController(_IBookServices, _IPersonServices);
            Person person = new Person();
            Book book = new Book();
            book.Author = "author";
            book.Name = "name";
            book.ISBN = "ISBN";
            book.Language = "language";
            book.Category = "category";
            book.PublicationDate = DateTime.Parse("2012-10-12");
            controller.AddBook(book);
            person.Name = "name";
            person.ISBN = "ISBN";
            person.ReturnDate = DateTime.Parse("2021-10-12");
            controller.TakeBook(person);
            var countBefore = _IPersonServices.GetTakenBooks().Count;
            controller.TakeBook(person);
            var countAfter = _IPersonServices.GetTakenBooks().Count;
            Assert.Equal(countBefore, countAfter);
            controller.ReturnBook(person.ISBN);
            controller.RemoveBook(book.ISBN);
        }
        [Fact]
        public void TakeBookForMoreThan2Months()
        {
            BookController controller = new BookController(_IBookServices, _IPersonServices);
            Person person = new Person();
            person.Name = "name";
            person.ISBN = "ISBN";
            person.ReturnDate = DateTime.Parse("2022-01-20");
            var countBefore = _IPersonServices.GetTakenBooks().Count;
            controller.TakeBook(person);
            var countAfter = _IPersonServices.GetTakenBooks().Count;
            Assert.Equal(countBefore, countAfter);
        }
    }
}
