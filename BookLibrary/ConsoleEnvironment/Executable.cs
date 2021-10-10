using BookLibrary.Controller;
using BookLibrary.Core.Interfaces;
using BookLibrary.Core.Services;
using BookLibrary.Data.Models;
using System;

namespace BookLibrary.ConsoleEnvironment
{
    /// <summary>
    /// Console logic
    /// </summary>
    public class Executable
    {
        private readonly BookController _bookController;

        public Executable()
        {
            IBookServices _IBookServices = new BookServices();
            IPersonServices _IPersonServices = new PersonServices();
            _bookController = new BookController(_IBookServices,_IPersonServices);
        }

        /// <summary>
        /// Book library loop logic
        /// </summary>
        public void Execute()
        {
            Console.WriteLine("Welcome to Book Library");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("COMMANDS LIST:");
                Console.WriteLine("1 - Add Book \n2 - Take Book \n3 - Return Book \n4 - Get Books \n5 - Delete Book \n");
                switch (Console.ReadLine())
                {
                    case ("1"):
                        {
                            AddBook();
                            break;
                        }
                    case ("2"):
                        {
                            TakeBook();
                            break;
                        }
                    case ("3"):
                        {
                            ReturnBook();
                            break;
                        }
                    case ("4"):
                        {
                            GetBooks();
                            break;
                        }
                    case ("5"):
                        {
                            DeleteBook();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("There is no such command!");
                            break;
                        }
                }
                Console.WriteLine();
               
            }
        }

        /// <summary>
        /// Get Books List by chosen filter
        /// </summary>
        public void GetBooks()
        {
            Console.WriteLine("Filtering Books List");

            Console.WriteLine("Filter by:");
            string filter; //atribute by which books are filtered
            string search; //search term used in filtering
            Console.WriteLine("1) Get unfiltered books \n2) Author \n3) Category \n4) Language \n5) ISBN \n6) Name \n7) Availability");
            switch (Console.ReadLine())
            {
                case ("1"):
                    {
                        filter = "All";
                        search = null;
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("2"):
                    {
                        filter = "Author";
                        Console.WriteLine("Which author books should be filtered?");
                        search = Console.ReadLine();
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("3"):
                    {
                        filter = "Category";
                        Console.WriteLine("Which category books should be filtered?");
                        search = Console.ReadLine();
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("4"):
                    {
                        filter = "Language";
                        Console.WriteLine("Which language books should be filtered?");
                        search = Console.ReadLine();
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("5"):
                    {
                        filter = "ISBN";
                        Console.WriteLine("Which ISBN books should be filtered?");
                        search = Console.ReadLine();
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("6"):
                    {
                        filter = "Name";
                        Console.WriteLine("Which books should be filtered?");
                        search = Console.ReadLine();
                        _bookController.GetBooks(filter, search);
                        break;
                    }
                case ("7"):
                    {
                        filter = "All";
                        Console.WriteLine("1) Get taken books \n2) Get free books \n");
                        search = Console.ReadLine();
                        _bookController.GetBooksByAvailability(filter, search);
                        break;
                    }
                default:
                    Console.WriteLine("There is no such of filter!");
                    break;
            }
        }

        /// <summary>
        /// Method to add books to library
        /// </summary>
        public void AddBook()
        {
            while (true)
            {
                Console.WriteLine("Adding book");
                Console.WriteLine("Write books name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Write books author: ");
                string author = Console.ReadLine();
                Console.WriteLine("Write books category: ");
                string category = Console.ReadLine();
                Console.WriteLine("Write in which language book is writen: ");
                string language = Console.ReadLine();
                Console.WriteLine("Write books publication date [yyyy-mm-dd]: ");
                string date = Console.ReadLine();
                DateTime publicationdate;
                if (DateTime.TryParse(date, out publicationdate))
                {
                    String.Format("{0:d/M/yyyy}", publicationdate);
                    publicationdate = DateTime.Parse(date);
                }
                else
                {
                    Console.WriteLine("Invalid date format");
                    break;
                }
                Console.WriteLine("Write books ISBN: ");
                string ISBN = Console.ReadLine();
                Book book = new Book()
                {
                    Name = name,
                    Author = author,
                    Category = category,
                    Language = language,
                    PublicationDate = publicationdate,
                    ISBN = ISBN
                };
                var filteredBook = _bookController.AddBook(book);
                if (filteredBook != null) 
                {
                    Console.WriteLine("Book {0} has been added to library!", filteredBook.Name);
                }
                break;
            }
        }
        /// <summary>
        /// Method to take books from library
        /// </summary>
        public void TakeBook()
        {
            while (true)
            {
                Console.WriteLine("Taking book");
                Console.WriteLine("Who is taking book?");
                string name = Console.ReadLine();
                Console.WriteLine("Which book is being taken? [Write ISBN number]");
                string ISBN = Console.ReadLine();
                Console.WriteLine("For what time period book is being taken? [in days]");
                DateTime today = DateTime.Now;
                DateTime returnDate;
                try
                {
                    var days = int.Parse(Console.ReadLine());
                    returnDate = today.AddDays(days);
                }
                catch { 
                    Console.WriteLine("Corrupted input");
                    break;
                }
                Person person = new Person()
                {
                    Name = name,
                    ISBN = ISBN,
                    ReturnDate = returnDate
                };
                var takenBook = _bookController.TakeBook(person);
                if (takenBook != null)
                {
                    Console.WriteLine("Book [{0}] is being taken by {1} until {2:d/M/yyyy}", takenBook.ISBN, takenBook.Name, takenBook.ReturnDate);
                }
                break;
            }
        }
        /// <summary>
        /// Method to return books to library
        /// </summary>
        public void ReturnBook()
        {
            Console.WriteLine("Returning book");
            Console.WriteLine("Which book is being returned?");
            string ISBN = Console.ReadLine();
            var returnedBook = _bookController.ReturnBook(ISBN);
            if (returnedBook != null)
            {
                Console.WriteLine("Book [{0}] has been returned!", returnedBook.ISBN);
            }
        }
        /// <summary>
        /// Method to delete books from library
        /// </summary>
        public void DeleteBook()
        {
            Console.WriteLine("Delete Book");
            Console.WriteLine("Write books ISBN: ");
            string ISBN = Console.ReadLine();
            var deletedBook = _bookController.RemoveBook(ISBN);
            if (deletedBook != null)
            {
                Console.WriteLine("Book [{0}] has been deleted!", deletedBook.Name);
            }
        }
    }
}
