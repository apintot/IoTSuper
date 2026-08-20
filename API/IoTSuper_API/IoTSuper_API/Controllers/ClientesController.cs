using IoTSuper_API.Data;
using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Cliente;
using IoTSuper_API.Models;
using IoTSuper_API.Security;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDBContext _context;

        private readonly IContrasenaService _contrasenaService;
        private readonly ILogService _logger;

        private readonly Crypto _crypto;
        public ClientesController(AppDBContext context, IContrasenaService contrasenaService, Crypto crypto, ILogService logger)
        {
            _context = context;
            _contrasenaService = contrasenaService;
            _crypto = crypto;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> obtenerTodosLosClientes()
        {
            await _logger.LogAsync("Obteniendo todos los clientes habilitados y no administradores.");
            List<Cliente> clientes = await _context.Clientes.Where(c => c.Habilitado && !c.EsAdmin).ToListAsync();

            if(clientes == null || clientes.Count == 0)
            {
                await _logger.LogAsync("No se encontraron clientes habilitados y no administradores.");
                return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Cliente no encontrado" } } } });
            }

            List<ClienteResponse> clientesResponse = clientes.Select(c => new ClienteResponse
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Empresa = c.Empresa,
                EsAdmin = c.EsAdmin,
                Habilitado = c.Habilitado,
                Login = c.Login
            }).ToList();

            await _logger.LogAsync($"Se encontraron {clientesResponse.Count} clientes habilitados y no administradores.");

            return Ok(clientesResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> obtenerCliente(int id)
        {
            await _logger.LogAsync($"Obteniendo cliente con ID: {id}.");
            Cliente? cliente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (cliente == null)
            {
                await _logger.LogAsync($"Cliente con ID: {id} no encontrado.");
                return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Cliente no encontrado" } } } });
            }

            ClienteResponse clienteResponse = new ClienteResponse
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Habilitado = cliente.Habilitado,
                EsAdmin = cliente.EsAdmin,
                Empresa = cliente.Empresa,
                Login = cliente.Login,
                UltimoAcceso = (DateTime)cliente.UltimoAcceso
            };

            return Ok(clienteResponse);
        }

        [HttpPost]
        public async Task<ActionResult> crearCliente([FromBody] NuevoClienteRequest cliente)
        {
            if (!ModelState.IsValid)
            {
                await _logger.LogAsync("Datos de cliente inválidos.");
                return ValidationProblem(ModelState);
            }

            if (await _context.Clientes.AnyAsync(c => c.Login == cliente.Login))
            {
                await _logger.LogAsync($"Cliente con login {cliente.Login} ya existe.");
                return BadRequest(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error cliente ya existe" } } } });
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre)) { await _logger.LogAsync("El nombre del cliente es obligatorio."); return BadRequest(new { mensaje = "El nombre es obligatorio." }); }

            if (!_contrasenaService.EsContrasenaSegura(cliente.Contrasena))
            {
                await _logger.LogAsync("La contraseña del cliente no es segura.");
                return BadRequest(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error contrasena" } } } });
            }

            Cliente nuevoCliente = new Cliente
            {
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Habilitado = true,
                EsAdmin = false,
                Empresa = cliente.Empresa,
                Login = cliente.Login,
                Contrasena = _contrasenaService.hashContrasena(cliente.Contrasena)
            };

            try
            {
                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();
                await _logger.LogAsync($"Cliente con login {cliente.Login} creado exitosamente.");
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al crear cliente con login {cliente.Login}: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear cliente" } } } });
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> actualizarCliente(int id, [FromBody] ActualizarClienteRequest cliente)
        {
            await   _logger.LogAsync($"Actualizando cliente con ID: {id}.");
            if (!ModelState.IsValid)
            {
                await _logger.LogAsync("Datos de cliente inválidos.");
                return ValidationProblem(ModelState);
            }

            Cliente? clienteExistente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                await _logger.LogAsync($"Cliente con ID: {id} no encontrado.");
                return NotFound();
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Empresa = cliente.Empresa;
            clienteExistente.Login = cliente.Login;
            clienteExistente.Contrasena = cliente.Contrasena;

            if (!string.IsNullOrWhiteSpace(clienteExistente.Contrasena))
            {
                if (!_contrasenaService.EsContrasenaSegura(clienteExistente.Contrasena))
                {
                    await _logger.LogAsync("La contraseña del cliente no es segura.");
                    return BadRequest(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar cliente" } } } });
                }

                clienteExistente.Contrasena = _contrasenaService.hashContrasena(clienteExistente.Contrasena);
            }

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
                await _logger.LogAsync($"Cliente con ID: {id} actualizado exitosamente.");
            }
            catch (Exception ex) {await _logger.LogAsync($"Error al actualizar cliente con ID: {id}: {ex.Message}"); return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar cliente" } } } }); }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> eliminarCliente(int id)
        {
            await _logger.LogAsync($"Eliminando cliente con ID: {id}.");

            Cliente? clienteExistente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                await _logger.LogAsync($"Cliente con ID: {id} no encontrado.");
                return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Cliente no encontrado" } } } });
            }

            clienteExistente.Habilitado = false;

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
                await _logger.LogAsync($"Cliente con ID: {id} deshabilitado exitosamente.");

                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync($"Error al eliminar cliente con ID: {id}: {ex.Message}");
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar cliente" } } } });
            }
        }
    }
}
