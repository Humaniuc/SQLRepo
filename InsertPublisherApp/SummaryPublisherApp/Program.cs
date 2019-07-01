using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using AdoConnection;

namespace SummaryPublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            var publisherRepo = new PublisherRepo(connection);

            var numberOfRows = publisherRepo.NumberOfPublisherRows();
            Console.WriteLine($"Number of Rows from Publisher is {numberOfRows}");

            Console.WriteLine("\nTop ten pubishers: ");
            publisherRepo.Print(publisherRepo.TopTenPublisher());

            Console.WriteLine("\nPublishers with bookCount: ");
            publisherRepo.Print(publisherRepo.PublisherWithNumberOfBooks());

            Console.WriteLine("\nPublishers with total booksPrice");
            publisherRepo.Print(publisherRepo.PublisherWithBooksPrice());
        }
    }
}
