using System.Linq;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using webapi.core.DomainModels;
using webapi.core.Interfaces;
using webapi.infrastructure.DbObjects;
using System;

namespace webapi.infrastructure.Repositories
{
    public class BookMongoDbRepository : IBookRepository
    {
        private readonly IMongoCollection<BookRowMongo> books;

        public BookMongoDbRepository(IConfiguration config = null)
        {
            var connString = config?.GetConnectionString("MyMongoDb");

            var client = new MongoClient(connString);
            var database = client.GetDatabase("webapimongodb");
            books = database.GetCollection<BookRowMongo>("Books");
        }

        public object Add(Book book)
        {
            var row = new BookRowMongo 
            { 
                Title = book.Title, 
                Price = book.Price, 
                Description = book.Description ,
                Dic = new System.Collections.Generic.Dictionary<string, string> {{"ssd", "sdsdsds"}, {"ssd2", "sdsdsds2"}},
                Guid = Guid.NewGuid()
            };

            books.InsertOne(row);

            return row.Id;
        }

        public Book[] Get()
        {
            var bks = books.Find<BookRowMongo>(b => true).ToList();

            return bks
                    .Select(row => new Book(row.Title, row.Price, row.Description)).ToArray();
        }

        public Book GetById(object oid)
        {
            var id = (ObjectId)oid;

            var row = books.Find<BookRowMongo>(b => b.Id == id).FirstOrDefault();

            return new Book(row.Title, row.Price, row.Description);
        }
    }
}