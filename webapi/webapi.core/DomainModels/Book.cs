namespace webapi.core.DomainModels
{
    public class Book
    {
        public Book(string title, decimal price, string description)
        {
            Price = price;
            Title = title;
            Description = description;
        }

        public decimal Price { get; }
        public string Title { get; }
        public string Description {get; }
    }
}