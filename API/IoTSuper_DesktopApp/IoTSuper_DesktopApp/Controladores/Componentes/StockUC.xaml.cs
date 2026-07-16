using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Componente;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace IoTSuper_DesktopApp.Controladores.Componentes
{
    /// <summary>
    /// Lógica de interacción para StockUC.xaml
    /// </summary>
    public partial class StockUC : UserControl
    {
        public ComponenteDTO data;

        private bool esNuevo = true;

        public StockUC(bool posicionInvertida = false)
        {
            InitializeComponent();

            data = new ComponenteDTO();
            data.Stock = new StockDTO();

            if (posicionInvertida)
            {
                grdSecundarySTK.Margin = new Thickness(0, -580, 0, 0);
            }

            data = new ComponenteDTO();

            grdSecundarySTK.Visibility = Visibility.Collapsed;

            imgBorrar.IsHitTestVisible = false;
            imgBorrar.Opacity = 0.5;
        }

        public StockUC(ComponenteDTO _data)
        {
            InitializeComponent();

            if (_data.Stock == null) { return; }

            imgEstado.Source = Sesion.Componentes.FirstOrDefault(c => c.IdComponente == _data.IdComponente).Estado.Equals("OK") ?
                new BitmapImage(new Uri("pack://application:,,,/Estilos/Iconos/verde.png", UriKind.Absolute)) : new BitmapImage(new Uri("pack://application:,,,/Estilos/Iconos/rojo.png", UriKind.Absolute));

            data = _data;

            txbNombre.IsEnabled = false;

            txbNombre.Text = txbSTK.Text = data.Nombre;
            txbTopic.Text = data.Topic;

            txbPeso.Text = data.Stock?.Peso_Unidad.ToString();
            txbStcMax.Text = data.Stock?.Stock_Maximo.ToString();
            txbStcMin.Text = data.Stock?.Stock_Minimo.ToString();
            txbEmail.Text = data.Stock?.EmailEmergencia;

            Debug.WriteLine($"Inicio  X: {data.PosicionX} Y: {data.PosicionY}");

            esNuevo = false;
            grdSecundarySTK.Visibility = Visibility.Collapsed;
        }

        public void actualizarPosicion(double x, double y)
        {
            data.PosicionX = x;
            data.PosicionY = y;

            Debug.WriteLine($"Final X: {x} Y: {y}");
        }

        public void cambiarPosicionDelMenu(bool cambio)
        {
            if (cambio)
            {
                grdSecundarySTK.Margin = new Thickness(0, -580, 0, 0);
            }
            else
            {
                grdSecundarySTK.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void mgrdMainSTK_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdSecundarySTK.Visibility = Visibility.Visible;
        }

        private void mgrdMainSTK_MouseEnter(object sender, MouseEventArgs e)
        {
            imgStock.Opacity = 0.5;
        }

        private void mgrdMainSTK_MouseLeave(object sender, MouseEventArgs e)
        {
            imgStock.Opacity = 1;
        }

        private async void Basura_MouseLeftButtonUpAsync(object sender, MouseButtonEventArgs e)
        {
            ErrorDTO respuesta = await ComponenteService.EliminarComponente(data.IdComponente);

            if (respuesta == null || respuesta.Status != 200) { return; }//popup

            this.Visibility = Visibility.Collapsed;

            int indiceCentro = Sesion._centros.FindIndex(c => c.IdCentro == Sesion.centroSelecionado);
            int indiceSeccion = Sesion._centros[indiceCentro]._secciones.FindIndex(s => s.IdSeccion == Sesion.seccionSelecionado);

            Sesion._centros[indiceCentro]._secciones[indiceSeccion]._componentes.Remove(data);
            Sesion.Componentes.Remove(Sesion.Componentes.Where(c => c.IdComponente == data.IdComponente).First());
        }

        private void Volver_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.grdSecundarySTK.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }

        private async void Save_MouseLeftButtonUpAsync(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(txbNombre.Text))
            {
                txbNombre.Background = Brushes.OrangeRed;
                return;
            }

            data.Nombre = txbSTK.Text = txbNombre.Text;

            if (string.IsNullOrEmpty(txbTopic.Text))
            {
                txbNombre.Background = Brushes.OrangeRed;
                return;
            }

            data.Topic = txbTopic.Text;
            data.IdSeccion = Sesion.seccionSelecionado;

            if (data.Stock is null) { data.Stock = new StockDTO(); }

            data.Stock.Peso_Unidad = String.IsNullOrEmpty(txbPeso.Text) ? 0 : double.Parse(txbPeso.Text);
            data.Stock.Stock_Maximo = String.IsNullOrEmpty(txbStcMax.Text) ? 0 : int.Parse(txbStcMax.Text);
            data.Stock.Stock_Minimo = String.IsNullOrEmpty(txbStcMin.Text) ? 0 : int.Parse(txbStcMin.Text);
            data.Stock.EmailEmergencia = String.IsNullOrEmpty(txbEmail.Text) ? String.Empty : txbEmail.Text;

            if (esNuevo)
            {
                ErrorDTO respuesta = await ComponenteService.CrearComponente(data);

                if (respuesta == null || respuesta.Status != 200) { return; }//popup

                data.IdComponente = int.Parse(respuesta.Errors["Id"][0]);

                int indiceSeccion = Sesion._centros[Sesion.centroSelecionado]._secciones.FindIndex(s => s.IdSeccion == Sesion.seccionSelecionado);

                Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion]._componentes.Add(data);

                Sesion.Componentes.Add(ComponenteToResumen.ConvierteAResumenDTO(data, Sesion._centros[Sesion.centroSelecionado].Nombre, Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion].Nombre));

                txbNombre.IsEnabled = false;
            }
            else
            {
                ErrorDTO respuesta = await ComponenteService.ActualizarComponente(data);
                if (respuesta == null || respuesta.Status != 200) { return; }//popup
            }

            imgBorrar.IsHitTestVisible = true;
            imgBorrar.Opacity = 1;

            //Sesion.publicarMensajeMqtt($"STK/OUTPUT/{data.Topic}", $"{data.Etiqueta.Frase1}|{data.Etiqueta.Frase2}|{data.Etiqueta.Frase3}|{data.Etiqueta.Frase4}|");

            grdSecundarySTK.Visibility = Visibility.Collapsed;
        }
    }
}
