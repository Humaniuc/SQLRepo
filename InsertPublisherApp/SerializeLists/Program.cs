using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoConnection;
using Entities;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;
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

            if (File.Exists("Books.json"))
            {
                File.Delete("Books.json");
            }
            foreach (Book book in books)
            {
                var jSonSerializeRezult = JsonConvert.SerializeObject(book);
                
                File.AppendAllText("Books.json", jSonSerializeRezult + Environment.NewLine);
            }

            using (var writer = new System.IO.StreamWriter("Books.xml"))
            {
                var serializer = new XmlSerializer(books.GetType());
                serializer.Serialize(writer, books);
                writer.Flush();
            }
        }
    }
}
