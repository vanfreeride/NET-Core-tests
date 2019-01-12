using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace webapi.infrastructure.DbObjects
{
    public class BookRow
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set;}
    }

    public class BookRowMongo
    {
        public ObjectId Id { get; set; }

        [BsonElement("Price")]
        public decimal Price { get;set; }

        [BsonElement("Title")]
        public string Title { get; set;}

        [BsonElement("Description")]
        public string Description { get; set;}
    }
}