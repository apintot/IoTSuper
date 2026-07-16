using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Seguridad;
using IoTSuper_DesktopApp.Servicios;
using IoTSuper_DesktopApp.Servicios.API;
using OtpNet;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace IoTSuper_DesktopApp.Modelos
{
    /// <summary>
    /// Lógica de interacción para LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : Window
    {
        public LoginUserControl()
        {
            InitializeComponent();
        }

        private async void Login_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                Sesion._stopwatch.Start();
                txbError.Visibility = Visibility.Collapsed;

                Crypto crypto = new Crypto();

                LoginResponse response = await LoginService.IniciarSesionAsync(camUser.Texto, camPass.Contrasena);

                if(response.IdCliente == 0) { txbError.Text = "Usuario o contraseña incorrectos"; txbError.Visibility = Visibility.Visible; return; }

                Sesion.LoginData = response;

                if((DateTime.Now - response.ultimoAcceso).TotalDays > 1)
                {
                    MostrarTOTP(string.IsNullOrEmpty(response.TOTP));
                }
                else
                {
                    MostrarApp();
                }       
            }
            catch (Exception ex) { txbError.Text = ex.Message; txbError.Visibility = Visibility.Visible; }
        }

        private void MostrarApp()
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void MostrarTOTP(bool generarCodigoQr)
        {
            grdLogin.Visibility = Visibility.Collapsed;
            grdTOTP.Visibility = Visibility.Visible;

            if (generarCodigoQr)
            {
                Sesion.LoginData.TOTP = Base32Encoding.ToString(KeyGeneration.GenerateRandomKey(20));
                string otpUrl = $"otpauth://totp/{Sesion.msiName}:{camUser.Texto}?secret={Sesion.LoginData.TOTP}&issuer={Sesion.msiName}";
                string pathQr = Rutas.AppFolder + "//QR.png";

                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpUrl, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap bitmap = qrCode.GetGraphic(20);
                bitmap.Save(pathQr);

                imgQR.Source = new BitmapImage(new Uri(pathQr));
            }
            else
            {
                imgQR.Visibility = Visibility.Collapsed;
            }
        }

        private async void TOPT_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Sesion.LoginData.TOTP))
            {
                Totp totp = new Totp(Base32Encoding.ToBytes(Sesion.LoginData.TOTP));

                if (totp.VerifyTotp(camTOPT.Texto, out long tiempoRestante))
                {
                    await LoginService.ActualizarTOTP(Sesion.LoginData.TOTP);
                    MostrarApp();
                }
                else
                {
                    camTOPT.Background = new SolidColorBrush(Colors.Red);
                }
            }
        }
    }
}
