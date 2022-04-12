using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestASP_NETUdemy.Model;
using RestASP_NETUdemy.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestASP_NETUdemy.Repository;
using Microsoft.AspNetCore.Authorization;

namespace RestASP_NETUdemy.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")] //Adicionado o termo "api/" após alterar o Launch //Adiciona a versão da API
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPessoaBusiness _pessoaBusiness;  //Declarou o serviço

        public PersonController(ILogger<PersonController> logger, IPessoaBusiness pessoaBusiness) //Adicionou o serviço como parametro que vai receber
        {
            _logger = logger;
            _pessoaBusiness = pessoaBusiness; //Setou no serviço no controller com o que veio
        }

        [HttpGet]
        //[ProducesResponseType((200), Type = typeof(List<Pessoa>))] //Personalizar o Swagger
        //[ProducesResponseType(204)]
        //[ProducesResponseType(204)]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(201)]
        public IActionResult GetFindAll()
        {
            return Ok(_pessoaBusiness.FindAll());
        }

        [HttpGet("{id}")] //Passar o id que será localizado como parametro na URL e não no Body
                          //Os GETs podem ter o mesmo nome desde que para retirar ambiguidade colocar o Parametro "id" no  [HttpGet] com  [HttpGet("id")]
        //[ProducesResponseType((200), Type = typeof(Pessoa))] //Personalizar o Swagger
        public IActionResult GetFindId(long id)
        {
            var pessoa = _pessoaBusiness.FindByID(id);
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
            return Ok(_pessoaBusiness.Create(pessoa));
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
            return Ok(_pessoaBusiness.Update(pessoa));
        }

        [HttpDelete("{id}")] //Passar o id que será deletado como parametro na URL e não no Body
        //Delete
        public IActionResult Delete(long id)
        {
            _pessoaBusiness.Delete(id);           
            return NoContent();
        }
    }
}
