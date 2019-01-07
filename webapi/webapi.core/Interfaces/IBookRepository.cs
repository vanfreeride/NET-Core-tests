using webapi.core.DomainModels;

namespace webapi.core.Interfaces
{
    public interface IBookRepository
    {
        Book[] Get();
        Book GetById(int id);
        void Add(Book book);
    }
}