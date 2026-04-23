using IoTSuper_API.Data;
using IoTSuper_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Controllers
{
    [ApiController]
    [Route("IoTSuper/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuth")]
    public class ClientesController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ClientesController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> obtenerTodosLosClientes()
        { 
            List<Cliente> clientes = await _context.Clientes.Where(c => c.Habilitado && !c.EsAdmin).ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> obtenerCliente(int id)
        {
            Cliente? cliente = await _context.Clientes.Where(c => c.IdCliente == id && c.Habilitado && !c.EsAdmin).FirstOrDefaultAsync();

            if (cliente == null)
            {
                return NotFound();
            }

            return Ok(cliente);
        }
    }
}
