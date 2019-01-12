using webapi.core.DomainModels;

namespace webapi.core.Interfaces
{
    public interface IBookRepository
    {
        Book[] Get();
        Book GetById(object id);
        object Add(Book book);
    }
}