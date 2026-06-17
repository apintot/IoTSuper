using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Servicios.Componente;
using IoTSuper_DesktopApp.Servicios.Seccion;
using IoTSuper_DesktopApp.Vistas.Cliente;
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

namespace IoTSuper_DesktopApp.Controladores
{
    /// <summary>
    /// Lógica de interacción para TarjetaSeccion.xaml
    /// </summary>
    public partial class TarjetaSeccion : UserControl
    {
        private SeccionDTO _seccion;
        private List<ComponenteDTO> componentes;

        public TarjetaSeccion(SeccionDTO seccion)
        {
            InitializeComponent();

            ActualizarSeccion(seccion);
            _seccion = seccion;

            this.Loaded += TarjetaSeccion_Loaded;
        }

        private async void TarjetaSeccion_Loaded(object sender, RoutedEventArgs e)
        {
            componentes = await ComponenteService.ObtenerComponentesSeccion(_seccion.IdSeccion);
            txbNumeroSensores.Text = componentes.Count().ToString();
            _seccion.NumComponentes = componentes.Count();
        }

        public async void ActualizarSeccion(SeccionDTO seccion)
        {
            _seccion = seccion;

            txbTituloSeccion.Text = _seccion.Nombre;

            txbNumeroSensores.Text = _seccion.NumComponentes.ToString();

            if (string.IsNullOrWhiteSpace(_seccion.Imagen))
            {
                ImgSeccion.Source = new BitmapImage(new Uri("pack://application:,,,/Estilos/Imagenes/ImagenDefecto.png"));
                ImgSeccion.Height = 100;
                ImgSeccion.Stretch = Stretch.Uniform;
            }
            else
            {
                ImgSeccion.Source = new BitmapImage(new Uri(_seccion.Imagen));
                ImgSeccion.Stretch = Stretch.UniformToFill;
                ImgSeccion.Height = double.NaN;
            }
        }

        private void VerSecciones_Click(object sender, RoutedEventArgs e)
        {
            Sesion.seccionSelecionado = this._seccion.IdSeccion;
            Navegacion.IrA(new ComponeneViewControl(this._seccion));
        }

        private void OtrasOpciones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stkOpciones.Opacity = stkOpciones.Opacity == 0 ? 1 : 0;
            stkOpciones.Visibility = stkOpciones.Opacity == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnEditarSeccion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Navegacion.IrA(new FormularioSeccionControl(_seccion));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Editar seccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEliminarSeccion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                await SeccionService.EliminarSeccion(_seccion.IdSeccion);
                Navegacion.IrA(new CarruselSeccion(await CentroService.ObtenerSeccionesCentro(_seccion.IdCentro), _seccion.IdCentro));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Eliminar seccion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
