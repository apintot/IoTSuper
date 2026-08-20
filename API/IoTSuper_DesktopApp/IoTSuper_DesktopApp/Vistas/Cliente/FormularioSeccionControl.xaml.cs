using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Servicios.Seccion;
using Microsoft.Win32;
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


    public partial class FormularioSeccionControl : UserControl
    {
        private SeccionDTO _seccion = new SeccionDTO();

        private int _idCentro;

        public FormularioSeccionControl(int idCentro)
        {
            InitializeComponent();

            _idCentro = idCentro;

            this.Loaded += FormularioSeccionControl_Loaded;
        }

        public FormularioSeccionControl(SeccionDTO seccion)
        {
            InitializeComponent();

            _seccion = seccion;

            this.Loaded += FormularioSeccionControl_Loaded;
        }

        private void FormularioSeccionControl_Loaded(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"FormularioSeccionControl cargado para la sección {_seccion.Nombre}.");
            if (_seccion.IdSeccion != 0)
            {
                LogLocal.logear($"Cargando sección {_seccion.Nombre} para edición.");
                camNombre.Texto = _seccion.Nombre;
                btnCrearSeccion.Visibility = Visibility.Collapsed;
                btnEditarSeccion.Visibility = Visibility.Visible;
            }
            else
            {
                LogLocal.logear($"Cargando sección {_seccion.Nombre} para creación.");
                btnCrearSeccion.Visibility = Visibility.Visible;
                btnEditarSeccion.Visibility = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(_seccion.Imagen))
            {
                LogLocal.logear($"Cargando imagen de la sección {_seccion.Nombre}.");
                imgCentro.Source = new BitmapImage(new Uri(_seccion.Imagen));
                imgCentro.Stretch = Stretch.UniformToFill;
                imgCentro.Width = double.NaN;
                imgCentro.Height = double.NaN;
            }
        }

        private async void btnCrearSeccion_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Creando sección {_seccion.Nombre}.");
            _seccion.Nombre = camNombre.Texto;
            _seccion.IdCentro = _idCentro;

            ErrorDTO errores = await SeccionService.GuardarSeccion(_seccion);

            if (errores != null && errores.Status == 200) 
            {
                LogLocal.logear($"Sección {_seccion.Nombre} creada correctamente.");
                int posicion = Sesion._centros.FindIndex(c => c.IdCentro == _seccion.IdCentro);
                _seccion.IdSeccion = int.Parse(errores.Errors.Values.FirstOrDefault()?.FirstOrDefault());

                if(Sesion._centros[posicion]?._secciones is null)
                {
                    Sesion._centros[posicion]?._secciones = new List<SeccionDTO>();
                }

                Sesion._centros[posicion]?._secciones?.Add(_seccion); 
                Navegacion.IrA(new CarruselSeccion(await CentroService.ObtenerSeccionesCentro(_seccion.IdCentro), _seccion.IdCentro)); 
            }

            MostrarErrores(errores);
        }

        private async void btnEditarSeccion_Click(object sender, RoutedEventArgs e)
        {
            LogLocal.logear($"Actualizando sección {_seccion.Nombre}.");
            _seccion.Nombre = camNombre.Texto;

            ErrorDTO errores = await SeccionService.ActualizarSeccion(_seccion);

            if (errores != null && errores.Status == 200) { LogLocal.logear($"Sección {_seccion.Nombre} actualizada correctamente."); Navegacion.IrA(new CarruselSeccion(await CentroService.ObtenerSeccionesCentro(_seccion.IdCentro), _seccion.IdCentro)); }

            MostrarErrores(errores);
        }

        private void MostrarErrores(ErrorDTO errores)
        {
            foreach (KeyValuePair<string, List<string>> error in errores.Errors)
            {
                if (error.Key.Equals("Nombre"))
                {
                    LogLocal.logear($"Error al actualizar la sección {_seccion.Nombre}: {string.Join(Environment.NewLine, error.Value)}");
                    txbErrorNombre.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorNombre.Visibility = Visibility.Visible;
                }
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Seleccionando imagen para la sección {_seccion.Nombre}.");
            OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Seleccionar imagen",
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true) return;

            string origen = dialog.FileName;
            string nombreArchivo = System.IO.Path.GetFileName(origen);

            System.IO.Directory.CreateDirectory(Rutas.ImagesFolder);

            string destino = System.IO.Path.Combine(Rutas.ImagesFolder, nombreArchivo);

            if (System.IO.File.Exists(destino))
            {
                string nombre = System.IO.Path.GetFileNameWithoutExtension(origen);
                string extension = System.IO.Path.GetExtension(origen);
                destino = System.IO.Path.Combine(Rutas.ImagesFolder, $"{nombre}_{DateTime.Now:yyyyMMddHHmmss}{extension}");
            }

            System.IO.File.Copy(origen, destino);

            _seccion.Imagen = destino;

            imgCentro.Source = new BitmapImage(new Uri(destino));
            imgCentro.Stretch = Stretch.UniformToFill;
            imgCentro.Width = double.NaN;
            imgCentro.Height = double.NaN;

            LogLocal.logear($"Imagen de la sección {_seccion.Nombre} actualizada correctamente.");

            RClone.RClone.SubirImagenesAlServidorAsync();
        }
    }
}
