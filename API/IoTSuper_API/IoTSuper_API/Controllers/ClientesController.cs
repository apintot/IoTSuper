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
        private readonly Crypto _crypto;
        public ClientesController(AppDBContext context, IContrasenaService contrasenaService, Crypto crypto)
        {
            _context = context;
            _contrasenaService = contrasenaService;
            _crypto = crypto;
        }

        [HttpGet]
        public async Task<ActionResult> obtenerTodosLosClientes()
        {
            List<Cliente> clientes = await _context.Clientes.Where(c => c.Habilitado && !c.EsAdmin).ToListAsync();

            if(clientes == null || clientes.Count == 0)
            {
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

            return Ok(clientesResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> obtenerCliente(int id)
        {
            Cliente? cliente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (cliente == null)
            {
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
                return ValidationProblem(ModelState);
            }

            if (await _context.Clientes.AnyAsync(c => c.Login == cliente.Login))
            {
                return BadRequest(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error cliente ya existe" } } } });
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre)) { return BadRequest(new { mensaje = "El nombre es obligatorio." }); }

            //cliente.Contrasena = _crypto.Encriptar(cliente.Contrasena);

            if (!_contrasenaService.EsContrasenaSegura(_crypto.Desencriptar(cliente.Contrasena)))
            {
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
                Contrasena = _contrasenaService.hashContrasena(_crypto.Desencriptar(cliente.Contrasena))
            };

            try
            {
                _context.Clientes.Add(nuevoCliente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al crear cliente" } } } }); }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> actualizarCliente(int id, [FromBody] ActualizarClienteRequest cliente)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            Cliente? clienteExistente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                return NotFound();
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Apellido = cliente.Apellido;
            clienteExistente.Empresa = cliente.Empresa;
            clienteExistente.Login = cliente.Login;

            string contrasena = _crypto.Desencriptar(cliente.Contrasena);

            if (!string.IsNullOrWhiteSpace(contrasena))
            {
                if (!_contrasenaService.EsContrasenaSegura(contrasena))
                {
                    return BadRequest(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar cliente" } } } });
                }

                clienteExistente.Contrasena = _contrasenaService.hashContrasena(contrasena);
            }

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al actualizar cliente" } } } }); }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> eliminarCliente(int id)
        {
            Cliente? clienteExistente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                return NotFound(new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Cliente no encontrado" } } } });
            }

            clienteExistente.Habilitado = false;

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorDTO() { Status = 500, Errors = new Dictionary<string, List<string>> { { "Error", new List<string> { "Error al eliminar cliente" } } } });
            }
        }
    }
}
