using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Controllers
{
    public class PessoaController : Controller
    {
        private readonly ILogger<PessoaController> _logger;

        public PessoaController(ILogger<PessoaController> logger)
        {
            _logger = logger;
        }


        [HttpGet("soma/{primeiroNumero}/{segundoNumero}")]
        public IActionResult Soma(string primeiroNumero, string segundoNumero)
        {
            return BadRequest("Numero Incorreto.");
        }
    }
}
