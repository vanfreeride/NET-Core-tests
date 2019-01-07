using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi.core.Interfaces;
using webapi.core.Services;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private BookService serv;

        public BooksController(BookService serv)
        {
            this.serv = serv;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var result = serv.Get();

            return Json(result);
        }

        [HttpGet]
        [Route("add")]
        public JsonResult Add()
        {
            serv.Add();

            return Json(true);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult GetById(int id)
        {
            var result = serv.GetById(id);

            return Json(result);
        }
    }
}
