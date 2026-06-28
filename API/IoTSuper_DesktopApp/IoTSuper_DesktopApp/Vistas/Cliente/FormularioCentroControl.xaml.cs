using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Centro;
using IoTSuper_DesktopApp.Vistas.Administrador;
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
using static QRCoder.SvgQRCode;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IoTSuper_DesktopApp.Vistas.Cliente
{
    /// <summary>
    /// Lógica de interacción para FormularioCentroControl.xaml
    /// </summary>
    public partial class FormularioCentroControl : UserControl
    {
        private List<PaisesDTO> _paises;

        private Dictionary<int, string> _tipologias;

        private ProvinciaDTO _provincia;

        private CentroDTO _centro = new CentroDTO();

        string rutaIamgen = string.Empty;

        public FormularioCentroControl()
        {
            InitializeComponent();

            this.Loaded += FormularioCentroControl_Loaded;

            this.cmbPais.SelectionChanged += CmbPais_SelectionChanged;
        }

        public FormularioCentroControl(CentroDTO centro)
        {
            InitializeComponent();

            _centro = centro;

            this.Loaded += FormularioCentroControl_Loaded;

            this.cmbPais.SelectionChanged += CmbPais_SelectionChanged;
        }

        private async void FormularioCentroControl_Loaded(object sender, RoutedEventArgs e)
        {
            _paises = await CentroService.ObtenerPaises();

            foreach (PaisesDTO paisDTO in _paises)
            {
                cmbPais.Items.Add(paisDTO.name.common);
            }

            _tipologias = await CentroService.ObtenerTipologias();

            foreach(KeyValuePair<int, string> tipologia in _tipologias)
            {
                cmbTipologia.Items.Add(tipologia.Value);
            }

            if (_centro.IdCentro != 0)
            {
                btnCrearCentro.Visibility = Visibility.Collapsed;
                btnEditarCentro.Visibility = Visibility.Visible;

                camEmpresa.Texto = _centro.RazonSocial;
                camCIF.Texto = _centro.Cif;
                camNombre.Texto = _centro.Nombre;
                camDireccion.Texto = _centro.Localizacion.Direccion;
                camCodigoPostal.Texto = _centro.Localizacion.CodigoPostal;

                if (!string.IsNullOrEmpty(_centro.Imagen))
                {
                    imgCentro.Source = new BitmapImage(new Uri(_centro.Imagen));
                    imgCentro.Stretch = Stretch.UniformToFill;
                    imgCentro.Width = double.NaN;
                    imgCentro.Height = double.NaN;
                }

                cmbPais.SelectedValue = _centro.Localizacion.Pais;
                cmbProvincia.SelectedValue = _centro.Localizacion.provincia;
                cmbTipologia.SelectedValue = _tipologias.FirstOrDefault(x => x.Key.Equals(_centro.IdTipologia)).Value;
            }
        }

        private async void CmbPais_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbProvincia.Items.Clear();

            if(cmbPais.SelectedItem == null) { return; }

            _centro.Localizacion.Pais = cmbPais.SelectedItem.ToString() ?? string.Empty;

            _provincia = await CentroService.ObtenerProvincia(_centro.Localizacion.Pais);

            if (_provincia.data == null) { return; }

            foreach (State provinciaDTO in _provincia.data.states)
            {
                if(provinciaDTO.name.Contains("Province") && _centro.Localizacion.Pais.Equals("Spain"))
                    cmbProvincia.Items.Add(provinciaDTO.name.Replace("Province", "").Trim());
            }

            stkProvincia.Opacity = 1;
            stkProvincia.IsEnabled = true;
        }

        private async void CrearCentro_Click(object sender, RoutedEventArgs e)
        {
            OcultarTextoError();

            GuardarDatosCentro();

            ErrorDTO errores = await CentroService.GuardarCentro(_centro);

            if (errores != null && errores.Status == 200) { Navegacion.IrA(new CarruselCentro()); }

            MostrarErrores(errores);

        }

        private void GuardarDatosCentro()
        {
            _centro.Localizacion.provincia = cmbProvincia.SelectedItem is null ? string.Empty : cmbProvincia.SelectedItem.ToString();
            _centro.Localizacion.CodigoPostal = camCodigoPostal.Texto;
            _centro.Localizacion.Direccion = camDireccion.Texto;

            try
            {
                _centro.IdTipologia = _tipologias.FirstOrDefault(x => x.Value.Equals(cmbTipologia.SelectedItem.ToString())).Key;
            }
            catch (Exception ex)
            {
                this.txbErrorTipologia.Text = "Seleccione una tipología!";
                this.txbErrorTipologia.Visibility = Visibility.Visible;
                return;
            }


            _centro.IdCliente = Sesion.LoginData.IdCliente;

            _centro.RazonSocial = camEmpresa.Texto;
            _centro.Cif = camCIF.Texto;
            _centro.Nombre = camNombre.Texto;
        }

        private void MostrarErrores(ErrorDTO errores)
        {
            foreach (KeyValuePair<string, List<string>> error in errores.Errors)
            {
                if (error.Key.Equals("Cif"))
                {
                    txbErrorCIF.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorCIF.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("RazonSocial"))
                {
                    txbErrorEmpresa.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorEmpresa.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Nombre"))
                {
                    txbErrorNombre.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorNombre.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Localizacion.Direccion"))
                {
                    txbErrorDireccion.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorDireccion.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Empresa"))
                {
                    txbErrorEmpresa.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorEmpresa.Visibility = Visibility.Visible;
                }
                else if (error.Key.Equals("Localizacion.CodigoPostal"))
                {
                    txbErrorCodigoPostal.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorCodigoPostal.Visibility = Visibility.Visible;
                }
                else if(error.Key.Equals("Localizacion.Pais"))
                {
                    txbErrorPais.Text = string.Join(Environment.NewLine, error.Value);
                    txbErrorPais.Visibility = Visibility.Visible;
                }
            }
        }

        public void OcultarTextoError()
        {
            this.txbErrorTipologia.Visibility = txbErrorCodigoPostal.Visibility = txbErrorCIF.Visibility = txbErrorNombre.Visibility = txbErrorDireccion.Visibility = txbErrorEmpresa.Visibility = txbErrorTipologia.Visibility = Visibility.Hidden;
        }

        private void SeleccionarImagen_Click(object sender, MouseButtonEventArgs e)
        {
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

            _centro.Imagen = destino;

            imgCentro.Source = new BitmapImage(new Uri(destino));
            imgCentro.Stretch = Stretch.UniformToFill;
            imgCentro.Width = double.NaN;
            imgCentro.Height = double.NaN;

            RClone.RClone.SubirImagenesAlServidorAsync();
        }

        private async void EditarCentro_Click(object sender, RoutedEventArgs e)
        {
            OcultarTextoError();

            GuardarDatosCentro();

            ErrorDTO errores = await CentroService.EditarCentro(_centro);

            if (errores != null && errores.Status == 200) { Navegacion.IrA(new CarruselCentro()); }

            MostrarErrores(errores);
        }
    }
}
