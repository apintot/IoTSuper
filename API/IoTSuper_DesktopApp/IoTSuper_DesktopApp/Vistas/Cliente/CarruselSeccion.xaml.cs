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
    /// Lógica de interacción para CarruselSeccion.xaml
    /// </summary>
    public partial class CarruselSeccion : UserControl
    {
        private List<Modelos.SeccionDTO> _secciones;

        private int _indiceCarusel = 1;

        private TarjetaSeccion _tarjetaIzquierda;
        private TarjetaSeccion _tarjetaCentro;
        private TarjetaSeccion _tarjetaDerecha;

        private int _idCentro;

        public CarruselSeccion(List<Modelos.SeccionDTO> secciones, int idCentro)
        {
            InitializeComponent();
            this.Loaded += CarruselSeccion_Loaded;

            LogLocal.logear($"Cargando carrusel de secciones para el centro {_idCentro}...");

            _idCentro = idCentro;
            _secciones = secciones;
        }

        private async void CarruselSeccion_Loaded(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Cargando carrusel de secciones para el centro {_idCentro}...");

            if (_secciones == null) { btnDerecha.Visibility = Visibility.Collapsed; btnIzquierda.Visibility = Visibility.Collapsed; return; }

            if (_secciones.Count == 1)
            {
                LogLocal.logear($"Mostrando un solo sección en el carrusel.");

                _tarjetaCentro = new TarjetaSeccion(_secciones[0]);
                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if (_secciones.Count == 2)
            {
                LogLocal.logear($"Mostrando 2 secciones en el carrusel.");

                _tarjetaIzquierda = new TarjetaSeccion(_secciones[0]);
                _tarjetaCentro = new TarjetaSeccion(_secciones[1]);

                _tarjetaIzquierda.Height = 450;
                _tarjetaIzquierda.Width = 350;
                _tarjetaIzquierda.Opacity = 0.7;
                _tarjetaIzquierda.IsHitTestVisible = false;
                _tarjetaIzquierda.brdImagen.Height = 250;

                ContenedorCarusel.Children.Add(_tarjetaIzquierda);
                ContenedorCarusel.Children.Add(_tarjetaCentro);
            }

            if (_secciones.Count > 2)
            {
                LogLocal.logear($"Mostrando {_secciones.Count} secciones en el carrusel.");

                _tarjetaIzquierda = new TarjetaSeccion(_secciones[0]);
                _tarjetaCentro = new TarjetaSeccion(_secciones[1]);
                _tarjetaDerecha = new TarjetaSeccion(_secciones[2]);
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
            int total = _secciones.Count;

            int izq = (_indiceCarusel - 1 + total) % total;
            int centro = _indiceCarusel;
            int der = (_indiceCarusel + 1) % total;

            _tarjetaIzquierda?.ActualizarSeccion(_secciones[izq]);
            _tarjetaCentro?.ActualizarSeccion(_secciones[centro]);
            _tarjetaDerecha?.ActualizarSeccion(_secciones[der]);
        }

        private void CrearSeccionView_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Creando una nueva sección...");

            Navegacion.IrA(new FormularioSeccionControl(_idCentro));
        }

        private void btnDerecha_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Moviendo carrusel a la derecha...");

            _indiceCarusel = (_indiceCarusel - 1 + _secciones.Count) % _secciones.Count;
            ActualizarCarrusel();
        }

        private void btnIzquierda_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Moviendo carrusel a la izquierda...");

            _indiceCarusel = (_indiceCarusel + 1) % _secciones.Count;
            ActualizarCarrusel();
        }

        private void BuscarSeccionView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogLocal.logear($"Buscando sección con el texto: {txtBusqueda.Texto}");

                _indiceCarusel = _secciones.IndexOf(_secciones.Find(s => s.Nombre.ToLower().Contains(txtBusqueda.Texto.ToLower())));

                int total = _secciones.Count;

                int izq = (_indiceCarusel - 1 + total) % total;
                int centro = _indiceCarusel;
                int der = (_indiceCarusel + 1) % total;

                _tarjetaIzquierda?.ActualizarSeccion(_secciones[izq]);
                _tarjetaCentro?.ActualizarSeccion(_secciones[centro]);
                _tarjetaDerecha?.ActualizarSeccion(_secciones[der]);
            }
            catch (Exception ex) 
            {
                LogLocal.logear($"No se encontró ninguna sección que coincida con la búsqueda.");
                MessageBox.Show("No se encontró ninguna sección que coincida con la búsqueda.", "Información", MessageBoxButton.OK, MessageBoxImage.Information); 
            }
        }
    }
}
