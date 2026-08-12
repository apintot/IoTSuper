using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Servicios.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.Eventos
{
    public static class EventosServices
    {
        public static async Task<List<Modelos.EventoDTO>> obtenerEventosRecientes()
        {
            try
            {
                return await APIService.GetAsync<List<Modelos.EventoDTO>>(Sesion.ApiConfigFolder.EndPointEventos) ?? new List<Modelos.EventoDTO>();
            }
            catch (Exception ex) { return new List<Modelos.EventoDTO>(); }
        }
    }
}
