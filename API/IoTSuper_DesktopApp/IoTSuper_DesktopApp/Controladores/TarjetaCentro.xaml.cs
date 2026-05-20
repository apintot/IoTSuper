using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Vistas.Cliente;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IoTSuper_DesktopApp.Controladores
{
    /// <summary>
    /// Lógica de interacción para TarjetaCentro.xaml
    /// </summary>
    public partial class TarjetaCentro : UserControl
    {
        private CentroDTO _centro;
        private List<Modelos.SeccionDTO> _secciones;

        public TarjetaCentro(CentroDTO centro)
        {
            InitializeComponent();

            ActualizarCentro(centro);;
        }

        public async void ActualizarCentro(CentroDTO centro)
        {
            _centro = centro;

            txbTituloCentro.Text = centro.Nombre;

            if (string.IsNullOrWhiteSpace(centro.Imagen))
            {
                ImgCentro.Source = new BitmapImage(new Uri("pack://application:,,,/Estilos/Imagenes/ImagenDefecto.png"));
                ImgCentro.Height = 100;
                ImgCentro.Stretch = Stretch.Uniform;
            }
            else
            {
                ImgCentro.Source = new BitmapImage(new Uri(centro.Imagen));
                ImgCentro.Stretch = Stretch.UniformToFill;
                ImgCentro.Height = double.NaN;
            }

            _secciones = await CentroService.ObtenerSeccionesCentro(_centro.IdCentro);

            if (_secciones == null) { txbNumeroSecciones.Text = "0"; return; }

            txbNumeroSecciones.Text = _secciones.Count.ToString();
        }

        private void VerSecciones_Click(object sender, RoutedEventArgs e)
        {
            if(_secciones == null || _secciones.Count == 0) { return; }
            Navegacion.IrA(new CarruselSeccion(_secciones, _centro.IdCentro));
        }

        private void OtrasOpciones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            stkOpciones.Opacity = stkOpciones.Opacity == 0 ? 1 : 0;
            stkOpciones.Visibility = stkOpciones.Opacity == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnEditarCentro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Navegacion.IrA(new FormularioCentroControl(_centro));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al Editar centro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEliminarCentro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                await CentroService.EliminarCentro(this._centro.IdCentro);
                Navegacion.IrA(new CarruselCentro());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error al Eliminar centro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
