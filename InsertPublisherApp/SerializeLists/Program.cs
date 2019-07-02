using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnection;
using Entities;
using Newtonsoft;
using System.Collections;

namespace SerializeLists
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            BookRepo bookRepo = new BookRepo(connection);

            List<Book> books = bookRepo.LoadAllBooks();
            Serializer.SerializeJSon(books);
            Serializer.SerializeXML(books);
        }
    }
}
