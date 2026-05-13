using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Seguridad;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace IoTSuper_DesktopApp.Servicios.API
{
    class APIService
    {
        private static readonly HttpClient _client = new HttpClient();

        public static async Task<T> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                Crypto crypto = new Crypto();
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

                string url = Sesion.ApiConfigFolder.API + endpoint;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{crypto.Desencriptar(Sesion.ApiConfigFolder.APIUsuario)}:{crypto.Desencriptar(Sesion.ApiConfigFolder.APIcontrasena)}")));
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.SendAsync(request, cts.Token);

                if (response.Content != null)
                    return await response.Content.ReadFromJsonAsync<T>();
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) { return (T)(object)true; }
                    else { return default(T); }
                }
            }
            catch (TaskCanceledException ex) { throw new Exception("No disponible"); }
            catch (Exception ex) { return default(T); }
        }

        public static async Task<T> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                Crypto crypto = new Crypto();
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

                string url = Sesion.ApiConfigFolder.API + endpoint;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{crypto.Desencriptar(Sesion.ApiConfigFolder.APIUsuario)}:{crypto.Desencriptar(Sesion.ApiConfigFolder.APIcontrasena)}")));
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.SendAsync(request, cts.Token);

                try
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) { return (T)(object)true; }
                    else { return default(T); }
                }
            }
            catch (TaskCanceledException ex) { throw new Exception("Login no disponible"); }
            catch (Exception ex) { return default(T); }
        }

        public static async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                Crypto crypto = new Crypto();
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));

                string url = Sesion.ApiConfigFolder.API + endpoint;

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{crypto.Desencriptar(Sesion.ApiConfigFolder.APIUsuario)}:{crypto.Desencriptar(Sesion.ApiConfigFolder.APIcontrasena)}")));
                //request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _client.SendAsync(request, cts.Token);

                try
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) { return (T)(object)true; }
                    else { return default(T); }
                }
            }
            catch (TaskCanceledException ex) { throw new Exception("Login no disponible"); }
            catch (Exception ex) { return default(T); }
        }

        public static async Task<T> DeleteAsync<T>(string endpoint)
        {
            try
            {
                Crypto crypto = new Crypto();
                using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                string url = Sesion.ApiConfigFolder.API + endpoint;
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{crypto.Desencriptar(Sesion.ApiConfigFolder.APIUsuario)}:{crypto.Desencriptar(Sesion.ApiConfigFolder.APIcontrasena)}")));
                HttpResponseMessage response = await _client.SendAsync(request, cts.Token);
                try
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                catch
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) { return (T)(object)true; }
                    else { return default(T); }
                }
            }
            catch (TaskCanceledException ex) { throw new Exception("Login no disponible"); }
            catch (Exception ex) { return default(T); }
        }
    }
}
