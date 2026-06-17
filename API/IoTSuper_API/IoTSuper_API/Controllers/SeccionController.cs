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
                    return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Sección no encontrada" } } } });
                }

                return Ok(secciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Sección no encontrada" } } } });
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
                return Ok(new ErrorDTO() { Title="Seccion creada correctamente"});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear la seccion" } } } });
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
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar la seccion" } } } });
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
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar seccion" } } } });
            }
        }
    }
}
