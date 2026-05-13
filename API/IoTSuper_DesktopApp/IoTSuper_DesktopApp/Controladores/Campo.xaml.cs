using System;
using System.Collections.Generic;
using System.Drawing;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IoTSuper_DesktopApp.Controles
{
    /// <summary>
    /// Lógica de interacción para Campo.xaml
    /// </summary>
    public partial class Campo : UserControl
    {
        #region Variables

        public string Texto
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Contrasena
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public ImageSource Icono
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public bool EsContrasena
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        #endregion

        #region Dependencias

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Texto), typeof(string), typeof(Campo), new PropertyMetadata(""));

        public static readonly DependencyProperty PasswordProperty =  DependencyProperty.Register(nameof(Contrasena), typeof(string), typeof(Campo), new PropertyMetadata(""));

        public static readonly DependencyProperty PlaceholderProperty =  DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(Campo), new PropertyMetadata(""));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icono), typeof(ImageSource), typeof(Campo));

        public static readonly DependencyProperty IsPasswordProperty = DependencyProperty.Register(nameof(EsContrasena), typeof(bool), typeof(Campo), new PropertyMetadata(false, ContrasenaCambiada));

        private static void ContrasenaCambiada(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Campo)d).ActualizarModelo();
        }

        #endregion

        public Campo()
        {
            InitializeComponent();
            ActualizarModelo();
        }

        private void ActualizarModelo()
        {
            txtEntrada.Visibility = EsContrasena ? Visibility.Collapsed : Visibility.Visible;
            txtEntradaPassword.Visibility = EsContrasena ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtEntradaPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(string.Empty != txtEntradaPassword.Password )
            {
                txbPlaceholder.Visibility = Visibility.Collapsed;
                Contrasena = txtEntradaPassword.Password;
            }
            else
            {
                txbPlaceholder.Visibility = Visibility.Visible;
            }  
        }

        private void txtEntrada_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.Empty != txtEntrada.Text)
            {
                txbPlaceholder.Visibility = Visibility.Collapsed;
                Texto = txtEntrada.Text;
            }
            else
            {
                txbPlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
