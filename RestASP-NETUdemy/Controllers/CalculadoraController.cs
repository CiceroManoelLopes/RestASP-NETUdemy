using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestASP_NETUdemy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculadoraController : ControllerBase
    {
        private readonly ILogger<CalculadoraController> _logger;

        public CalculadoraController(ILogger<CalculadoraController> logger)
        {
            _logger = logger;
        }

        [HttpGet("soma/{primeiroNumero}/{segundoNumero}")]
        public IActionResult Get(string primeiroNumero, string segundoNumero)
        {
            if(EhNumerico(primeiroNumero) && EhNumerico(segundoNumero))
            {
                var sum = ConverterParaDecimal(primeiroNumero) + ConverterParaDecimal(segundoNumero);
                return Ok(sum.ToString());
            }
            return BadRequest("Numero Incorreto.");
        }


        private bool EhNumerico(string strNumero)
        {
            double numero;
            bool ehNumero = double.TryParse(
                strNumero, 
                System.Globalization.NumberStyles.Any, 
                System.Globalization.NumberFormatInfo.InvariantInfo, 
                out numero);

            return ehNumero;  
        }
        private decimal ConverterParaDecimal(string strNumero)
        {
            decimal valor;
            if(decimal.TryParse(strNumero, out valor))
            {
                return valor;
            }
            return 0;
        }       
    }
}
