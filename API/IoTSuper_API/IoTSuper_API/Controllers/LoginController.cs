using IoTSuper_API.Data;
using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Cliente;
using IoTSuper_API.DTO.Login;
using IoTSuper_API.Models;
using IoTSuper_API.Security;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginRequest = IoTSuper_API.DTO.Login.LoginRequest;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class LoginController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IContrasenaService _contrasenaService;
        private readonly ILogService _logger;

        public LoginController(AppDBContext context, IContrasenaService contrasenaService, ILogService logger)
        {
            _context = context;
            _contrasenaService = contrasenaService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {

            try
            {
                await _logger.LogAsync($"Intentando iniciar sesión para el usuario {loginRequest.Usuario}");
                if (!ModelState.IsValid) { await _logger.LogAsync($"Error de validación: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}"); return BadRequest(ModelState); }

                Cliente? cliente = await _context.Clientes.Where(c => c.Login == loginRequest.Usuario && c.Habilitado).FirstOrDefaultAsync();

                if (cliente == null || !_contrasenaService.VerificarContrasena(cliente.Contrasena, loginRequest.Contrasena))
                {
                    await _logger.LogAsync($"Inicio de sesión fallido para el usuario {loginRequest.Usuario}");
                    return Unauthorized();
                }

                LoginResponse loginResponse = new LoginResponse
                {
                    IdCliente = cliente.IdCliente,
                    EsAdmin = cliente.EsAdmin,
                    TOTP = cliente.Totp,
                    ultimoAcceso = cliente.UltimoAcceso ?? DateTime.Now
                };

                await _logger.LogAsync($"Inicio de sesión exitoso para el usuario {loginRequest.Usuario}");

                return Ok(loginResponse);
            }
            catch (Exception ex) 
            {
                await _logger.LogAsync($"Error procesando el inicio de sesión para el usuario {loginRequest.Usuario}: {ex.Message}");
                return StatusCode(
                   StatusCodes.Status500InternalServerError,
                   new
                   {
                       mensaje = "Error procesando el inicio de sesión",
                       detalle = ex.Message // Solo temporalmente durante desarrollo
                    });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarTOTP(int id,[FromBody] TOTPRequest topt)
        {
            try
            {
                await _logger.LogAsync($"Intentando actualizar TOTP para el usuario con ID {id}");
                if (!ModelState.IsValid) 
                { 
                    await _logger.LogAsync($"Error de validación: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState); 
                }

                Cliente? cliente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado).FirstOrDefaultAsync();

                if (cliente == null) 
                { 
                    await _logger.LogAsync($"Usuario con ID {id} no existe");
                    return BadRequest("Usuario no existe"); 
                }

                cliente.UltimoAcceso = DateTime.Now;
                cliente.Totp = topt.Totp;

                await _context.SaveChangesAsync();
                await _logger.LogAsync($"TOTP actualizado exitosamente para el usuario con ID {id}");
                return Ok();
            }
            catch(Exception ex) 
            { 
                await _logger.LogAsync($"Error al actualizar TOTP para el usuario con ID {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error"); 
            }
        }
    }
}
