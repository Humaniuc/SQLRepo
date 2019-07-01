using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnection;
using Entities;

namespace CrudBookApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = ConnectionManager.GetConnection();
            var bookRepo = new BookRepo(connection);

            var book = new Book { Title = "Mizerabilii", PublisherId = 5, Year = 1985, Price = 10000 };
            var id = bookRepo.InsertBook(book);
            Console.WriteLine($"Id of inserted book is: {id}");

            bookRepo.UpdateBook();
            bookRepo.DeleteBook();

            Book b = bookRepo.SelectBook();
            Console.WriteLine($"Book selected: {b.Title} with price of {b.Price}");
        }
    }
}
