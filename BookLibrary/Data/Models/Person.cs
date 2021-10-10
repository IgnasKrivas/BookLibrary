using System;

namespace BookLibrary.Data.Models
{
    /// <summary>
    /// Taken books data model
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string ISBN { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
