using System;
using System.Collections.Generic;
using Entities;
using Newtonsoft.Json;
using System.IO;
using System.Xml.Serialization;

namespace SerializeLists
{
    public class Serializer
    {
        public static void SerializeXML(List<Book> books)
        {
            using (var writer = new System.IO.StreamWriter("Books.xml"))
            {
                var serializer = new XmlSerializer(books.GetType());
                serializer.Serialize(writer, books);
                writer.Flush();
            }
        }

        public static void SerializeJSon(List<Book> books)
        {
            if (File.Exists("Books.json"))
            {
                File.Delete("Books.json");
            }
            foreach (Book book in books)
            {
                var jSonSerializeRezult = JsonConvert.SerializeObject(book);

                File.AppendAllText("Books.json", jSonSerializeRezult + Environment.NewLine);
            }
        }
    }
}
