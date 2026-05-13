using IoTSuper_API.DTO.Centro;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class CentrosController : Controller
    {
        private readonly ICentroService _centroService;

        public CentrosController(ICentroService centroService)
        {
            _centroService = centroService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCentros()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<CentroDTO> centros = await _centroService.ObtenerCentrosAsync();

                if (centros == null || centros.Count == 0)
                {
                    return NotFound("Centro no encontrado");
                }

                return Ok(centros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al obtener los centros.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearCentro([FromBody] CentroDTO centroDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _centroService.CrearCentroAsync(centroDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al crear el centro.");
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarCentro([FromBody] CentroDTO centroDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _centroService.ActualizarCentroAsync(centroDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al actualizar el centro.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCentro(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _centroService.EliminarCentroAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al eliminar el centro.");
            }
        }
    }
}
