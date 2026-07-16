using IoTSuper_DesktopApp.Config;
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

        public enum TipoIoT
        {
            Display,
            Stock,
            Termometro
        }

        TipoIoT tipoIoTSeleccionado;

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
            List<StockUC> stks = new List<StockUC>();
            List<TemperaturaUC> tmps = new List<TemperaturaUC>();

            foreach (ComponenteDTO componente in seccion._componentes)
            {
                if(componente.Etiqueta != null)
                    lcds.Add(new DisplayUC(componente));
                else if(componente.Stock != null)
                    stks.Add(new StockUC(componente));
                else if(componente.Termometro != null)
                    tmps.Add(new TemperaturaUC(componente));
            }

            if (lcds is not null)
            {
                foreach (DisplayUC lcd in lcds)
                {
                    lcd.MouseRightButtonDown += UC_MouseRightButtonDown;
                    lcd.MouseRightButtonUp += UC_MouseRightButtonUp;
                    lcd.MouseMove += Lcd_MouseMove;

                    Debug.WriteLine($"Cargando LCD: {lcd.data.Topic}");

                    Canvas.SetLeft(lcd, lcd.data.PosicionX);
                    Canvas.SetTop(lcd, lcd.data.PosicionY);

                    ImagenCanvas.Children.Add(lcd);
                }
            }

            if(stks is not null)
            {
                foreach (StockUC stk in stks)
                {
                    stk.MouseRightButtonDown += UC_MouseRightButtonDown;
                    stk.MouseRightButtonUp += UC_MouseRightButtonUp;
                    stk.MouseMove += Stk_MouseMove;

                    Debug.WriteLine($"Cargando STK: {stk.data.Topic}");

                    Canvas.SetLeft(stk, stk.data.PosicionX);
                    Canvas.SetTop(stk, stk.data.PosicionY);

                    ImagenCanvas.Children.Add(stk);
                }
            }

            if (stks is not null)
            {
                foreach (TemperaturaUC tmp in tmps)
                {
                    tmp.MouseRightButtonDown += UC_MouseRightButtonDown;
                    tmp.MouseRightButtonUp += UC_MouseRightButtonUp;
                    tmp.MouseMove += Tmp_MouseMove;

                    Debug.WriteLine($"Cargando TMP: {tmp.data.Topic}");

                    Canvas.SetLeft(tmp, tmp.data.PosicionX);
                    Canvas.SetTop(tmp, tmp.data.PosicionY);

                    ImagenCanvas.Children.Add(tmp);
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

            tipoIoTSeleccionado = TipoIoT.Display;

            DragDrop.DoDragDrop(ImageLCD, ImageLCD.Source, DragDropEffects.Copy);
        }

        private void ImageLCD_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageLCD_MouseLeftButtonUp");
            arrastrandoElemento = false;
        }

        private void ImageSTK_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageSTK_MouseLeftButtonDown");
            Point pos = e.GetPosition(ImageStock);
            HitTestResult result = VisualTreeHelper.HitTest(ImageStock, pos);

            arrastrandoElemento = false;

            if (result == null || !(result.VisualHit is Image))
                return;

            arrastrandoElemento = true;

            tipoIoTSeleccionado = TipoIoT.Stock;

            DragDrop.DoDragDrop(ImageStock, ImageStock.Source, DragDropEffects.Copy);
        }

        private void ImageSTK_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageSTK_MouseLeftButtonUp");
            arrastrandoElemento = false;
        }

        private void ImageTMP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageTMP_MouseLeftButtonDown");

            Point pos = e.GetPosition(ImageTemperatura);
            HitTestResult result = VisualTreeHelper.HitTest(ImageTemperatura, pos);
            arrastrandoElemento = false;

            if (result == null || !(result.VisualHit is Image))
                return;

            arrastrandoElemento = true;

            tipoIoTSeleccionado = TipoIoT.Termometro;

            DragDrop.DoDragDrop(ImageTemperatura, ImageTemperatura.Source, DragDropEffects.Copy);
        }

        private void ImageTMP_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("ImageTMP_MouseLeftButtonUp");
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


                switch(tipoIoTSeleccionado)
                {
                    case TipoIoT.Display:
                        crearDisplay(posicionMouse);
                        break;
                    case TipoIoT.Stock:
                        crearStock(posicionMouse);
                        break;
                    case TipoIoT.Termometro:
                        crearTermometro(posicionMouse);
                        break;
                }
            }

            arrastrandoElemento = false;
        }

        private void crearTermometro(Point posicionMouse)
        {
            TemperaturaUC tmp = new TemperaturaUC(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));

            tmp.MouseRightButtonDown += UC_MouseRightButtonDown;
            tmp.MouseRightButtonUp += UC_MouseRightButtonUp;
            tmp.MouseMove += Tmp_MouseMove;

            Canvas.SetLeft(tmp, posicionMouse.X - 100);
            Canvas.SetTop(tmp, posicionMouse.Y - 50);
            ImagenCanvas.Children.Add(tmp);
            tmp.data.PosicionX = posicionMouse.X;
            tmp.data.PosicionY = posicionMouse.Y;
        }

        private void crearStock(Point posicionMouse)
        {
            StockUC stk = new StockUC(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));

            stk.MouseRightButtonDown += UC_MouseRightButtonDown;
            stk.MouseRightButtonUp += UC_MouseRightButtonUp;
            stk.MouseMove += Stk_MouseMove;

            Canvas.SetLeft(stk, posicionMouse.X - 100);
            Canvas.SetTop(stk, posicionMouse.Y - 50);
            ImagenCanvas.Children.Add(stk);

            stk.data.PosicionX = posicionMouse.X;
            stk.data.PosicionY = posicionMouse.Y;
        }

        private void crearDisplay(Point posicionMouse)
        {
            DisplayUC lcd = new DisplayUC(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));

            lcd.MouseRightButtonDown += UC_MouseRightButtonDown;
            lcd.MouseRightButtonUp += UC_MouseRightButtonUp;
            lcd.MouseMove += Lcd_MouseMove;

            Canvas.SetLeft(lcd, posicionMouse.X - 100);
            Canvas.SetTop(lcd, posicionMouse.Y - 50);

            ImagenCanvas.Children.Add(lcd);

            lcd.data.PosicionX = posicionMouse.X;
            lcd.data.PosicionY = posicionMouse.Y;
        }

        Point UCMouse = new Point();
        bool UCMoviendo = false;

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

        private void Stk_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                StockUC stk = sender as StockUC;

                Point posicionMouse = e.GetPosition(ImagenCanvas);

                Canvas.SetLeft(stk, posicionMouse.X - 100);
                Canvas.SetTop(stk, posicionMouse.Y - 50);

                stk.cambiarPosicionDelMenu(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));
                stk.actualizarPosicion(posicionMouse.X - 100 + Math.Abs(posicionAbsolutaEstanteria), posicionMouse.Y - 50);
            }
        }

        private void Tmp_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                TemperaturaUC lcd = sender as TemperaturaUC;
                Point posicionMouse = e.GetPosition(ImagenCanvas);

                Canvas.SetLeft(lcd, posicionMouse.X - 100);
                Canvas.SetTop(lcd, posicionMouse.Y - 50);

                lcd.cambiarPosicionDelMenu(posicionMouse.Y > (ImagenCanvas.ActualHeight / 2));
                lcd.actualizarPosicion(posicionMouse.X - 100 + Math.Abs(posicionAbsolutaEstanteria), posicionMouse.Y - 50);
            }
        }

        private void UC_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            UCMoviendo = false;

        }

        Point clickOffset;

        private void UC_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            UCMouse = e.GetPosition(ImagenCanvas);
            UCMoviendo = true;
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
            
            ImagenMovible.Source = new BitmapImage(new Uri(dialog.FileName));
            _seccion.Imagen = dialog.FileName;

            ErrorDTO respuesta = await SeccionService.ActualizarSeccion(_seccion);

            if (respuesta == null || respuesta.Status != 200) { return; }//popup

            RClone.RClone.SubirImagenesAlServidorAsync();
        }

        #endregion
    }
}

/*****************************************************/

/*
 

uint64_t chipID = ESP.getEfuseMac();
Serial.printf("CHIP MAC: %012llx\n", chipID);


*/  