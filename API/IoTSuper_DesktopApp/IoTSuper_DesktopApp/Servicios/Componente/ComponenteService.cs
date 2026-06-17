using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.Componente
{
    public static class ComponenteService
    {
        public static async Task<List<Modelos.ComponenteDTO>> ObtenerComponentesSeccion(int idSeccion)
        {
            Modelos.Cliente request = new Modelos.Cliente() { };
            try
            {
                return await APIService.GetAsync<List<Modelos.ComponenteDTO>>(Sesion.ApiConfigFolder.EndPointComponente + $"/{idSeccion}") ?? new List<Modelos.ComponenteDTO>();
            }
            catch (Exception ex) { return new List<Modelos.ComponenteDTO>(); }
        }

        public static async Task<ErrorDTO> CrearComponente(ComponenteDTO componente)
        {
            try
            {
                return await APIService.PostAsync<ErrorDTO>(Sesion.ApiConfigFolder.EndPointComponente, componente) ?? new ErrorDTO();
            }
            catch (Exception ex) { return new ErrorDTO(); }
        }

        public static async Task<ErrorDTO> ActualizarComponente(ComponenteDTO componente)
        {
            try
            {
                return await APIService.PutAsync<ErrorDTO>(Sesion.ApiConfigFolder.EndPointComponente, componente) ?? new ErrorDTO();
            }
            catch (Exception ex) { return new ErrorDTO(); }
        }

        public static async Task<ErrorDTO> EliminarComponente(int idComponente)
        {
            try
            {
                return await APIService.DeleteAsync<ErrorDTO>(Sesion.ApiConfigFolder.EndPointComponente + $"/{idComponente}") ?? new ErrorDTO();
            }
            catch (Exception ex) { return new ErrorDTO(); }
        }
    }
}
