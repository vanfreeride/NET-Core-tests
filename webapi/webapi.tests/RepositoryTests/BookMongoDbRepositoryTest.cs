using NUnit.Framework;
using webapi.core.DomainModels;
using webapi.core.Interfaces;
using webapi.infrastructure.Repositories;

namespace webapi.tests.RepositoryTests
{
    [TestFixture]
    public class BookMongoDbRepositoryTest
    {
        [Test]
        public void AddAndGetBookTest()
        {
            IBookRepository repo = new BookMongoDbRepository();

            var book = new Book("Незнайка", 100500M, "Desc");

            var id = repo.Add(book);

            var result = repo.GetById(id);

            Assert.AreEqual(book.Title, result.Title);
            Assert.AreEqual(book.Price, result.Price);
            Assert.AreEqual(book.Description, result.Description);
        }
    }
}