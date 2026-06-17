using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.Seccion
{
    public static class SeccionService
    {
        public static async Task EliminarSeccion(int idSeccion)
        {
            try
            {
                bool eliminado = await APIService.DeleteAsync<bool>($"{Sesion.ApiConfigFolder.EndPointSecciones}/{idSeccion}");
                if (!eliminado) throw new Exception("No se pudo eliminar la sección.");

            }
            catch (Exception ex) { throw new Exception("Error al eliminar la sección: " + ex.Message); }
        }

        public static async Task<ErrorDTO> GuardarSeccion(SeccionDTO seccion)
        {
            try
            {
                return await APIService.PostAsync<ErrorDTO>(Sesion.ApiConfigFolder.EndPointSecciones, seccion) ?? new ErrorDTO { Status = 500 };
            }
            catch (Exception ex) { return new ErrorDTO { Status = 500 }; }
        }

        internal static async Task<ErrorDTO> ActualizarSeccion(SeccionDTO seccion)
        {
            try
            {
                return await APIService.PutAsync<ErrorDTO>(Sesion.ApiConfigFolder.EndPointSecciones, seccion) ?? new ErrorDTO { Status = 500 };
            }
            catch (Exception ex) { return new ErrorDTO { Status = 500 }; }
        }
    }
}
