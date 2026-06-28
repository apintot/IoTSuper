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

        public EventoController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        [HttpPost]
        public async Task<ActionResult> CrearEvento(EventoDTO eventoDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _eventoService.CrearEventoAsync(eventoDTO);
                await _eventoService.EnviarCorreoAsync(eventoDTO);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió al enviar el email." } } } });
            }
        }
    }
}
