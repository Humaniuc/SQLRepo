using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace AdoConnection
{
    public class BookRepo
    {
        protected readonly SqlConnection connection;

        public BookRepo(SqlConnection conn)
        {
            connection = conn;
        }

        public int InsertBook(Book book)
        {
            const string query = @"insert into Book (Title, PublisherId, Year, Price) values (@title, @publisherId, @year, @price); select cast(scope_identity() as int);";
            SqlParameter bookTitle = new SqlParameter("@title", System.Data.DbType.String)
            {
                Value = book.Title
            };
            SqlParameter bookPubId = new SqlParameter("@publisherId", System.Data.SqlDbType.Int)
            {
                Value = book.PublisherId
            };
            SqlParameter bookYear = new SqlParameter("@year", System.Data.SqlDbType.Int)
            {
                Value = book.Year
            };
            SqlParameter bookPrice = new SqlParameter("@price", System.Data.SqlDbType.Decimal)
            {
                Value = book.Price
            };

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            command.Parameters.Add(bookTitle);
            command.Parameters.Add(bookPubId);
            command.Parameters.Add(bookYear);
            command.Parameters.Add(bookPrice);

            return (int)command.ExecuteScalar();
        }

        public void UpdateBook()
        {
            const string query = @"update Book set Price = @price where BookId = @id;";

            SqlParameter bookId = new SqlParameter("@id", System.Data.SqlDbType.Int)
            {
                Value = GetId("Write id for book to update: ")
            };

            SqlParameter bookPrice = new SqlParameter("@price", System.Data.SqlDbType.Decimal)
            {
                Value = GetPrice()
            };
            
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            var comm = new SqlCommand
            {
                CommandText = @"select count(BookId) from Book;",
                Connection = connection
            };

            command.Parameters.Add(bookPrice);
            command.Parameters.Add(bookId);

            if((int)bookId.Value < (int)comm.ExecuteScalar())
            {
                command.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("There is not suck an id in books collection.");
            }    
        }

        public static decimal GetPrice()
        {
            decimal price;
            while (true)
            {
                Console.WriteLine("Write book price to update: ");
                if (decimal.TryParse(Console.ReadLine(), out price))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input price!");
                }
            }
            return price;
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
        public void DeleteBook()
        {
            const string query = @"delete from Book where BookId = @id";
            SqlParameter bookId = new SqlParameter("@id", System.Data.SqlDbType.Int)
            {
                Value = GetId("Write book id to delete: ")
            };

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            var comm = new SqlCommand
            {
                CommandText = @"select count(BookId) from Book;",
                Connection = connection
            };

            command.Parameters.Add(bookId);

            if ((int)bookId.Value < (int)comm.ExecuteScalar())
            {
                command.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("There is not suck an id in books collection.");
            }
        }

        public Book SelectBook()
        {
            Book book = new Book();
            const string query = @"select * from Book where BookId = @id;";
            SqlParameter bookId = new SqlParameter("@id", System.Data.SqlDbType.Int)
            {
                Value = GetId("Write book id to select: ")
            };

            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };
            var comm = new SqlCommand
            {
                CommandText = @"select count(BookId) from Book;",
                Connection = connection
            };

            command.Parameters.Add(bookId);

            if ((int)bookId.Value < (int)comm.ExecuteScalar())
            {
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if(rdr.Read())
                    {
                        book.Id = (int)rdr["BookId"];
                        book.Title = (string)rdr["Title"];
                        book.PublisherId = (int)rdr["PublisherId"];
                        book.Year = (int)rdr["Year"];
                        book.Price = (decimal)rdr["Price"];
                    }
                }
                return book;
            }
            else
            {
                throw new Exception("There is not suck an id in books collection.");
            }
        }

        public List<string> SelectAllBooksPublishedIn2010()
        {
            List<string> books = new List<string>();

            const string query = @"select Title from Book where Year = 2010 order by Title;";
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var title = (string)reader["Title"];

                        books.Add(title);
                    }
                    return books;
                }
                else
                {
                    throw new InvalidOperationException("There are no rows");
                }
            }
        }

        public void Print(List<string> ls)
        {
            foreach(string s in ls)
            {
                Console.WriteLine(s);
            }
        }

        public List<string> SelectBooksPublishedInMaxYear()
        {
            List<string> books = new List<string>();

            const string query = @"select Title from Book where year = (select max(Year) from Book) order by Title;";
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var title = (string)reader["Title"];

                        books.Add(title);
                    }
                    return books;
                }
                else
                {
                    throw new InvalidOperationException("There are no rows");
                }
            }
        }

        public List<object> TopTenPublisher()
        {
            List<object> books = new List<object>();
            const string query = @"select top 10 Title, Year, Price from Book order by Title";
            var command = new SqlCommand
            {
                CommandText = query,
                Connection = connection
            };

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var title = (string)reader["Title"];
                        var year = (int)reader["Year"];
                        var price = (decimal)reader["Price"];

                        books.Add(new Book{ Title = title, Year = year, Price = price });
                    }
                    return books;
                }
                else
                {
                    throw new InvalidOperationException("There are no rows");
                }
            }
        }

        public void PrintTopTen(List<object> lsObj)
        {
            foreach(var obj in lsObj)
            {
                Console.WriteLine($"{(obj as Book).Title}, Year: {(obj as Book).Year}, Price: {(obj as Book).Price}");
            }
        }
    }
}
