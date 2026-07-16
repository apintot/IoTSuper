using IoTSuper_API.DTO.Seccion;

namespace IoTSuper_API.Services.Interface
{
    public interface ISeccionService
    {
        Task ActualizarSeccionAsync(SeccionDTO seccionDTO);
        Task<int> CrearSeccionAsync(SeccionDTO seccionDTO);
        Task EliminarSeccionAsync(int id);
        Task<List<SeccionDTO>> ObtenerSeccionesAsync(int centroId);
    }
}
