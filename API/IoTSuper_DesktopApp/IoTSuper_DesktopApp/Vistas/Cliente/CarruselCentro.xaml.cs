using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Controladores;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Servicios.Centro;
using OtpNet;
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

namespace IoTSuper_DesktopApp.Vistas.Cliente
{
    /// <summary>
    /// Lógica de interacción para CarruselCentro.xaml
    /// </summary>
    public partial class CarruselCentro : UserControl
    {
        private int _indiceCarusel = 1;

        private TarjetaCentro _tarjetaIzquierda;
        private TarjetaCentro _tarjetaCentro;
        private TarjetaCentro _tarjetaDerecha;

        public CarruselCentro()
        {
            InitializeComponent();
            this.Loaded += CarruselCentro_Loaded;
        }

        private async void CarruselCentro_Loaded(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Cargando carrusel de centros...");
            if (Sesion._centros == null || Sesion._centros.Count == 0) { btnDerecha.Visibility = Visibility.Collapsed; btnIzquierda.Visibility = Visibility.Collapsed; return; }
            LogLocal.logear($"");

            if (Sesion._centros.Count == 1)
            {
                LogLocal.logear($"Mostrando un solo centro en el carrusel.");
                _tarjetaCentro = new TarjetaCentro(Sesion._centros[0]);

                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if(Sesion._centros.Count == 2)
            {
                LogLocal.logear($"Mostrando 2 centros en el carrusel.");
                _tarjetaIzquierda = new TarjetaCentro(Sesion._centros[0]);
                _tarjetaCentro = new TarjetaCentro(Sesion._centros[1]);

                _tarjetaIzquierda.Height = 450;
                _tarjetaIzquierda.Width = 350;
                _tarjetaIzquierda.Opacity = 0.7;
                _tarjetaIzquierda.IsHitTestVisible = false;
                _tarjetaIzquierda.brdImagen.Height = 250;

                ContenedorCarusel.Children.Add(_tarjetaIzquierda);
                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if (Sesion._centros.Count > 2)
            {
                LogLocal.logear($"Mostrando {Sesion._centros.Count} centros en el carrusel.");
                _tarjetaIzquierda = new TarjetaCentro(Sesion._centros[0]);
                _tarjetaCentro = new TarjetaCentro(Sesion._centros[1]);
                _tarjetaDerecha = new TarjetaCentro(Sesion._centros[2]);
                _tarjetaIzquierda.Height = 450;
                _tarjetaIzquierda.Width = 350;
                _tarjetaIzquierda.Opacity = 0.7;
                _tarjetaIzquierda.IsHitTestVisible = false;
                _tarjetaIzquierda.brdImagen.Height = 250;

                _tarjetaDerecha.Height = 450;
                _tarjetaDerecha.Width = 350;
                _tarjetaDerecha.Opacity = 0.7;
                _tarjetaDerecha.IsHitTestVisible = false;
                _tarjetaDerecha.brdImagen.Height = 250;

                ContenedorCarusel.Children.Add(_tarjetaIzquierda);
                ContenedorCarusel.Children.Add(_tarjetaCentro);
                ContenedorCarusel.Children.Add(_tarjetaDerecha);
            }
        }

        public void ActualizarCarrusel()
        {
            int total = Sesion._centros.Count;

            int izq = (_indiceCarusel - 1 + total) % total;
            int centro = _indiceCarusel;
            int der = (_indiceCarusel + 1) % total;

            _tarjetaIzquierda?.ActualizarCentro(Sesion._centros[izq]);
            _tarjetaCentro?.ActualizarCentro(Sesion._centros[centro]);
            _tarjetaDerecha?.ActualizarCentro(Sesion._centros[der]);
        }

        private void CrearCentroView_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Creando un nuevo centro...");
            Navegacion.IrA(new FormularioCentroControl());
        }

        private void btnDerecha_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Moviendo carrusel a la derecha...");
            _indiceCarusel = (_indiceCarusel - 1 + Sesion._centros.Count) % Sesion._centros.Count;
            ActualizarCarrusel();
        }

        private void btnIzquierda_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Moviendo carrusel a la izquierda...");
            _indiceCarusel = (_indiceCarusel + 1) % Sesion._centros.Count;
            ActualizarCarrusel();
        }

        private void BuscarCentroView_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Buscando centro con el texto: {txtBusqueda.Texto}");
            try
            {
                _indiceCarusel = Sesion._centros.IndexOf(Sesion._centros.Find(c => c.Nombre.ToLower().Contains(txtBusqueda.Texto.ToLower())));

                int total = Sesion._centros.Count;

                int izq = (_indiceCarusel - 1 + total) % total;
                int centro = _indiceCarusel;
                int der = (_indiceCarusel + 1) % total;

                _tarjetaIzquierda?.ActualizarCentro(Sesion._centros[izq]);
                _tarjetaCentro?.ActualizarCentro(Sesion._centros[centro]);
                _tarjetaDerecha?.ActualizarCentro(Sesion._centros[der]);
            }
            catch (Exception ex)
            {
                LogLocal.logear($"No se encontró ningún centro que coincida con la búsqueda.");
                MessageBox.Show("No se encontró ningún centro que coincida con la búsqueda.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
