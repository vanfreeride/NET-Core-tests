using System.ComponentModel.DataAnnotations;

namespace webapi.infrastructure.DbObjects
{
    public class BookRow
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get;set; }
        public string Title { get; set;}
    }
}