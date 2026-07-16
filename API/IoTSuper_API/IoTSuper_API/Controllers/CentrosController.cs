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

        public CentrosController(ICentroService centroService)
        {
            _centroService = centroService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCentros(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<CentroDTO> centros = await _centroService.ObtenerCentrosAsync(id);

                if (centros == null || centros.Count == 0)
                {
                    return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al encontrar centro" } } } });
                }

                return Ok(centros);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al obtener el centro" } } } });
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

                int idCentro = await _centroService.CrearCentroAsync(centroDTO);
                return Ok(new ErrorDTO() { Status = 200, Errors = new Dictionary<string, List<string>> { { "IdCentro", new List<string> { idCentro.ToString() } } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear centro" } } } });
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
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
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
                    return BadRequest(ModelState);
                }

                await _centroService.EliminarCentroAsync(id);
                return Ok(new ErrorDTO() { Status = 200 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar centro" } } } });
            }
        }
    }
}
