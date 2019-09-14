using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

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

        public decimal Price { get;set; }

        public string Title { get; set;}

        public string Description { get; set;}

        public Dictionary<string, string> Dic { get; set; }
        public Guid Guid { get; set; }
    }
}