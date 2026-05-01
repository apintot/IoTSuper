using IoTSuper_API.Data;
using IoTSuper_API.DTO.Cliente;
using IoTSuper_API.Models;
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
        public ClientesController(AppDBContext context, IContrasenaService contrasenaService)
        {
            _context = context;
            _contrasenaService = contrasenaService;
        }

        [HttpGet]
        public async Task<ActionResult> obtenerTodosLosClientes()
        {
            List<Cliente> clientes = await _context.Clientes.Where(c => c.Habilitado && !c.EsAdmin).ToListAsync();

            List<ClienteResponse> clientesResponse = clientes.Select(c => new ClienteResponse
            {
                IdCliente = c.IdCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                Empresa = c.Empresa,
                Login = c.Login,
            }).ToList();

            return Ok(clientesResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> obtenerCliente(int id)
        {
            Cliente? cliente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }

            ClienteResponse clienteResponse = new ClienteResponse
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Empresa = cliente.Empresa,
                Login = cliente.Login,
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
                return BadRequest(new { mensaje = "El login ya existe." });
            }

            if (string.IsNullOrWhiteSpace(cliente.Nombre)) { return BadRequest(new { mensaje = "El nombre es obligatorio." }); }

            if (!_contrasenaService.EsContrasenaSegura(cliente.Contrasena))
            {
                return BadRequest(new
                {
                    mensaje = "La contraseña debe tener al menos 12 caracteres e incluir mayúsculas, minúsculas, números y caracteres especiales."
                });
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
            }
            catch (Exception ex) { return StatusCode(500, new { mensaje = "Error interno del servidor." }); }

            ClienteResponse response = new ClienteResponse
            {
                IdCliente = nuevoCliente.IdCliente,
                Nombre = nuevoCliente.Nombre,
                Apellido = nuevoCliente.Apellido,
                Empresa = nuevoCliente.Empresa,
                Login = nuevoCliente.Login,
            };

            return CreatedAtAction(nameof(obtenerCliente), new { id = nuevoCliente.IdCliente }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> actualizarCliente(int id, [FromBody] NuevoClienteRequest cliente)
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

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return StatusCode(500, new { mensaje = "Error interno del servidor." }); }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> eliminarCliente(int id)
        {
            Cliente? clienteExistente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (clienteExistente == null)
            {
                return NotFound();
            }

            clienteExistente.Habilitado = false;

            try
            {
                _context.Clientes.Update(clienteExistente);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return StatusCode(500, new { mensaje = "Error interno del servidor." }); }

            return Ok();
        }
    }
}
