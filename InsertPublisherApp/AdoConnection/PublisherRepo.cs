using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Entities;

namespace AdoConnection
{
    public class PublisherRepo
    {
        protected readonly SqlConnection connection;
        public PublisherRepo(SqlConnection conn)
        {
            connection = conn;
        }
        public int InsertPublisher(Publisher pub)
        {
            const string query = @"insert into Publisher (PublisherId, Name) values (@pubId, @name); select cast(scope_identity() as int);";
            SqlParameter pubId = new SqlParameter("@pubId", System.Data.SqlDbType.Int)
            {
                Value = pub.Id
            };
            SqlParameter name = new SqlParameter("@name", System.Data.DbType.String)
            {
                Value = pub.PublisherName
            };
            
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            command.Parameters.Add(pubId);
            command.Parameters.Add(name);

            return (int)command.ExecuteScalar();
        }

        public int NumberOfPublisherRows()
        {
            const string query = @"select count(*) from Publisher;";
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            return (int)command.ExecuteScalar();
        }

        public List<Publisher> TopTenPublisher()
        {
            List<Publisher> publishers = new List<Publisher>();
            const string query = @"select top 10 PublisherId, Name from Publisher order by PublisherId";
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            var reader = command.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    var publisherId = (int)reader["PublisherId"];
                    var name = (string)reader["Name"];

                    publishers.Add(new Publisher { Id = publisherId, PublisherName = name });
                }
            }
            else
            {
                throw new InvalidOperationException("There are no rows");
            }

            reader.Close();

            return publishers;
        }

        public void Print(List<Publisher> publishers)  
        {
            foreach (var publisher in publishers)
            {
                Console.WriteLine($"PublisherId = {publisher.Id}, PublisherName = {publisher.PublisherName}");
            }
        }

        public List<Pubs> PublisherWithNumberOfBooks()
        {
            List<Pubs> pubs = new List<Pubs>();
            const string query = @"select Name, COUNT(p.PublisherId) as cnt from Publisher p inner join Book b on b.PublisherId = p.PublisherId group by p.Name;";

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            var reader = command.ExecuteReader();

            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    var publisherName = (string)reader["Name"];
                    var count = (int)reader["cnt"];

                    pubs.Add(new Pubs{ Name = publisherName, BookCount = count });
                }
            }
            else
            {
                throw new InvalidOperationException("There are no rows");
            }
            reader.Close();

            return pubs;
        }

        public class Pubs
        {
            internal string Name { get; set; }
            internal int BookCount { get; set; }
        }

        public void Print(List<Pubs> publishers)
        {
            foreach (var publisher in publishers)
            {
                Console.WriteLine($"PublisherName = {publisher.Name}, BookCount = {publisher.BookCount}");
            }
        }

        public List<PubsPrice> PublisherWithBooksPrice()
        {
            List<PubsPrice> pubs = new List<PubsPrice>();
            const string query = @"select Name, Sum(b.Price) as Price from Publisher p inner join Book b on b.PublisherId = p.PublisherId group by p.Name;";

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var publisherName = (string)reader["Name"];
                    var price = (decimal)reader["Price"];

                    pubs.Add(new PubsPrice { Name = publisherName, BooksPrice = price });
                }
            }
            else
            {
                throw new InvalidOperationException("There are no rows");
            }
            reader.Close();

            return pubs;
        }

        public class PubsPrice
        {
            public string Name { get; set; }
            public decimal BooksPrice { get; set; }
        }

        public void Print(List<PubsPrice> publishers)
        {
            foreach (var publisher in publishers)
            {
                Console.WriteLine($"PublisherName = {publisher.Name}, BooksPrice = {publisher.BooksPrice}");
            }
        }

        public void SelectPublisherToDelete()
        {
            const string query = @"DeletePublisher";
            SqlParameter publisherId = new SqlParameter("@id", System.Data.SqlDbType.Int)
            {
                Value = GetId("Write publisher id to delete: ")
            };

            var command = new SqlCommand
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandText = query,
                Connection = connection,
            };
            var comm = new SqlCommand
            {
                CommandText = @"select count(PublisherId) from Publisher;",
                Connection = connection
            };

            command.Parameters.Add(publisherId);

            if ((int)publisherId.Value < (int)comm.ExecuteScalar())
            {
                command.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("There is not suck an id in books collection.");
            }
        }
        public static int GetId(string str)
        {
            int id;
            while (true)
            {
                Console.WriteLine(str);
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input id!");
                }
            }
            return id;
        }
    }
}
