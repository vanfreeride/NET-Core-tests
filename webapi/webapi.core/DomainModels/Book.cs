namespace webapi.core.DomainModels
{
    public class Book
    {
        public Book(string title, decimal price)
        {
            Price = price;
            Title = title;
        }

        public decimal Price { get; }
        public string Title { get; }
    }
}