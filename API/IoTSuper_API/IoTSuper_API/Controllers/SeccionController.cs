using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Seccion;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class SeccionController : Controller
    {
        private readonly ISeccionService _seccionService;

        public SeccionController(ISeccionService seccionService)
        {
            _seccionService = seccionService;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetSecciones(int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<SeccionDTO> secciones = await _seccionService.ObtenerSeccionesAsync(Id);

                if (secciones == null || secciones.Count == 0)
                {
                    return NotFound("Sección no encontrada");
                }

                return Ok(secciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al obtener las secciones.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearSeccion([FromBody] SeccionDTO seccionDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _seccionService.CrearSeccionAsync(seccionDTO);
                return Ok(new ErrorDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al crear la sección.");
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarSeccion(SeccionDTO seccionDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _seccionService.ActualizarSeccionAsync(seccionDTO);
                return Ok(new ErrorDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al actualizar la sección.");
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> EliminarSeccion(int Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _seccionService.EliminarSeccionAsync(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al eliminar la sección.");
            }
        }
    }
}
