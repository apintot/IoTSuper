using IoTSuper_API.DTO.Componentes;

namespace IoTSuper_API.Services.Interface
{
    public interface IComponenteService
    {
        Task ActualizarComponenteAsync(ComponenteDTO componenteDTO);
        Task<int> CrearComponenteAsync(ComponenteDTO componenteDTO);
        Task EliminarComponenteAsync(int idComponente);
        Task<List<ComponenteDTO>> GetComponentesAsync(int seccion);
        Task SumarUnoVisualizacionAsync(string topic);
    }
}
