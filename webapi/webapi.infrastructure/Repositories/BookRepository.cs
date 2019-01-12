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

        public object Add(Book book)
        {
            var bookRow = new BookRow { Title = book.Title, Price = book.Price};

            db.Books.Add(bookRow);

            db.SaveChanges();

            return bookRow.Id;
        }

        public Book[] Get()
        {
            return db.Books.Select(b => new Book(b.Title, b.Price, null)).ToArray();
        }

        public Book GetById(object oid)
        {
            var id = (int)oid;

            return db.Books.Where(b => b.Id == id)
                           .Select(b => new Book(b.Title, b.Price, null))
                           .FirstOrDefault();
        }
    }
}