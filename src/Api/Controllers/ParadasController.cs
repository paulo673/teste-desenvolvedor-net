﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Commons.Dtos;
using Services.Parada;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ParadasController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] ListarTodasAsParadas listarTodasAsParadas)
        {
            var paradas = await listarTodasAsParadas.Executar();

            return new ObjectResult(paradas);
        }

        [HttpGet]
        [Route("{id:long}")]
        public async Task<IActionResult> Get([FromServices] ObterParadaPorId obterParadaPorId, long id)
        {
            var parada = await obterParadaPorId.Executar(id);

            if (parada is null)
                return NotFound();

            return new ObjectResult(parada);
        }

        [HttpGet]
        [Route("{latitude:double}/{longitude:double}")]
        public async Task<IActionResult> Get([FromServices] ListarParadasMaisProximas listarParadasMaisProximas, double latitude, double longitude, int raioEmMetros = 1000)
        {
            var paradasMaisProximas = await listarParadasMaisProximas.Executar(latitude, longitude, raioEmMetros);

            return new ObjectResult(paradasMaisProximas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] CadastrarParada cadastrarParada, ParadaDto paradaDto)
        {
            if (ModelState.IsValid) {
                await cadastrarParada.Executar(paradaDto);

                return Ok();
            }

            return BadRequest(paradaDto);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromServices] EditarParada editarParada, ParadaDto paradaDto)
        {
            if (ModelState.IsValid) {
                await editarParada.Executar(paradaDto);

                if (editarParada.Notifications.Any()) {
                    return BadRequest(editarParada.Notifications);
                }

                return Ok();
            }

            return BadRequest(paradaDto);
        }

        [HttpDelete]
        [Route("{id:long}")]
        public async Task<IActionResult> Delete([FromServices] DeletarParada deletarParada, long id)
        {
            await deletarParada.Executar(id);

            if (deletarParada.Notifications.Any()) {
                return BadRequest(deletarParada.Notifications);
            }

            return Ok();
        }
    }
}