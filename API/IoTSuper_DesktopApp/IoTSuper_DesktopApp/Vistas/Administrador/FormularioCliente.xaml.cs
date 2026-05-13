using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Seguridad;
using IoTSuper_DesktopApp.Servicios.Cliente;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IoTSuper_DesktopApp.Vistas.Administrador
{
    /// <summary>
    /// Lógica de interacción para FormularioCliente.xaml
    /// </summary>
    public partial class FormularioCliente : UserControl
    {
        Cliente _cliente = null;

        public FormularioCliente()
        {
            InitializeComponent();
        }

        public FormularioCliente(Cliente cliente)
        {
            InitializeComponent();

            _cliente = cliente;
            txbActualizarTitulo.Visibility = Visibility.Visible;
            txbCrearTitulo.Visibility = Visibility.Collapsed;

            rellenarDatosEnCampos();

            btnActualizarCliente.Visibility = Visibility.Visible;
            btnCrearCliente.Visibility = Visibility.Collapsed;
        }

        private void rellenarDatosEnCampos()
        {
            camNombre.Texto = _cliente.Nombre;
            camApellido.Texto = _cliente.Apellido;
            camEmpresa.Texto = _cliente.Empresa;
            camLogin.Texto = _cliente.Login;
        }

        private async void CrearCliente_ClickAsync(object sender, RoutedEventArgs e)
        {
            Cliente clienteNuevo = new Cliente();
            Crypto crypto = new Crypto();

            clienteNuevo.Nombre = camNombre.Texto;
            clienteNuevo.Apellido = camApellido.Texto;
            clienteNuevo.Empresa = camEmpresa.Texto;
            clienteNuevo.Login = camLogin.Texto;

            if(string.IsNullOrEmpty(camContrasena.Texto) || camContrasena.Texto.Length < 12)
            {
                txbErrorContrasena.Text = "La contraseña no puede estar vacía.";
                txbErrorContrasena.Visibility = Visibility.Visible;
                return;
            }

            clienteNuevo.Contrasena = crypto.Encriptar(camContrasena.Texto);

            ActualizarClienteResponse errores = await ClienteService.CrearCliente(clienteNuevo);

            if (errores != null && errores.Status == 200) { Navegacion.IrA(new AdminInicio()); }

            MostrarErrores(errores);
        }

        private void MostrarErrores(ActualizarClienteResponse errores)
        {
            foreach (KeyValuePair<string, List<string>> error in errores.Errors)
            {
                if (error.Key.Equals("Login"))
                {
                    txbErrorLogin.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorLogin.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Contrasena"))
                {
                    txbErrorContrasena.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorContrasena.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Nombre"))
                {
                    txbErrorNombre.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorNombre.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Apellido"))
                {
                    txbErrorApellido.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorApellido.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Empresa"))
                {
                    txbErrorEmpresa.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorEmpresa.Visibility = Visibility.Visible;
                }
            }
        }

        private async void ActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (_cliente != null)
            {
                Crypto crypto = new Crypto();
                _cliente.Contrasena = crypto.Encriptar(_cliente.Contrasena);

                _cliente.Nombre = camNombre.Texto;
                _cliente.Apellido = camApellido.Texto;
                _cliente.Empresa = camEmpresa.Texto;
                _cliente.Login = camLogin.Texto;

                ActualizarClienteResponse errores = await ClienteService.actualizarCliente(_cliente);

                if (errores != null && errores.Status == 200) { Navegacion.IrA(new AdminInicio()); }

                MostrarErrores(errores);
            }
        }
    }


}
