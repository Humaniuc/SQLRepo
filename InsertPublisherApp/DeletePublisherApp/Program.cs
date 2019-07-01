using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnection;

namespace DeletePublisherApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            PublisherRepo publisherRepo = new PublisherRepo(connection);

            publisherRepo.SelectPublisherToDelete();
        }
    }
}
