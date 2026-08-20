using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Componentes;
using IoTSuper_API.DTO.Evento;
using IoTSuper_API.Services;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class EventoController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ILogService _logger;
        public EventoController(IEventoService eventoService, ILogService logger)
        {
            _eventoService = eventoService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> CrearEvento(EventoDTO eventoDTO)
        {
            try
            {
                await _logger.LogAsync($"Creando evento para el componente {eventoDTO.IdComponente} con tipo {eventoDTO.TipoEvento} y fecha {eventoDTO.FechaEvento}");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error de validación: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }

                await _eventoService.CrearEventoAsync(eventoDTO);
                await _logger.LogAsync($"Evento creado exitosamente para el componente {eventoDTO.IdComponente}");
                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al crear evento: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { ex.Message } } } });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerEventos(int idUsuario)
        {
            try
            {
                await _logger.LogAsync($"Obteniendo eventos para el usuario {idUsuario}");
                List<EventoDTO> eventos = await _eventoService.ObtenerEventosAsync(idUsuario);
                await _logger.LogAsync($"Eventos obtenidos exitosamente para el usuario {idUsuario}");
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al obtener eventos: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { ex.Message } } } });
            }
        }
    }
}
