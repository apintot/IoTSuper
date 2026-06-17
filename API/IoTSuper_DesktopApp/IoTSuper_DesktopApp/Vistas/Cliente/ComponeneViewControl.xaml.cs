using IoTSuper_DesktopApp.Controladores.Componentes;
using IoTSuper_DesktopApp.Helpers;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Componente;
using IoTSuper_DesktopApp.Servicios.Seccion;
using Microsoft.Win32;
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

namespace IoTSuper_DesktopApp.Vistas.Cliente
{
    /// <summary>
    /// Lógica de interacción para ComponeneViewControl.xaml
    /// </summary>
    public partial class ComponeneViewControl : UserControl
    {
        private bool arrastrandoElemento = false;
        private bool arrastrandoCanva = false;

        private double final;
        private double posicionInicialElemento;

        private Point puntoDePartidaRaton;

        private double posicionAbsolutaEstanteria = 0;

        SeccionDTO _seccion;

        #region ctro

        public ComponeneViewControl(SeccionDTO seccion)
        {
            InitializeComponent();

            final = ImagenMovible.ActualWidth - 1600;

            if (final > 0) { final = -final; }

            _seccion = seccion;

            if (!string.IsNullOrEmpty(_seccion.Imagen))
            {
                ImagenMovible.Source = new BitmapImage(new Uri(_seccion.Imagen));
            }

            if(_seccion.NumComponentes == 0) { return; }

            List<DisplayUC> lcds = new List<DisplayUC>();

            foreach(ComponenteDTO componente in seccion._componentes)
            {
                if(componente.Etiqueta != null)
                    lcds.Add(new DisplayUC(componente));
            }

            if (lcds is not null)
            {
                foreach (DisplayUC lcd in lcds)
                {
                    lcd.MouseRightButtonDown += Lcd_MouseRightButtonDown;
                    lcd.MouseRightButtonUp += Lcd_MouseRightButtonUp;
                    lcd.MouseMove += Lcd_MouseMove;

                    Debug.WriteLine($"Cargando LCD: {lcd.data.Topic}");

                    Canvas.SetLeft(lcd, lcd.data.PosicionX);
                    Canvas.SetTop(lcd, lcd.data.PosicionY);

                    ImagenCanvas.Children.Add(lcd);
                }
            }
        }

        #endregion

        #region Imagen Estanteria eventos

        private void ImagenMovible_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !arrastrandoElemento && arrastrandoCanva)
            {
                Debug.WriteLine("ImagenMovible_MouseMove");
                Point posicionActualRaton = e.GetPosition(ImagenCanvas);

                double movimiento = posicionActualRaton.X - puntoDePartidaRaton.X;

                double posicionImagen = Canvas.GetLeft(ImagenMovible);

                if (posicionImagen + movimiento > 0 || (-(posicionImagen + movimiento) > (ImagenMovible.ActualWidth - ImagenCanvas.ActualWidth))) { return; }

                Canvas.SetLeft(ImagenMovible, posicionImagen + movimiento);

                posicionAbsolutaEstanteria = posicionAbsolutaEstanteria + movimiento;

                foreach (DisplayUC lcd in ImagenCanvas.Children.OfType<DisplayUC>())
                {
                    Canvas.SetLeft(lcd, Canvas.GetLeft(lcd) + movimiento);
                    lcd.data.PosicionX = Canvas.GetLeft(lcd);
                }

                puntoDePartidaRaton = posicionActualRaton;
            }
            else
            {
                Console.WriteLine($"{ImageTransform.X} y {ImageTransform.Y}");
            }
        }

        private void ImagenMovible_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            puntoDePartidaRaton = e.GetPosition(ImagenCanvas);
            posicionInicialElemento = Canvas.GetLeft(ImagenMovible);

            Debug.WriteLine("ImagenMovible_MouseLeftButtonDown");

            arrastrandoCanva = true;
        }

        private void ImagenMovible_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImagenMovible_MouseLeftButtonUp");
            arrastrandoCanva = false;
        }

        #endregion

        #region LCD

        private void ImageLCD_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageLCD_MouseLeftButtonDown");
            Point pos = e.GetPosition(ImageLCD);
            HitTestResult result = VisualTreeHelper.HitTest(ImageLCD, pos);

            arrastrandoElemento = false;

            if (result == null || !(result.VisualHit is Image))
                return;

            arrastrandoElemento = true;

            DragDrop.DoDragDrop(ImageLCD, ImageLCD.Source, DragDropEffects.Copy);
        }

        private void ImageLCD_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageLCD_MouseLeftButtonUp");
            arrastrandoElemento = false;
        }

        private void ImagenMovible_Drop(object sender, DragEventArgs e)
        {
            if (arrastrandoElemento)
            {
                Debug.WriteLine("ImagenMovible_Drop");
                e.Effects = DragDropEffects.None;
                Console.Write("Suelto");
                Point posicionMouse = e.GetPosition(ImagenCanvas);
                Console.Write(posicionMouse.X + " " + posicionMouse.Y);

                DisplayUC lcd = new DisplayUC(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));

                lcd.MouseRightButtonDown += Lcd_MouseRightButtonDown;
                lcd.MouseRightButtonUp += Lcd_MouseRightButtonUp;
                lcd.MouseMove += Lcd_MouseMove;

                Canvas.SetLeft(lcd, posicionMouse.X - 100);
                Canvas.SetTop(lcd, posicionMouse.Y - 50);

                ImagenCanvas.Children.Add(lcd);

                lcd.data.PosicionX = posicionMouse.X;
                lcd.data.PosicionY = posicionMouse.Y;

                //StaticDataSession.guardarDatosDelCentro(_seccion);
            }

            arrastrandoElemento = false;
        }


        Point lcdMouse = new Point();
        bool DisplayUCMoviendo = false;

        private void Lcd_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                DisplayUC lcd = sender as DisplayUC;

                Point posicionMouse = e.GetPosition(ImagenCanvas);

                Canvas.SetLeft(lcd, posicionMouse.X - 100);
                Canvas.SetTop(lcd, posicionMouse.Y - 50);

                lcd.cambiarPosicionDelMenu(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));
                lcd.actualizarPosicion(posicionMouse.X - 100 + Math.Abs(posicionAbsolutaEstanteria), posicionMouse.Y - 50);
            }
        }

        private void Lcd_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DisplayUCMoviendo = false;

        }

        Point clickOffset;

        private void Lcd_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            lcdMouse = e.GetPosition(ImagenCanvas);
            DisplayUCMoviendo = true;
        }

        #endregion

        #region Imagen Anadir Imagen eventos

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            imgAdd.Opacity = 0.75;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            imgAdd.Opacity = 1;
        }

        private async void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LogLocal.logear($"Seleccionado imagen");
            OpenFileDialog fotoSeccion = new OpenFileDialog();
            fotoSeccion.Filter = "Imágenes (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg";

            if (fotoSeccion.ShowDialog() == true)
            {
                ImagenMovible.Source = new BitmapImage(new Uri(fotoSeccion.FileName));

                _seccion.Imagen = fotoSeccion.FileName;

                ErrorDTO respuesta = await SeccionService.ActualizarSeccion(_seccion);

                if (respuesta == null || respuesta.Status != 200) { return; }//popup

                //StaticDataSession.guardarDatosDelCentro(_seccion);
            }
        }

        #endregion
    }
}

/*****************************************************/

/*
 

uint64_t chipID = ESP.getEfuseMac();
Serial.printf("CHIP MAC: %012llx\n", chipID);


*/  