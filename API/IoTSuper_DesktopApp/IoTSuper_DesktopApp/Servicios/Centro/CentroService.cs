using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.API;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.Centro
{
    public static class CentroService
    {
        public static async Task<List<Modelos.CentroDTO>> ObtenerCentros(int idCliente)
        {
            try
            {
                return await APIService.GetAsync<List<Modelos.CentroDTO>>($"{Sesion.ApiConfigFolder.EndPointCentro}/{idCliente}") ?? new List<Modelos.CentroDTO>();
            }
            catch (Exception ex) { return new List<Modelos.CentroDTO>(); }
        }

        public static async Task EliminarCentro(int idCentro)
        {
            try
            {
                bool eliminado = await APIService.DeleteAsync<bool>($"{Sesion.ApiConfigFolder.EndPointCentro}/{idCentro}");

                if (!eliminado) throw new Exception("No se pudo eliminar el centro.");

            }
            catch (Exception ex) { throw new Exception("Error al eliminar el centro: " + ex.Message); }
        }

        public static async Task<List<PaisesDTO>> ObtenerPaises()
        {
                try
                {
                    return await APIService.GetAsync<List<PaisesDTO>>(Sesion.ApiConfigFolder.EndPointPaises, false) ?? new List<PaisesDTO>();
                }
                catch (Exception ex) { return new List<PaisesDTO>(); }
        }

        public static async Task<ProvinciaDTO> ObtenerProvincia(string pais)
        {
            try
            {
                return await APIService.PostAsync<ProvinciaDTO>(Sesion.ApiConfigFolder.EndPointProvincias, new { country = pais }, false) ?? new ProvinciaDTO();
            }
            catch (Exception ex) { return new ProvinciaDTO(); }
        }

        public static async Task<Dictionary<int, string>> ObtenerTipologias()
        {
            try
            {
                return await APIService.GetAsync<Dictionary<int, string>>(Sesion.ApiConfigFolder.EndPointTipologia) ?? new Dictionary<int, string>();
            }
            catch (Exception ex) { return new Dictionary<int, string>(); }
        }

        public static async Task<ErrorPost> GuardarCentro(CentroDTO centro)
        {
            try
            {
                return await APIService.PostAsync<ErrorPost>(Sesion.ApiConfigFolder.EndPointCentro, centro) ?? new ErrorPost { Status = 500 };
            }
            catch (Exception ex) { return new ErrorPost { Status = 500 }; }
        }

        public static async Task<ErrorPost> EditarCentro(CentroDTO centro)
        {
            try
            {
                return await APIService.PutAsync<ErrorPost>($"{Sesion.ApiConfigFolder.EndPointCentro}", centro) ?? new ErrorPost { Status = 500 };
            }
            catch (Exception ex)
            {
                return new ErrorPost { Status = 500 };
            }
        }

        public static async Task<List<SeccionDTO>> ObtenerSeccionesCentro(int idCentro)
        {
            try
            {
                return await APIService.GetAsync<List<SeccionDTO>>($"{Sesion.ApiConfigFolder.EndPointSecciones}/{idCentro}") ?? new List<SeccionDTO>();
            }
            catch (Exception ex) { return new List<SeccionDTO>(); }
        }
    }
}
