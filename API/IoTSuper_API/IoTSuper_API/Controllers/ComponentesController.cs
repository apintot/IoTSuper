using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Componentes;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class ComponentesController : Controller
    {
        private readonly IComponenteService _componenteService;

        public ComponentesController(IComponenteService componenteService)
        {
            _componenteService = componenteService;
        }

        [HttpGet("{idSeccion}")]
        public async Task<ActionResult> GetComponentes(int idSeccion)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<ComponenteDTO> respuesta = await _componenteService.GetComponentesAsync(idSeccion);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al obtener los componentes." } } } });
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarComponente(ComponenteDTO componenteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _componenteService.ActualizarComponenteAsync(componenteDTO);
                return Ok(new ErrorDTO() { Status = 200, Title = "Componente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al actualizar el componente." } } } });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearComponente(ComponenteDTO componenteDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                int id = await _componenteService.CrearComponenteAsync(componenteDTO);
                return Ok(new ErrorDTO() { Errors = new Dictionary<string, List<string>> { { "Id", new List<string> { id.ToString() } } } });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al crear el componente." } } } });
            }
        }

        [HttpDelete("{idComponente}")]
        public async Task<ActionResult> EliminarComponente(int idComponente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await _componenteService.EliminarComponenteAsync(idComponente);
                return Ok(new ErrorDTO() { Status = 200, Title = "Componente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al eliminar el componente." } } } });
            }
        }
    }
}