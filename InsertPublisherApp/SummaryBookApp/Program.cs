using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnection;
using Entities;

namespace SummaryBookApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            BookRepo bookRepo = new BookRepo(connection);

            List<string> books2010 = bookRepo.SelectAllBooksPublishedIn2010();
            Console.WriteLine("Books from 2010: ");
            bookRepo.Print(books2010);

            List<string> booksMaxYear = bookRepo.SelectBooksPublishedInMaxYear();
            Console.WriteLine("Books published in max year: ");
            bookRepo.Print(booksMaxYear);

            List<object> topTenBooks = bookRepo.TopTenPublisher();
            Console.WriteLine("Top 10 books: ");
            bookRepo.PrintTopTen(topTenBooks);
        }
    }
}
