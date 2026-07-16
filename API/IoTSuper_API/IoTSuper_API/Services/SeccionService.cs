using IoTSuper_API.Data;
using IoTSuper_API.DTO.Seccion;
using IoTSuper_API.Models;
using IoTSuper_API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Services
{
    public class SeccionService : ISeccionService
    {
        private readonly AppDBContext _context;

        public SeccionService(AppDBContext context)
        {
            _context = context;
        }

        public async Task ActualizarSeccionAsync(SeccionDTO seccionDTO)
        {
            Seccion seccion = await _context.Secciones.FirstOrDefaultAsync(s => s.IdSeccion == seccionDTO.IdSeccion);

            if (seccion == null) { throw new Exception("Sección no encontrada"); }

            seccion.Nombre = seccionDTO.Nombre;
            seccion.Imagen = seccionDTO.Imagen;
            seccion.Habilitado = seccionDTO.Habilitado;
            seccion.UpdateAt = DateTime.UtcNow;
            
            _context.Secciones.Update(seccion);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CrearSeccionAsync(SeccionDTO seccionDTO)
        {
            Seccion seccion = new Seccion
            {
                IdCentro = seccionDTO.IdCentro,
                Nombre = seccionDTO.Nombre,
                Imagen = seccionDTO.Imagen,
                Habilitado = seccionDTO.Habilitado,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Secciones.AddAsync(seccion);
            await _context.SaveChangesAsync();

            return seccion.IdSeccion;
        }

        public async Task EliminarSeccionAsync(int id)
        {
            await _context.Secciones
                .Where(s => s.IdSeccion == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(p => p.Habilitado, false)
                    .SetProperty(p => p.UpdateAt, DateTime.UtcNow));
        }

        public async Task<List<SeccionDTO>> ObtenerSeccionesAsync(int centroId)
        {
            return await _context.Secciones
                .Where(s => s.IdCentro == centroId && s.Habilitado == true)
                .Select(s => new SeccionDTO
                {
                    IdSeccion = s.IdSeccion,
                    IdCentro = s.IdCentro,
                    Nombre = s.Nombre,
                    Imagen = s.Imagen,
                    Habilitado = s.Habilitado,
                    UpdateAt = s.UpdateAt,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();
        }
    }
}
