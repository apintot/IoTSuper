using IoTSuper_API.Data;
using IoTSuper_API.DTO.Centro;
using IoTSuper_API.DTO.Localizacion;
using IoTSuper_API.DTO.Tipologia;
using IoTSuper_API.Models;
using IoTSuper_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace IoTSuper_API.Services
{
    public class CentroService : ICentroService
    {
        private readonly AppDBContext _context;

        public CentroService(AppDBContext context)
        {
            _context = context;
        }

        public async Task ActualizarCentroAsync(CentroDTO centroDTO)
        {
            Localizacion localizacion = await _context.Localizaciones.FirstOrDefaultAsync(l => l.IdLocalizacion == centroDTO.IdLocalizacion) ?? new Localizacion();

            Centro nuevoCentro = await _context.Centros.FirstOrDefaultAsync(c => c.IdCentro == centroDTO.IdCentro) ?? new Centro();

            localizacion.CodigoPostal = centroDTO.Localizacion.CodigoPostal;
            localizacion.Direccion = centroDTO.Localizacion.Direccion;
            localizacion.Pais = centroDTO.Localizacion.Pais;
            localizacion.provincia = centroDTO.Localizacion.provincia;

            nuevoCentro.IdCliente = centroDTO.IdCliente;
            nuevoCentro.IdTipologia = centroDTO.IdTipologia;
            nuevoCentro.IdLocalizacion = localizacion.IdLocalizacion;
            nuevoCentro.Nombre = centroDTO.Nombre;
            nuevoCentro.Imagen = centroDTO.Imagen;
            nuevoCentro.Cif = centroDTO.Cif;
            nuevoCentro.RazonSocial = centroDTO.RazonSocial;
            nuevoCentro.UpdatedAt = DateTime.UtcNow;

            _context.Localizaciones.Update(localizacion);
            _context.Centros.Update(nuevoCentro);

            await _context.SaveChangesAsync();
        }

        public async Task CrearCentroAsync(CentroDTO centroDTO)
        {
            Localizacion localizacion = new Localizacion()
            {
                CodigoPostal = centroDTO.Localizacion.CodigoPostal,
                Direccion = centroDTO.Localizacion.Direccion,
                Pais = centroDTO.Localizacion.Pais,
                provincia = centroDTO.Localizacion.provincia
            };

            await _context.Localizaciones.AddAsync(localizacion);
            await _context.SaveChangesAsync();

            Centro nuevoCentro = new Centro()
            {
                IdCliente = centroDTO.IdCliente,
                IdTipologia = centroDTO.IdTipologia,
                IdLocalizacion = localizacion.IdLocalizacion,
                Habilitado = centroDTO.Habilitado,
                Nombre = centroDTO.Nombre,
                Imagen = centroDTO.Imagen,
                Cif = centroDTO.Cif,
                RazonSocial = centroDTO.RazonSocial,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Centros.AddAsync(nuevoCentro);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarCentroAsync(int id)
        {
            Centro centro = await _context.Centros.FirstOrDefaultAsync(c => c.IdCentro == id);

            if (centro != null)
            {
                centro.Habilitado = false;
                _context.Centros.Update(centro);
                _context.SaveChanges();
            }
        }

        public async Task<List<CentroDTO>> ObtenerCentrosAsync(int id)
        {
            List<CentroDTO> centros = await _context.Centros
            .Where(c => c.IdCliente == id)
            .Select(c => new CentroDTO
            {
                IdCentro = c.IdCentro,
                IdCliente = c.IdCliente,
                IdTipologia = c.IdTipologia,
                IdLocalizacion = c.IdLocalizacion,
                Habilitado = c.Habilitado,
                Nombre = c.Nombre,
                Imagen = c.Imagen,
                Cif = c.Cif,
                RazonSocial = c.RazonSocial,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            })
            .Where(c => c.Habilitado)
            .ToListAsync();

            foreach (CentroDTO centro in centros)
            {
                centro.Localizacion = await ObtenerLocalizacionAsync(centro.IdLocalizacion);
            }

            return centros;
        }

        internal async Task<LocalizacionDTO> ObtenerLocalizacionAsync(int idLocalizacion)
        {
            return await _context.Localizaciones.Where(l => l.IdLocalizacion == idLocalizacion).Select(l => new LocalizacionDTO
            {
                Direccion = l.Direccion,
                CodigoPostal = l.CodigoPostal,
                Pais = l.Pais,
                provincia = l.provincia
            }).FirstOrDefaultAsync();
        }

        internal async Task<TipologiaDTO> ObtenerTipologiaAsync(int idTipologia)
        {
            return await _context.Tipologias.Where(t => t.IdTipologia == idTipologia).Select(t => new TipologiaDTO
            {
                TipoTienda = t.TipoTienda
            }).FirstOrDefaultAsync();
        }
    }
}
