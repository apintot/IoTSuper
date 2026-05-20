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

        public static async Task<ErrorPost> GuardarSeccion(SeccionDTO seccion)
        {
            try
            {
                return await APIService.PostAsync<ErrorPost>(Sesion.ApiConfigFolder.EndPointSecciones, seccion) ?? new ErrorPost { Status = 500 };
            }
            catch (Exception ex) { return new ErrorPost { Status = 500 }; }
        }

        internal static async Task<ErrorPost> ActualizarSeccion(SeccionDTO seccion)
        {
            try
            {
                return await APIService.PutAsync<ErrorPost>(Sesion.ApiConfigFolder.EndPointSecciones, seccion) ?? new ErrorPost { Status = 500 };
            }
            catch (Exception ex) { return new ErrorPost { Status = 500 }; }
        }
    }
}
