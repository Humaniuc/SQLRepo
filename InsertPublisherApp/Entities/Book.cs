using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Book : BaseEntity
    {
        private string title;
        private int publisherId;
        private int year;
        private decimal price;

        public string Title { get; set; }
        public int PublisherId { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        //public Book(int bookId, string title, int pubId, int year, int price) : base()
        //{
        //    Title = title;
        //    PublisherId = pubId;
        //    Year = year;
        //    Price = price;
        //}
    }
}
