using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Servicios.Componente;
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

        public TarjetaCentro(CentroDTO centro)
        {
            InitializeComponent();

            txbNumeroSensores.Text = 0.ToString();

            this.Loaded += TarjetaCentro_Loaded;

            ActualizarCentro(centro);
        }

        private async void TarjetaCentro_Loaded(object sender, RoutedEventArgs e)
        {
            _centro._secciones = Sesion._centros.Where(c => c.IdCentro == _centro.IdCentro).FirstOrDefault()?._secciones;
            txbNumeroSecciones.Text = _centro._secciones?.Count.ToString();
            _centro.numeroComponentes = 0;

            foreach (SeccionDTO seccion in _centro._secciones)
            {
                _centro.numeroComponentes += seccion._componentes.Count;
            }

            txbNumeroSensores.Text = _centro.numeroComponentes.ToString();
        }

        public async void ActualizarCentro(CentroDTO centro)
        {
            _centro = centro;

            txbTituloCentro.Text = centro.Nombre;

            txbNumeroSensores.Text = _centro.numeroComponentes.ToString();

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

            _centro._secciones = centro._secciones;

            if (_centro._secciones == null) { txbNumeroSecciones.Text = "0"; return; }

            txbNumeroSecciones.Text = _centro._secciones.Count.ToString();
        }

        private void VerSecciones_Click(object sender, RoutedEventArgs e)
        {
            if(_centro._secciones == null || _centro._secciones.Count == 0) { return; }
            Sesion.centroSelecionado = _centro.IdCentro;
            Navegacion.IrA(new CarruselSeccion(_centro._secciones, _centro.IdCentro));
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
                Sesion._centros.Remove(this._centro);
                Navegacion.IrA(new CarruselCentro());
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error al Eliminar centro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
