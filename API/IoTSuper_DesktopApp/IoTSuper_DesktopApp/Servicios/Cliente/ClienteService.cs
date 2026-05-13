using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.API;
using IoTSuper_DesktopApp.Servicios.Cliente;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.Cliente
{
    public static class ClienteService
    {
        public static async Task<List<Modelos.Cliente>> ObtenerClientes()
        {
            Modelos.Cliente request = new Modelos.Cliente() { };
            try
            {
                return await APIService.GetAsync<List<Modelos.Cliente>>(Sesion.ApiConfigFolder.EndPointCliente) ?? new List<Modelos.Cliente>();
            }
            catch (Exception ex) { return new List<Modelos.Cliente>(); }
        }

        public static async Task<ActualizarClienteResponse> CrearCliente(Modelos.Cliente cliente)
        {
            try
            {
                return await APIService.PostAsync<ActualizarClienteResponse>(Sesion.ApiConfigFolder.EndPointCliente, cliente) ?? new ActualizarClienteResponse();
            }
            catch (Exception ex) { return new ActualizarClienteResponse() { Status = 500 }; }
        }

        public static async Task<ActualizarClienteResponse> actualizarCliente(Modelos.Cliente cliente) 
        {
            try
            {
                return await APIService.PutAsync<ActualizarClienteResponse>(Sesion.ApiConfigFolder.EndPointCliente + $"/{cliente.IdCliente}", cliente) ?? new ActualizarClienteResponse();
            }
            catch (Exception ex) { return new ActualizarClienteResponse() { Status = 500 }; }
        }

        public static async Task<bool> eliminarCliente(int id)
        {
            try
            {
                return await APIService.DeleteAsync<bool>(Sesion.ApiConfigFolder.EndPointCliente + $"/{id}");
            }
            catch (Exception ex) { return false; }
        }
    }
}
