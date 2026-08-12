using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Componente;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace IoTSuper_DesktopApp.Controladores.Componentes
{
    /// <summary>
    /// Lógica de interacción para DisplayUC.xaml
    /// </summary>
    public partial class DisplayUC : UserControl
    {
        public ComponenteDTO data;

        private bool esNuevo = true;

        public DisplayUC(bool posicionInvertida = false)
        {
            InitializeComponent();

            data = new ComponenteDTO();
            data.Etiqueta = new EtiquetaDTO();

            if (posicionInvertida)
            {
                grdSecundaryLCD.Margin = new Thickness(0, -580, 0, 0);
            }

            data = new ComponenteDTO();

            grdSecundaryLCD.Visibility = Visibility.Collapsed;

            imgBorrar.IsHitTestVisible = false;
            imgBorrar.Opacity = 0.5;
        }

        public DisplayUC(ComponenteDTO _data)
        {
            InitializeComponent();

            if( _data.Etiqueta == null ) { return; }

            imgEstado.Source = Sesion.Componentes.FirstOrDefault(c => c.IdComponente == _data.IdComponente).Estado.Equals("OK") ? 
                new BitmapImage(new Uri("pack://application:,,,/Estilos/Iconos/verde.png", UriKind.Absolute)) : new BitmapImage(new Uri("pack://application:,,,/Estilos/Iconos/rojo.png", UriKind.Absolute));

            data = _data;

            txbNombre.IsEnabled = false;

            txbNombre.Text = txbLCD.Text = data.Nombre;
            txbTopic.Text = data.Topic;

            txbFila1.Text = data.Etiqueta?.Frase1;
            txbFila2.Text = data.Etiqueta?.Frase2;
            txbFila3.Text = data.Etiqueta?.Frase3;
            txbFila4.Text = data.Etiqueta?.Frase4;

            Debug.WriteLine($"Inicio  X: {data.PosicionX} Y: {data.PosicionY}");

            esNuevo = false;
            grdSecundaryLCD.Visibility = Visibility.Collapsed;
        }

        public void actualizarPosicion(double x, double y)
        {
            data.PosicionX = x ;
            data.PosicionY = y;

            Debug.WriteLine($"Final X: {x} Y: {y}");
        }

        public void cambiarPosicionDelMenu(bool cambio)
        {
            if (cambio)
            {
                grdSecundaryLCD.Margin = new Thickness(0, -580, 0, 0);
            }
            else
            {
                grdSecundaryLCD.Margin = new Thickness(0, 0, 0, 0);
            }
        }

        private void mgrdMainLCD_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            grdSecundaryLCD.Visibility = Visibility.Visible;
        }

        private void mgrdMainLCD_MouseEnter(object sender, MouseEventArgs e)
        {
            imgLCD.Opacity = 0.5;
        }

        private void mgrdMainLCD_MouseLeave(object sender, MouseEventArgs e)
        {
            imgLCD.Opacity = 1;
        }

        private async void Basura_MouseLeftButtonUpAsync(object sender, MouseButtonEventArgs e)
        {
            ErrorDTO respuesta = await ComponenteService.EliminarComponente(data.IdComponente);

            if (respuesta == null || respuesta.Status != 200) { MessageBox.Show($"Error al eliminar el componente: {respuesta.Errors.First().Value}"); return; }//popup

            this.Visibility = Visibility.Collapsed;

            int indiceSeccion = Sesion._centros[Sesion.centroSelecionado]._secciones.FindIndex(s => s.IdSeccion == data.IdSeccion);

            Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion]._componentes.Remove(data);
            Sesion.Componentes.Remove(Sesion.Componentes.Where(c => c.IdComponente == data.IdComponente).First());
        }

        private void Volver_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.grdSecundaryLCD.Visibility = Visibility.Collapsed;
            e.Handled = true;
        }

        private async void Save_MouseLeftButtonUpAsync(object sender, MouseButtonEventArgs e)
        {
            if(string.IsNullOrEmpty(txbNombre.Text))
            {
                txbNombre.Background = Brushes.OrangeRed;
                return;
            }

            data.Nombre = txbLCD.Text = txbNombre.Text;

            if (string.IsNullOrEmpty(txbTopic.Text))
            {
                txbNombre.Background = Brushes.OrangeRed;
                return;
            }

            data.Topic = txbTopic.Text;
            data.IdSeccion = Sesion.seccionSelecionado; 

            if(data.Etiqueta is null) { data.Etiqueta = new EtiquetaDTO(); }

            data.Etiqueta.Frase1 = String.IsNullOrEmpty(txbFila1.Text) ? string.Empty : txbFila1.Text;
            data.Etiqueta.Frase2 = String.IsNullOrEmpty(txbFila2.Text) ? string.Empty : txbFila2.Text;
            data.Etiqueta.Frase3 = String.IsNullOrEmpty(txbFila3.Text) ? string.Empty : txbFila3.Text;
            data.Etiqueta.Frase4 = String.IsNullOrEmpty(txbFila4.Text) ? string.Empty : txbFila4.Text;

            if (esNuevo)
            {
                ErrorDTO respuesta = await ComponenteService.CrearComponente(data);

                if (respuesta == null || respuesta.Status != 200) { MessageBox.Show($"Error al crear el componente: {respuesta.Errors.First().Value.First()}"); return; }//popup

                data.IdComponente = int.Parse(respuesta.Errors["Id"][0]);

                if(Sesion._centros[Sesion.centroSelecionado]._secciones is null)
                {
                    Sesion._centros[Sesion.centroSelecionado]._secciones = new List<SeccionDTO>();
                }

                int indiceSeccion = Sesion._centros[Sesion.centroSelecionado]._secciones.FindIndex(s => s.IdSeccion == data.IdSeccion);

                if(Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion]._componentes is null)
                {
                    Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion]._componentes = new List<ComponenteDTO>();
                }

                Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion]._componentes.Add(data);

                //Sesion._centros[Sesion.centroSelecionado].numeroComponentes++;

                Sesion.Componentes.Add(ComponenteToResumen.ConvierteAResumenDTO(data, Sesion._centros[Sesion.centroSelecionado].Nombre, Sesion._centros[Sesion.centroSelecionado]._secciones[indiceSeccion].Nombre));

                txbNombre.IsEnabled = false;

                esNuevo = false;
            }
            else
            {
                ErrorDTO respuesta = await ComponenteService.ActualizarComponente(data);
                if(respuesta == null || respuesta.Status != 200) { MessageBox.Show($"Error al actualizar el componente: {respuesta.Errors.First().Value}"); return; }//popup
            }

            imgBorrar.IsHitTestVisible = true;
            imgBorrar.Opacity = 1;

            Sesion.publicarMensajeMqtt($"LCD/{data.Topic}/OUTPUT", $"{data.Etiqueta.Frase1}|{data.Etiqueta.Frase2}|{data.Etiqueta.Frase3}|{data.Etiqueta.Frase4}|");

            grdSecundaryLCD.Visibility = Visibility.Collapsed;
        }
    }
}
