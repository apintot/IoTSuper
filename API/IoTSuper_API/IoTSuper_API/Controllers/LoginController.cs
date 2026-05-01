using IoTSuper_API.Data;
using IoTSuper_API.DTO.Cliente;
using IoTSuper_API.DTO.Login;
using IoTSuper_API.Models;
using IoTSuper_API.Security;
using IoTSuper_API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class LoginController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IContrasenaService _contrasenaService;
        private readonly Crypto _crypto;

        public LoginController(AppDBContext context, IContrasenaService contrasenaService, Crypto crypto)
        {
            _context = context;
            _contrasenaService = contrasenaService;
            _crypto = crypto;
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest loginRequest)
        {
            if(!ModelState.IsValid) { return BadRequest(ModelState); }

            loginRequest.Contrasena = _crypto.Encriptar(loginRequest.Contrasena);

            Cliente? cliente = await _context.Clientes.Where(c => c.Login == loginRequest.Usuario && c.Habilitado).FirstOrDefaultAsync();

            if (cliente == null || !_contrasenaService.VerificarContrasena(cliente.Contrasena, _crypto.Desencriptar(loginRequest.Contrasena))) 
            {
                return Unauthorized();
            }

            LoginResponse loginResponse = new LoginResponse
            {
                IdCliente = cliente.IdCliente,
                EsAdmin = cliente.EsAdmin
            };

            return Ok(loginResponse);
        }
    }
}
