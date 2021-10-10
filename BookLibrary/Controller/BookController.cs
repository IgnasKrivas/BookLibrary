using BookLibrary.Core.Interfaces;
using BookLibrary.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Controller
{
    public class BookController
    {
        private readonly IBookServices _IBookServices;
        private readonly IPersonServices _IPersonServices;
        public BookController(IBookServices IBookServices, IPersonServices IPersonServices)
        {
            _IBookServices = IBookServices;
            _IPersonServices = IPersonServices;
        }
        /// <summary>
        /// Return filtered book list
        /// </summary>
        /// <param name="filter">Filtered atribute</param>
        /// <param name="search">Filter search term</param>
        /// <returns>Book list</returns>
        public List<Book> GetBooks(string filter,string search)
        {

            List<Book> books = _IBookServices.GetBooks(filter,search);
            foreach (var b in books)
            {
                Console.WriteLine("--------------");
                Console.WriteLine(b.ToString());
                Console.WriteLine("--------------");
            }
            return books;
        }
        /// <summary>
        /// Return filtered book list
        /// </summary>
        /// <param name="filter">Filtered atribute</param>
        /// <param name="search">Filter search term</param>
        public void GetBooksByAvailability(string filter, string search)
        {

            List<Book> books = _IBookServices.GetBooks(filter, search);
            List<Person> takenBooks = _IPersonServices.GetTakenBooks();
            List<Book> availableBooks = new List<Book>();
            List<Book> reservedBooks = new List<Book>();
            availableBooks = AvailableBooks(availableBooks, takenBooks, books);
            reservedBooks = ReservedBooks(reservedBooks, takenBooks, books);

            if (search == "1")
            {
                foreach (var rbooks in reservedBooks)
                {
                    Console.WriteLine("--------------");
                    Console.WriteLine(rbooks.ToString());
                    Console.WriteLine("--------------");
                }
            }
            else if (search == "2")
            {
                foreach (var abooks in availableBooks)
                {
                    Console.WriteLine("--------------");
                    Console.WriteLine(abooks.ToString());
                    Console.WriteLine("--------------");
                }
            }
            else
            {
                Console.WriteLine("There is no such command!");
            }
            
        }
        /// <summary>
        /// Returns available books list
        /// </summary>
        /// <param name="availableBooks">Not taken books list</param>
        /// <param name="takenBooks">Taken books list</param>
        /// <param name="books">All book list</param>
        /// <returns>Available books</returns>
        public List<Book> AvailableBooks(List<Book> availableBooks, List<Person> takenBooks, List<Book> books)
        {
            foreach (var b in takenBooks)
            {
                availableBooks = books.Where(x => x.ISBN != b.ISBN).ToList();
            }
            return availableBooks;
        }
        /// <summary>
        /// Returns reserved books list
        /// </summary>
        /// <param name="reservedBooks">Take books list</param>
        /// <param name="takenBooks">Taken books list</param>
        /// <param name="books">All book list</param>
        /// <returns>Reserved books</returns>
        public List<Book> ReservedBooks(List<Book> reservedBooks, List<Person> takenBooks, List<Book> books)
        {
            foreach (var b in takenBooks)
            {
                reservedBooks = books.Where(x => x.ISBN == b.ISBN).ToList();
            }
            return reservedBooks;
        }
        /// <summary>
        /// Adds book to library
        /// </summary>
        /// <param name="book">Book</param>
        /// <returns>Added book</returns>
        public Book AddBook(Book book)
        {
            List<Book> books = _IBookServices.GetBooks("All", "");
            if (books.Where(x => x.ISBN == book.ISBN).SingleOrDefault() != null){
                Console.WriteLine("Book with [{0}] ISBN number already exist!", book.ISBN);
                return null;
            }
            else
            {
                _IBookServices.AddBook(book);
            }
            return book;
        }
        /// <summary>
        /// Takes book from library
        /// </summary>
        /// <param name="person">Taken book</param>
        /// <returns>Taken book</returns>
        public Person TakeBook(Person person)
        {
            string filter="All", search=null;
            List<Book> books = _IBookServices.GetBooks(filter, search);
            List<Person> takenBooks = _IPersonServices.GetTakenBooks();
            if (books.Where(x => x.ISBN == person.ISBN).SingleOrDefault() == null)
            {
                Console.WriteLine("Book [{0}] doesn't exist!", person.ISBN);
                return null;
            }
            else if (IsBookTaken(person.ISBN, takenBooks))
            {
                Console.WriteLine("Book [{0}] is already taken!", person.ISBN);
                return null;
            }
            else if (TakeBookDaysRestriction(person))
            {
                return null;
            }
            else if (TakenBooksByPerson(person, takenBooks))
            {
                return null;
            }
            else
            {
                _IPersonServices.TakeBook(person);
            }
            return person;
        }
        /// <summary>
        /// Returns bool value if book is taken or not
        /// </summary>
        /// <param name="ISBN">ISBN number</param>
        /// <param name="takenBooks">Taken books</param>
        /// <returns>bool value</returns>
        public bool IsBookTaken(string ISBN,List<Person> takenBooks)
        {
            var takenBook = takenBooks.Where(x => x.ISBN == ISBN).SingleOrDefault();
            if (takenBook != null)
            {
                return true;
            }
            else 
                return false;
        }
        /// <summary>
        /// Returns bool value if book return date is correct
        /// </summary>
        /// <param name="person">Taken books</param>
        /// <returns>bool value</returns>
        public bool TakeBookDaysRestriction(Person person)
        {
            DateTime today = DateTime.Now;
            int days = person.ReturnDate.Subtract(today).Days;
            if (days >= 60)
            {
                Console.WriteLine("Can not take book for longer than 2 months!");
                return true;
            }
            else if (days <= 0)
            {
                Console.WriteLine("Return date should be after present!");
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Returns bool value if person has taken more than 3 books
        /// </summary>
        /// <param name="person">Taken books</param>
        /// <param name="takenBooks">Taken books</param>
        /// <returns>bool value</returns>
        public bool TakenBooksByPerson(Person person,List<Person> takenBooks)
        {
            var count = takenBooks.Where(x => x.Name == person.Name).Count();
            if(count > 3)
            {
                Console.WriteLine("{0} can not take any more books!", person.Name);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Returns taken book
        /// </summary>
        /// <param name="ISBN">ISBN number</param>
        /// <returns>taken book</returns>
        public Person ReturnBook(string ISBN)
        {
            List<Person> takenBooks = _IPersonServices.GetTakenBooks();
            var takenBook = takenBooks.Where(x => x.ISBN == ISBN).SingleOrDefault();
            LateReturn(takenBook);
            if (takenBook == null)
            {
                Console.WriteLine("Book [{0}] is not taken!", ISBN);
                return null;
            }
            
            else
            {
                _IPersonServices.ReturnBook(takenBook);
            }

            return takenBook;
        }
        /// <summary>
        /// Informs if books was not returned on time
        /// </summary>
        /// <param name="takenBook">taken book</param>
        public void LateReturn(Person takenBook)
        {
            DateTime today = DateTime.Now;
            if(today > takenBook.ReturnDate)
            {
                Console.WriteLine("{0} did not return books on time :(", takenBook.Name);
            }
        }
        /// <summary>
        /// Returns removed book from library
        /// </summary>
        /// <param name="ISBN">ISBN number</param>
        /// <returns>removed book</returns>
        public Book RemoveBook(string ISBN)
        {
            var book = _IBookServices.GetBook(ISBN);
            List<Person> takenBooks = _IPersonServices.GetTakenBooks();
            if (IsBookTaken(ISBN,takenBooks))
            {
                Console.WriteLine("Can not delete taken book!");
                return null;
            }
            else
            {
                _IBookServices.DeleteBook(book);
            }
            return book;
        }

    }
}
