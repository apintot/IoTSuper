using IoTSuper_API.Data;
using IoTSuper_API.DTO;
using IoTSuper_API.DTO.Evento;
using IoTSuper_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MQTTnet;
using System.Buffers;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace IoTSuper_API.Services.Worker
{
    public class EventoWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConfiguracionEmail _configuracionEmail;
        private static IMqttClient? mqttClient;

        public EventoWorker(IServiceScopeFactory scopeFactory, IOptions<ConfiguracionEmail> emailOptions)
        {
            _scopeFactory = scopeFactory;
            _configuracionEmail = emailOptions.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MqttClientFactory mqttFactory = new MqttClientFactory();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    //.WithTcpServer("iotsuper.duckdns.org", 8883)
                    .WithTcpServer("localhost", 8883)
                    .WithCredentials("iotsuper", "iotsupermqtt")
                    .WithTlsOptions(o => o
                    .UseTls()
                    .WithCertificateValidationHandler(_ => true))
                .WithCleanSession()
                .Build();

            mqttClient = mqttFactory.CreateMqttClient();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                procesarLLamadaMqttAsync(e.ApplicationMessage.Topic, e.ApplicationMessage.Payload);
                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            await subscribirseAMiTopic();

            mqttClient.DisconnectedAsync += async d =>
            {
                try
                {
                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                    await subscribirseAMiTopic();
                    Console.WriteLine("Reconectado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fallo al reconectar: {ex.Message}");
                }
            };

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public static async Task subscribirseAMiTopic()
        {
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
            .WithTopic("IoTSuper/#")
            .Build(), CancellationToken.None);
        }

        private async Task procesarLLamadaMqttAsync(string topic, System.Buffers.ReadOnlySequence<byte> payload)
        {
            using IServiceScope scope = _scopeFactory.CreateScope();
            AppDBContext _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            string mensaje = Encoding.UTF8.GetString(payload.ToArray());

            if (topic.Contains("OUTPUT")) { return; }

            topic = topic.Split('/')[2];

            Componente componente = await _context.Componentes.Where(c => c.Topic == topic).FirstOrDefaultAsync();

            if (componente == null) { return; }

            Stock stock = await _context.Stocks.Where(s => s.IdComponente == componente.IdComponente).FirstOrDefaultAsync();

            if (stock != null)
            {
                double peso = double.TryParse(mensaje.Replace('.', ','), out double result) ? result : 0;

                int valorStock = (int)(peso / stock.Peso_Unidad);

                bool bajoMinimo = valorStock < stock.Stock_Minimo;

                if (bajoMinimo)
                {
                    string tipoEvento = $"Componente: { componente.Nombre} (ID:{ componente.IdComponente}) | Valor recibido: { valorStock} | Mínimo permitido: { stock.Stock_Minimo}";

                    bool yaExisteEvento = await EventoRecienteExisteAsync(componente.IdComponente, tipoEvento, _context);

                    if (!yaExisteEvento)
                    {
                        await CrearEventoAsync(componente.IdComponente, tipoEvento, _context);
                    }
                }

                return;
            }
            else
            {
                Termometro termometro = await _context.Termometros.Where(t => t.IdComponente == componente.IdComponente).FirstOrDefaultAsync();

                if (termometro != null)
                {
                    double temperatura = double.TryParse(mensaje.Replace('.', ','), out double result) ? result : 0;
                    bool fueraRango = temperatura < termometro.Temperatura_Minima || temperatura > termometro.Temperatura_Maxima;
                    if (fueraRango)
                    {
                        string tipoEvento = $"Componente: { componente.Nombre} (ID:{ componente.IdComponente}) | Valor recibido: { temperatura} | Rango permitido: { termometro.Temperatura_Minima} - { termometro.Temperatura_Maxima}";
                        bool yaExisteEvento = await EventoRecienteExisteAsync(componente.IdComponente, tipoEvento, _context);
                        if (!yaExisteEvento)
                        {
                            await CrearEventoAsync(componente.IdComponente, tipoEvento, _context);
                        }
                    }
                }
                else
                {
                    Etiqueta etiqueta = await _context.Etiquetas.Where(t => t.IdComponente == componente.IdComponente).FirstOrDefaultAsync();

                    if (mensaje.Equals("VISTO")) { etiqueta.Visualizaciones += 1; _context.Etiquetas.Update(etiqueta); await _context.SaveChangesAsync(); }

                    return;
                }
            }
        }

        private async Task CrearEventoAsync(int idComponente, string tipoEvento, AppDBContext _context)
        {
            Evento evento = new Evento
            {
                IdComponente = idComponente,
                TipoEvento = tipoEvento,
                FechaEvento = DateTime.UtcNow
            };

            await EnviarCorreoAsync(new EventoDTO { IdComponente = idComponente, TipoEvento = tipoEvento, FechaEvento = DateTime.UtcNow }, _context);

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> EventoRecienteExisteAsync(int idComponente, string tipoEvento, AppDBContext _context)
        {
            return await _context.Eventos.AnyAsync(e => e.IdComponente == idComponente && e.TipoEvento == tipoEvento && e.FechaEvento >= DateTime.UtcNow.AddHours(-24));
        }

        public async Task EnviarCorreoAsync(EventoDTO evento, AppDBContext _context)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_configuracionEmail.EmailEnvio, _configuracionEmail.ContrasenaEnvio),
                EnableSsl = true
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress(_configuracionEmail.EmailEnvio),
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
