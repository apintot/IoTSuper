using IoTSuper_API.DTO.Centro;
using IoTSuper_API.DTO.Localizacion;
using IoTSuper_API.DTO.Tipologia;

namespace IoTSuper_API.Services.Interface
{
    public interface ICentroService
    {
        Task ActualizarCentroAsync(CentroDTO centroDTO);
        Task CrearCentroAsync(CentroDTO centroDTO);
        Task EliminarCentroAsync(int id);
        Task<List<CentroDTO>> ObtenerCentrosAsync(int id);

        //internal Task<LocalizacionDTO> ObtenerLocalizacionAsync(int idLocalizacion);
        //internal Task<TipologiaDTO> ObtenerTipologiaAsync(int idTipologia);
    }
}
