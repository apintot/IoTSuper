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
        private readonly ILogService _logger;

        public ComponentesController(IComponenteService componenteService, ILogService logger)
        {
            _componenteService = componenteService;
            _logger = logger;
        }

        [HttpGet("{idSeccion}")]
        public async Task<ActionResult> GetComponentes(int idSeccion)
        {
            try
            {
                await _logger.LogAsync($"Se obtuvo la lista de componentes de la sección {idSeccion}.");

                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al obtener la lista de componentes de la sección {idSeccion}: {ModelState}.");
                    return BadRequest(ModelState);
                }

                List<ComponenteDTO> respuesta = await _componenteService.GetComponentesAsync(idSeccion);
                await _logger.LogAsync($"Se obtuvo la lista de componentes de la sección {idSeccion} correctamente.");
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al obtener la lista de componentes de la sección {idSeccion}: {ex.Message}.");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al obtener los componentes." } } } });
            }
        }

        [HttpGet("Componente/{id}")]
        public async Task<ActionResult> GetComponente(int id)
        {
            try
            {
                await _logger.LogAsync($"Se obtuvo el componente con id {id}.");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al obtener el componente con id {id}: {ModelState}.");
                    return BadRequest(ModelState);
                }

                ComponenteDTO respuesta = await _componenteService.GetComponenteAsync(id);

                await _logger.LogAsync($"Se obtuvo el componente con id {id} correctamente.");

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al obtener el componente con id {id}: {ex.Message}.");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al obtener los componentes." } } } });
            }
        }

        [HttpPut]
        public async Task<ActionResult> ActualizarComponente(ComponenteDTO componenteDTO)
        {
            try
            {
                await _logger.LogAsync($"Se actualizó el componente con id {componenteDTO.IdComponente}.");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al actualizar el componente con id {componenteDTO.IdComponente}: {ModelState}.");
                    return BadRequest(ModelState);
                }
                await _componenteService.ActualizarComponenteAsync(componenteDTO);
                await _logger.LogAsync($"Se actualizó el componente con id {componenteDTO.IdComponente} correctamente.");
                return Ok(new ErrorDTO() { Status = 200, Title = "Componente actualizado correctamente" });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al actualizar el componente con id {componenteDTO.IdComponente}: {ex.Message}.");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al actualizar el componente." } } } });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CrearComponente(ComponenteDTO componenteDTO)
        {
            try
            {
                await _logger.LogAsync($"Se creó un nuevo componente con nombre {componenteDTO.Nombre}.");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al crear un nuevo componente con nombre {componenteDTO.Nombre}: {ModelState}.");
                    return BadRequest(ModelState);
                }
                int id = await _componenteService.CrearComponenteAsync(componenteDTO);
                await _logger.LogAsync($"Se creó un nuevo componente con nombre {componenteDTO.Nombre} correctamente.");
                return Ok(new ErrorDTO() { Errors = new Dictionary<string, List<string>> { { "Id", new List<string> { id.ToString() } } } });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al crear un nuevo componente con nombre {componenteDTO.Nombre}: {ex.Message}.");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al crear el componente." } } } });
            }
        }

        [HttpPost("{topic}")]
        public async Task<ActionResult> SumarUnoVisualizacion(string topic)
        {
            try
            {
                await _logger.LogAsync($"Se sumó una visualización al componente con topic {topic}.");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al sumar una visualización al componente con topic {topic}: {ModelState}.");
                    return BadRequest(ModelState);
                }

                await _componenteService.SumarUnoVisualizacionAsync(topic);
                await _logger.LogAsync($"Se sumó una visualización al componente con topic {topic} correctamente.");
                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al sumar una visualización al componente con topic {topic}: {ex.Message}.");
                return Ok();
            }
        }

        [HttpDelete("{idComponente}")]
        public async Task<ActionResult> EliminarComponente(int idComponente)
        {
            try
            {
                await _logger.LogAsync($"Se eliminó el componente con id {idComponente}.");
                if (!ModelState.IsValid)
                {
                    await _logger.LogAsync($"Error al eliminar el componente con id {idComponente}: {ModelState}.");    
                    return BadRequest(ModelState);
                }
                await _componenteService.EliminarComponenteAsync(idComponente);
                await _logger.LogAsync($"Se eliminó el componente con id {idComponente} correctamente.");
                return Ok(new ErrorDTO() { Status = 200, Title = "Componente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Ocurrió un error al eliminar el componente con id {idComponente}: {ex.Message}.");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Ocurrió un error al eliminar el componente." } } } });
            }
        }
    }
}