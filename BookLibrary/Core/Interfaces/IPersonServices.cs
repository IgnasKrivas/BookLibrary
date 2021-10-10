using BookLibrary.Data.Models;
using System.Collections.Generic;

namespace BookLibrary.Core.Interfaces
{
    public interface IPersonServices
    {
        public void TakeBook(Person person);
        public List<Person> GetTakenBooks();
        public void ReturnBook(Person person);
    }
}
