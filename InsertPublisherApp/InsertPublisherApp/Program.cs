using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using AdoConnection;

namespace PublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            var publisherRepo = new PublisherRepo(connection);

            var pub = new Publisher { Id = 15, PublisherName = "Militara" };
            var id = publisherRepo.InsertPublisher(pub);

            Console.WriteLine($"inserted publisher with {id}");
        }
    }
}
