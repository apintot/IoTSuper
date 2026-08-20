using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Seccion;
using IoTSuper_API.Models;
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
        private readonly ILogService _logger;
        public SeccionController(ISeccionService seccionService)
        {
            _seccionService = seccionService;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetSecciones(int Id)
        {
            try
            {
                await _logger.LogAsync($"Obteniendo secciones para el centro con Id: {Id}");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error en la validación del modelo: {ModelState}");
                    return BadRequest(ModelState);
                }

                List<SeccionDTO> secciones = await _seccionService.ObtenerSeccionesAsync(Id);

                if (secciones == null || secciones.Count == 0)
                {
                    await _logger.LogAsync($"No se encontraron secciones para el centro con Id: {Id}");
                    return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Sección no encontrada" } } } });
                }
                await _logger.LogAsync($"Secciones obtenidas exitosamente para el centro con Id: {Id}");
                return Ok(secciones);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al obtener secciones para el centro con Id: {Id}. Excepción: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Sección no encontrada" } } } });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearSeccion([FromBody] SeccionDTO seccionDTO)
        {
            try
            {
                await _logger.LogAsync($"Creando nueva sección con nombre: {seccionDTO.Nombre}");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error en la validación del modelo: {ModelState}");
                    return BadRequest(ModelState);
                }
                int idSeccion = await _seccionService.CrearSeccionAsync(seccionDTO);
                await _logger.LogAsync($"Sección creada exitosamente con Id: {idSeccion}");
                return Ok(new ErrorDTO() { Status = 200, Errors = new Dictionary<string, List<string>> { { "IdSeccion", new List<string> { idSeccion.ToString() } } } });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al crear la sección con nombre: {seccionDTO.Nombre}. Excepción: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear la seccion" } } } });
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarSeccion(SeccionDTO seccionDTO)
        {
            try
            {
                await _logger.LogAsync($"Actualizando sección con Id: {seccionDTO.IdSeccion}");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error en la validación del modelo: {ModelState}");
                    return BadRequest(ModelState);
                }
                await _seccionService.ActualizarSeccionAsync(seccionDTO);
                await _logger.LogAsync($"Sección actualizada exitosamente con Id: {seccionDTO.IdSeccion}");
                return Ok(new ErrorDTO());
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al actualizar la sección con Id: {seccionDTO.IdSeccion}. Excepción: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar la seccion" } } } });
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> EliminarSeccion(int Id)
        {
            try
            {
                await _logger.LogAsync($"Eliminando sección con Id: {Id}");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error en la validación del modelo: {ModelState}");
                    return BadRequest(ModelState);
                }
                await _seccionService.EliminarSeccionAsync(Id);
                await _logger.LogAsync($"Sección eliminada exitosamente con Id: {Id}");
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al eliminar la sección con Id: {Id}. Excepción: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar seccion" } } } });
            }
        }
    }
}
