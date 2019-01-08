using System.Linq;
using webapi.core.DomainModels;
using webapi.core.Interfaces;
using webapi.infrastructure.Db;
using webapi.infrastructure.DbObjects;

namespace webapi.infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private MyDb db;

        public BookRepository(MyDb db)
        {
            this.db = db;
        }

        public void Add(Book book)
        {
            db.Books.Add(new BookRow { Title = book.Title, Price = book.Price});

            db.SaveChanges();
        }

        public Book[] Get()
        {
            return db.Books.Select(b => new Book(b.Title, b.Price)).ToArray();
        }

        public Book GetById(int id)
        {
            return db.Books.Where(b => b.Id == id)
                           .Select(b => new Book(b.Title, b.Price))
                           .FirstOrDefault();
        }
    }
}