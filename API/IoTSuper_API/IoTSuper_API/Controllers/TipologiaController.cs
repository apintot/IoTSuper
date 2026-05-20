using IoTSuper_API.Data;
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

        public TipologiaController(AppDBContext context, IContrasenaService contrasenaService)
        {
            _context = context;
            _contrasenaService = contrasenaService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTipologias()
        {
            try
            {
                Dictionary<int, string> tipologias = await _context.Tipologias.ToDictionaryAsync(t => t.IdTipologia, t => t.TipoTienda);

                if (tipologias == null || tipologias.Count == 0)
                {
                    return NotFound("No se encontraron tipologías.");
                }

                return Ok(tipologias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al obtener las tipologías.");
            }
        }
    }
}
