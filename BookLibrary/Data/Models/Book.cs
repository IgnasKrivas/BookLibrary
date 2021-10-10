using System;

namespace BookLibrary.Data.Models
{
    /// <summary>
    /// Books data model
    /// </summary>
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN { get; set; }

      

        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}\n{4:d/M/yyyy}\n{5}",Name,Author,Category,Language,PublicationDate,ISBN);
        }
    }
}
