using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Controladores;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Servicios.Centro;
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
        private List<Modelos.CentroDTO> _centros;

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
            _centros = await CentroService.ObtenerCentros(Sesion.LoginData.IdCliente);

            if(_centros == null || _centros.Count == 0) { btnDerecha.Visibility = Visibility.Collapsed; btnIzquierda.Visibility = Visibility.Collapsed; return; }

            if(_centros.Count == 1)
            {
                _tarjetaCentro = new TarjetaCentro(_centros[0]);

                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if(_centros.Count == 2)
            {
                _tarjetaIzquierda = new TarjetaCentro(_centros[0]);
                _tarjetaCentro = new TarjetaCentro(_centros[1]);

                _tarjetaIzquierda.Height = 450;
                _tarjetaIzquierda.Width = 350;
                _tarjetaIzquierda.Opacity = 0.7;
                _tarjetaIzquierda.IsHitTestVisible = false;
                _tarjetaIzquierda.brdImagen.Height = 250;

                ContenedorCarusel.Children.Add(_tarjetaIzquierda);
                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if (_centros.Count > 2)
            {
                _tarjetaIzquierda = new TarjetaCentro(_centros[0]);
                _tarjetaCentro = new TarjetaCentro(_centros[1]);
                _tarjetaDerecha = new TarjetaCentro(_centros[2]);

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
            int total = _centros.Count;

            int izq = (_indiceCarusel - 1 + total) % total;
            int centro = _indiceCarusel;
            int der = (_indiceCarusel + 1) % total;

            _tarjetaIzquierda?.ActualizarCentro(_centros[izq]);
            _tarjetaCentro?.ActualizarCentro(_centros[centro]);
            _tarjetaDerecha?.ActualizarCentro(_centros[der]);
        }

        private void CrearCentroView_Click(object sender, RoutedEventArgs e)
        {
            Navegacion.IrA(new FormularioCentroControl());
        }

        private void btnDerecha_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _indiceCarusel = (_indiceCarusel - 1 + _centros.Count) % _centros.Count;
            ActualizarCarrusel();
        }

        private void btnIzquierda_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _indiceCarusel = (_indiceCarusel + 1) % _centros.Count;
            ActualizarCarrusel();
        }
    }
}
