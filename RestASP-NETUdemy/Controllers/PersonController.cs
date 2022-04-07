using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //Adicionado o termo "api/" após alterar o Launch 
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPessoaService _pessoaService;  //Declarou o serviço

        public PersonController(ILogger<PersonController> logger, IPessoaService pessoaService) //Adicionou o serviço como parametro que vai receber
        {
            _logger = logger;
            _pessoaService = pessoaService; //Setou no serviço no controller com o que veio
        }

        [HttpGet]
        public IActionResult GetFindAll()
        {
            return Ok(_pessoaService.FindAll());
        }

        [HttpGet("{id}")] //Passar o id que será localizado como parametro na URL e não no Body
        //Os GETs podem ter o mesmo nome desde que para retirar ambiguidade colocar o Parametro "id" no  [HttpGet] com  [HttpGet("id")]
        public IActionResult GetFindId(long id)
        {
            var pessoa = _pessoaService.FindByID(id);
            if(pessoa == null)
            {
                return NotFound();
            }
            return Ok(pessoa);
        }

        [HttpPost]
        //Creat
        public IActionResult Post([FromBody] Pessoa pessoa)
        {           
            if (pessoa == null)
            {
                return BadRequest();
            }
            //return Ok();
            return Ok(_pessoaService.Create(pessoa));
        }

        [HttpPut]
        //Update
        public IActionResult Put([FromBody] Pessoa pessoa) //Para transformar o Body que veio na requisião em uma classe pessoa
        {
            if (pessoa == null)
            {
                return BadRequest();
            }
            //return Ok();
            return Ok(_pessoaService.Update(pessoa));
        }

        [HttpDelete("{id}")] //Passar o id que será deletado como parametro na URL e não no Body
        //Delete
        public IActionResult Delete(long id)
        {
            _pessoaService.Delete(id);           
            return NoContent();
        }
    }
}
