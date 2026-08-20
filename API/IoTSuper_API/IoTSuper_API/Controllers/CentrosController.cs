using IoTSuper_API.DTO;
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
        private readonly ILogService _logger;

        public CentrosController(ICentroService centroService, ILogService logger)
        {
            _centroService = centroService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCentros(int id)
        {
            try
            {
                await _logger.LogAsync($"Obteniendo centros para el cliente con ID: {id}");

                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Modelo inválido al obtener centros para el cliente con ID: {id}");
                    return BadRequest(ModelState);
                }

                List<CentroDTO> centros = await _centroService.ObtenerCentrosAsync(id);

                if (centros == null || centros.Count == 0)
                {
                    await _logger.LogAsync($"No se encontraron centros para el cliente con ID: {id}");
                    return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al encontrar centro" } } } });
                }

                return Ok(centros);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al obtener centros para el cliente con ID: {id} {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al obtener el centro" } } } });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearCentro([FromBody] CentroDTO centroDTO)
        {
            try
            {
                await _logger.LogAsync($"Creando un nuevo centro para el cliente con ID: {centroDTO.IdCliente}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int idCentro = await _centroService.CrearCentroAsync(centroDTO);

                await _logger.LogAsync($"Centro creado con éxito con ID: {idCentro} para el cliente con ID: {centroDTO.IdCliente}");

                return Ok(new ErrorDTO() { Status = 200, Errors = new Dictionary<string, List<string>> { { "IdCentro", new List<string> { idCentro.ToString() } } } });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al crear centro para el cliente con ID: {centroDTO.IdCliente} {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear centro" } } } });
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarCentro([FromBody] CentroDTO centroDTO)
        {
            try
            {
                await _logger.LogAsync($"Actualizando centro con ID: {centroDTO.IdCentro} para el cliente con ID: {centroDTO.IdCliente}"); 
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Modelo inválido al actualizar centro con ID: {centroDTO.IdCentro} para el cliente con ID: {centroDTO.IdCliente}");
                    return BadRequest(ModelState);
                }

                await _centroService.ActualizarCentroAsync(centroDTO);

                await _logger.LogAsync($"Centro con ID: {centroDTO.IdCentro} actualizado con éxito para el cliente con ID: {centroDTO.IdCliente}");
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al actualizar centro con ID: {centroDTO.IdCentro} para el cliente con ID: {centroDTO.IdCliente} {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar centro" } } } });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCentro(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Modelo inválido al eliminar centro con ID: {id}");
                    return BadRequest(ModelState);
                }

                await _centroService.EliminarCentroAsync(id);

                await _logger.LogAsync($"Centro con ID: {id} eliminado con éxito");
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al eliminar centro con ID: {id} {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar centro" } } } });
            }
        }
    }
}
