using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using webapi.infrastructure.Db;

namespace webapi.tests.Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestMethod() {

            var foo = new {id = 2, value = "vavava"};
            var bar = new {id = 2, value = "vavava"};

            Assert.AreEqual(foo, bar);
        }

        [Test]
        public void TestMethod2() {

            var foo = new Bla{id = 2, value = "vavava"};
            var bar = new Bla{id = 2, value = "vavava"};

            Assert.AreEqual(foo, bar);
        }

        [Test]
        public void TestMethod3() {

            var builder = new DbContextOptionsBuilder<MyDb>();

            builder.UseMySql("server=127.0.0.1;port=3306;UserId=root;Password=mypassword;database=webapidb;");
            var db = new MyDb(builder.Options);


            var result = new {Count = db.Books.Count(), Books = db.Books.ToList() };
            
        }
    }

    public class Bla{
        public int id { get; set; }
        public string value { get; set; }
    }
}