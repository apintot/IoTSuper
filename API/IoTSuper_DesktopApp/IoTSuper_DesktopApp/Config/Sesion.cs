using IoTSuper_DesktopApp.Controladores.Componentes;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Seguridad;
using Microsoft.AspNetCore.Http;
using MQTTnet;
using MQTTnet.Extensions.TopicTemplate;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace IoTSuper_DesktopApp.Config
{
    public static class Sesion
    {
        public static ApiConfigFolder ApiConfigFolder = new ApiConfigFolder();
        public static LoginResponse LoginData = new LoginResponse();
        public static readonly string msiName = Assembly.GetExecutingAssembly().GetName().Name;

        public static event Action OnComponenteActualizado;

        public static List<Modelos.CentroDTO> _centros;
        public static ObservableCollection<ResumenDTO> Componentes { get; set; } = new();

        public static int seccionSelecionado = 0;
        public static int centroSelecionado = 0;

        public static MQTT Mqtt = new MQTT();

        #region Subscripcion a un topic

        public static async Task Subscribe_Topic()
        {
            Crypto crypto = new Crypto();

            MqttClientFactory mqttFactory = new MqttClientFactory();

            MqttTopicTemplate topicToConnect = new(crypto.Desencriptar(Mqtt.topic));

            IMqttClient mqttClient = mqttFactory.CreateMqttClient();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(crypto.Desencriptar(Mqtt.broker), 1883).Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder().WithTopicTemplate(topicToConnect).Build();

            MqttClientSubscribeResult response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

            mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
        }

        private static Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            LogLocal.logear($"Topic:{e.ApplicationMessage.Topic}, Payload:{Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            return Task.CompletedTask;
        }

        #endregion

        #region Publicar mensaje a un topic

        private static IMqttClient mqttClient;

        public static async Task conectarAMqtt()
        {
            MqttClientFactory mqttFactory = new MqttClientFactory();
            
            Crypto crypto = new Crypto();

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(crypto.Desencriptar(Mqtt.broker), 8883)
                    .WithCredentials(crypto.Desencriptar(Mqtt.usuario), crypto.Desencriptar(Mqtt.contrasena))
                    .WithTlsOptions(o => o
                    .UseTls()
                        .WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12))
                        .WithCleanSession()
                    .Build();

            mqttClient = mqttFactory.CreateMqttClient();

            mqttClient.DisconnectedAsync += async e =>
            {
                LogLocal.logear("Desconectado de MQTT, intentando reconectar...");
                await Task.Delay(TimeSpan.FromSeconds(1));
                try
                {
                    await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
                    await subscribirseAMiTopic();
                    LogLocal.logear("Reconectado a MQTT.");
                }
                catch (Exception ex)
                {
                    LogLocal.logear($"Error al reconectar a MQTT: {ex.Message}");
                }
            };

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                procesarLLamadaMqtt(e.ApplicationMessage.Topic, e.ApplicationMessage.Payload);
                return Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

            await subscribirseAMiTopic();

        }

        public static async Task subscribirseAMiTopic()
        {
            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
            .WithTopic("IoTSuper/#")
            .Build(), CancellationToken.None);
        }

        private static void procesarLLamadaMqtt(string topic, System.Buffers.ReadOnlySequence<byte> payload)
        {
            string mensaje = Encoding.UTF8.GetString(payload.ToArray());

            if (topic.Contains("OUTPUT")) { return; }

            topic = topic.Split('/').Last();

            ComponenteDTO componenteActual = _centros.SelectMany(c => c._secciones).SelectMany(s => s._componentes).FirstOrDefault(c=> c.Topic == topic);

            if(componenteActual == null) { return; }

            ResumenDTO resumenActual = Componentes.FirstOrDefault(c => c.IdComponente == componenteActual.IdComponente);

            if (resumenActual == null) { return; }

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (componenteActual.Stock != null)
                {
                    double peso = double.TryParse(mensaje.Replace('.', ','), out double result) ? result : 0;

                    int unidades = (int)(peso / componenteActual.Stock.Peso_Unidad);

                    componenteActual.Stock.Stock_Actual = unidades;
                    resumenActual.UltimoDato = $"{componenteActual.Stock.Stock_Actual} uds";
                    resumenActual.Estado = componenteActual.Stock.Stock_Actual > 0 ? "OK" : "Vacío";

                    if(resumenActual.Estado.Equals("Ok")) { resumenActual.Estado = componenteActual.Stock.Stock_Actual > componenteActual.Stock.Stock_Minimo ? "OK" : "Agotandose"; }

                    resumenActual.EstadoColor = resumenActual.Estado switch
                    {
                        "OK" => new SolidColorBrush(Colors.Green),
                        "Vacío" => new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44)),
                        "Error" => new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44)),
                        "Agotandose" => new SolidColorBrush(Color.FromRgb(0xFB, 0xBF, 0x24))
                    };

                    resumenActual.Disponible?.Stop();
                    resumenActual.Disponible?.Start();
                }
                else if (componenteActual.Termometro != null)
                {
                    componenteActual.Termometro.Temperatura_Actual = double.TryParse(mensaje.Replace('.', ','), out double result) ? result : 0;
                    resumenActual.UltimoDato = $"{componenteActual.Termometro.Temperatura_Actual} °C";
                    resumenActual.Estado = componenteActual.Termometro.Temperatura_Maxima >= componenteActual.Termometro.Temperatura_Actual && componenteActual.Termometro.Temperatura_Actual >= componenteActual.Termometro.Temperatura_Minima ? "OK" : "Alerta!";

                    resumenActual.EstadoColor = resumenActual.Estado switch
                    {
                        "OK" => new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81)),
                        "Error" => new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44)),
                        "Alerta!" => new SolidColorBrush(Color.FromRgb(0x25, 0x63, 0xEB))
                    };

                    resumenActual.Disponible?.Stop();
                    resumenActual.Disponible?.Start();
                }
                else if (componenteActual.Etiqueta != null)
                {
                    if(mensaje.Equals("PING"))
                    {
                        resumenActual.Estado =  "OK";
                    }

                    if(mensaje.Equals("VISTO"))
                    {
                        resumenActual.Estado = "OK";
                        //TODO: get componenete y acualizarlo
                    }

                    resumenActual.EstadoColor = resumenActual.Estado switch
                    {
                        "OK" => new SolidColorBrush(Color.FromRgb(0x10, 0xB9, 0x81)),
                        "Error" => new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44))
                    };

                    resumenActual.Disponible?.Stop();
                    resumenActual.Disponible?.Start();
                }

                resumenActual.Actualizado = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            });

            OnComponenteActualizado.Invoke();
        }

        public static async void publicarMensajeMqtt(string topic, string payload)
        {
            Crypto crypto = new Crypto();
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(crypto.Desencriptar(Mqtt.topic) + "/" + topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                .Build());
        }

        #endregion
    }
}
