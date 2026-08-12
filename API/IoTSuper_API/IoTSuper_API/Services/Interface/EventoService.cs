using IoTSuper_API.Data;
using IoTSuper_API.DTO.Evento;
using IoTSuper_API.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace IoTSuper_API.Services.Interface
{
    public class EventoService : IEventoService
    {
        private readonly AppDBContext _context;

        public EventoService(AppDBContext context)
        {
            _context = context;
        }

        public async Task CrearEventoAsync(EventoDTO evento)
        {
            Evento eventoNuevo = new Evento
            {
                IdComponente = evento.IdComponente,
                TipoEvento = evento.TipoEvento,
                FechaEvento = evento.FechaEvento
            };

            await _context.Eventos.AddAsync(eventoNuevo);

            await _context.SaveChangesAsync();
        }

        public async Task<List<EventoDTO>> ObtenerEventosAsync(int idUsuario)
        {
            List<EventoDTO> eventosDTO = await _context.Eventos
                .Where(e => _context.Componentes
                    .Where(c => _context.Secciones
                        .Where(s => _context.Centros
                            .Where(ce => ce.IdCliente == idUsuario)
                            .Select(ce => ce.IdCentro)
                            .Contains(s.IdCentro))
                        .Select(s => s.IdSeccion)
                        .Contains(c.IdSeccion))
                    .Select(c => c.IdComponente)
                    .Contains(e.IdComponente))
                .Select(e => new EventoDTO
                {
                    IdComponente = e.IdComponente,
                    TipoEvento = e.TipoEvento,
                    FechaEvento = e.FechaEvento
                })
                .ToListAsync();

            return eventosDTO;
        }
    }
}
