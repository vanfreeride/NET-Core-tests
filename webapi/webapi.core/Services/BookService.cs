using System;
using webapi.core.DomainModels;
using webapi.core.Interfaces;

namespace webapi.core.Services
{
    public class BookService
    {
        private IBookRepository repo;

        public BookService(IBookRepository repo)
        {
            this.repo = repo;
        }

        public Book[] Get()
        {
            var books = repo.Get();

            return books;
        }

        public void Add()
        {
            repo.Add(new Book("Added book", 666M));
        }

        public Book GetById(int id)
        {
            var books = repo.GetById(id);

            return books;
        }
    }
}