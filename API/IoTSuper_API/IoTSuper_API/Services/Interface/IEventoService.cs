using IoTSuper_API.DTO.Centro;
using IoTSuper_API.DTO.Evento;

namespace IoTSuper_API.Services.Interface
{
    public interface IEventoService
    {
        Task EnviarCorreoAsync(EventoDTO evento);
        Task CrearEventoAsync(EventoDTO evento);
    }
}
