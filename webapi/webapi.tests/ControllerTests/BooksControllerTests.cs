using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using webapi.Controllers;
using webapi.core.DomainModels;
using webapi.core.Interfaces;
using webapi.core.Services;

namespace webapi.tests.ControllerTests
{
    [TestFixture]
    public class BooksControllerTests
    {
        [Test]
        public void GetAllBooksTest() 
        {
            var books = new [] { new Book("War and Peace", 10.0M)};

            var mockRepo = Mock.Of<IBookRepository>(r => r.Get() == books);

            var controller = new BooksController(new BookService(mockRepo));

            var response = controller.Get();

            var result = (Book[])response.Value;

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("War and Peace", result[0].Title);
            Assert.AreEqual(10.0M, result[0].Price);
        }

        [Test]
        public void GetBookByIdTest() 
        {
            var book = new Book("War and Peace", 10.0M);

            var mockRepo = Mock.Of<IBookRepository>(r => r.GetById(1) == book);

            var controller = new BooksController(new BookService(mockRepo));

            var response = controller.GetById(1);

            var result = (Book)response.Value;

            Assert.AreEqual("War and Peace", result.Title);
            Assert.AreEqual(10.0M, result.Price);
        }
    }
}