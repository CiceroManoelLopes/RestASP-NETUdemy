using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestASP_NETUdemy.Business;
using RestASP_NETUdemy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")] //Adicionado o termo "api/" após alterar o Launch //Adiciona a versão da API
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;
        private IBooksBusiness _booksBusiness; 

        public BooksController(ILogger<BooksController> logger, IBooksBusiness booksBusiness)
        {
            _logger = logger;
            _booksBusiness = booksBusiness; //Setou no serviço no controller com o que veio


        }

        [HttpGet]
        public IActionResult GetFindAll()
        {
            return Ok(_booksBusiness.FindAll());
        }

        [HttpGet("{id}")] //Passar o id que será localizado como parametro na URL e não no Body
        //Os GETs podem ter o mesmo nome desde que para retirar ambiguidade colocar o Parametro "id" no  [HttpGet] com  [HttpGet("id")]
        public IActionResult GetFindId(long id)
        {
            var books = _booksBusiness.FindByID(id);
            if (books == null)
            {
                return NotFound();
            }
            return Ok(books);
        }

        [HttpPost]
        //Creat
        public IActionResult Post([FromBody] Books book)
        {
            if (book == null)
            {
                return BadRequest();
            }
            //return Ok();
            return Ok(_booksBusiness.Create(book));
        }

        [HttpPut]
        //Update
        public IActionResult Put([FromBody] Books book) //Para transformar o Body que veio na requisião em uma classe pessoa
        {
            if (book == null)
            {
                return BadRequest();
            }
            //return Ok();
            return Ok(_booksBusiness.Update(book));
        }

        [HttpDelete("{id}")] //Passar o id que será deletado como parametro na URL e não no Body
        //Delete
        public IActionResult Delete(long id)
        {
            _booksBusiness.Delete(id);
            return NoContent();
        }
    }
}
