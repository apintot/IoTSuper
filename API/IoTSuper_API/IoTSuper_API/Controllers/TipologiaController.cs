using IoTSuper_API.Data;
using IoTSuper_API.DTO;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class TipologiaController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IContrasenaService _contrasenaService;
        private readonly ILogService _logger;

        public TipologiaController(AppDBContext context, IContrasenaService contrasenaService, ILogService logger)
        {
            _context = context;
            _contrasenaService = contrasenaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetTipologias()
        {
            try
            {
                await _logger.LogAsync("Obteniendo tipologias de la base de datos.");
                Dictionary<int, string> tipologias = await _context.Tipologias.ToDictionaryAsync(t => t.IdTipologia, t => t.TipoTienda);

                if (tipologias == null || tipologias.Count == 0)
                {
                    await _logger.LogAsync("No se encontraron tipologías en la base de datos.");
                    return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "No se encontraron tipologías." } } } });
                    
                }

                await _logger.LogAsync($"Se encontraron {tipologias.Count} tipologías en la base de datos.");
                return Ok(tipologias);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al obtener las tipologías: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al obtener las tipologias." } } } });
            }
        }
    }
}
