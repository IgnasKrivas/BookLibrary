using BookLibrary.Core.Interfaces;
using BookLibrary.Data.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BookLibrary.Core.Services
{
    /// <summary>
    /// Taken books service class
    /// </summary>
    public class PersonServices : IPersonServices
    {
        private List<Person> persons = new List<Person>();
        public PersonServices()
        {
            PersonDatabase();
        }

        public List<Person> GetTakenBooks()
        {
            return persons;
        }

        public void ReturnBook(Person person)
        {
            persons.Remove(person);
            string data = JsonSerializer.Serialize<List<Person>>(persons);
            File.WriteAllText(Environment.Environment.personDataPath, data);
        }

        public void TakeBook(Person person)
        {
            persons.Add(person);
            string data = JsonSerializer.Serialize<List<Person>>(persons);
            File.WriteAllText(Environment.Environment.personDataPath, data);
        }
        void PersonDatabase()
        {
            string route = File.ReadAllText(Environment.Environment.personDataPath);
            persons = JsonSerializer.Deserialize<List<Person>>(route).ToList();
        }
    }
}
