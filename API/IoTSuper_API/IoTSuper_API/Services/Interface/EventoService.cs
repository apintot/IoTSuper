using IoTSuper_API.Data;
using IoTSuper_API.DTO.Evento;
using IoTSuper_API.Models;
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

        public async Task EnviarCorreoAsync(EventoDTO evento)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("tuemail@gmail.com", "tupassword"),
                EnableSsl = true,
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("tuemail@gmail.com"),
                Subject = "ADVERTENCIA!",
                Body = $"Se ha detectado un error critico: {evento.TipoEvento}.",
                IsBodyHtml = true
            };

            string email = _context.Stocks.Where(c => c.IdComponente == evento.IdComponente).Select(c => c.EmailEmergencia).FirstOrDefault() ??
                           _context.Termometros.Where(c => c.IdComponente == evento.IdComponente).Select(c => c.EmailEmergencia).FirstOrDefault() ??
                           throw new Exception("No se encontró un correo electrónico de emergencia para el componente especificado.");

            mail.To.Add(email);
            smtp.Send(mail);
        }
    }
}
