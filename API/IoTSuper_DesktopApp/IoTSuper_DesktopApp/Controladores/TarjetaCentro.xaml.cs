using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Servicios.Componente;
using IoTSuper_DesktopApp.Vistas.Cliente;
using System.IO;
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

            LogLocal.logear($"Cargando {_centro._secciones?.Count ?? 0} secciones.");

            if (!(_centro._secciones is null))
            {
                foreach (SeccionDTO seccion in _centro._secciones)
                {
                    if (seccion._componentes is null) continue;
                    //_centro.numeroComponentes += seccion._componentes.Count;
                }
            }
            else
            {
                txbNumeroSecciones.Text = 0.ToString();
            }

            //txbNumeroSensores.Text = _centro.numeroComponentes.ToString();
        }

        public async void ActualizarCentro(CentroDTO centro)
        {
            _centro = centro;

            LogLocal.logear($"Actualizando tarjeta del centro {_centro.Nombre}");

            txbTituloCentro.Text = centro.Nombre;

            txbNumeroSensores.Text = _centro._secciones?.Sum(s => s._componentes?.Count ?? 0).ToString();

            if (string.IsNullOrWhiteSpace(centro.Imagen))
            {
                ImgCentro.Source = new BitmapImage(new Uri("pack://application:,,,/Estilos/Imagenes/ImagenDefecto.png"));
                ImgCentro.Height = 100;
                ImgCentro.Stretch = Stretch.Uniform;
            }
            else
            {
                if(File.Exists(Rutas.ImagesFolder + "\\" + centro.Imagen))
                {
                    ImgCentro.Source = new BitmapImage(new Uri(Rutas.ImagesFolder + "\\" + centro.Imagen));
                } 
                else
                {
                    ImgCentro.Source = new BitmapImage(new Uri(centro.Imagen));
                }
                ImgCentro.Stretch = Stretch.UniformToFill;
                ImgCentro.Height = double.NaN;
            }

            _centro._secciones = centro._secciones;

            if (_centro._secciones == null) { LogLocal.logear($"El centro {_centro.Nombre} no tiene secciones."); txbNumeroSecciones.Text = "0"; return; }

            txbNumeroSecciones.Text = _centro._secciones.Count.ToString();
        }

        private void VerSecciones_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Mostrando secciones del centro {_centro.Nombre}");
            //if(_centro._secciones == null || _centro._secciones.Count == 0) { Navegacion.IrA(new FormularioCentroControl()); }
            Sesion.centroSelecionado = Sesion._centros.FindIndex(x => x.IdCentro == _centro.IdCentro);
            Navegacion.IrA(new CarruselSeccion(_centro._secciones, _centro.IdCentro));
        }

        private void OtrasOpciones_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Mostrando/Ocultando opciones del centro {_centro.Nombre}");
            stkOpciones.Opacity = stkOpciones.Opacity == 0 ? 1 : 0;
            stkOpciones.Visibility = stkOpciones.Opacity == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void BtnEditarCentro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                LogLocal.logear($"Editando centro {_centro.Nombre}");
                Navegacion.IrA(new FormularioCentroControl(_centro));
            }
            catch (Exception ex)
            {
                LogLocal.logear($"Error al editar centro {_centro.Nombre}: {ex.Message}");
                MessageBox.Show(ex.Message, "Error al Editar centro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnEliminarCentro_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                LogLocal.logear($"Eliminando centro {_centro.Nombre}");
                await CentroService.EliminarCentro(this._centro.IdCentro);
                Sesion._centros.Remove(this._centro);
                Navegacion.IrA(new CarruselCentro());
            }
            catch (Exception ex) 
            {
                LogLocal.logear($"Error al eliminar centro {_centro.Nombre}: {ex.Message}");
                MessageBox.Show(ex.Message, "Error al Eliminar centro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
