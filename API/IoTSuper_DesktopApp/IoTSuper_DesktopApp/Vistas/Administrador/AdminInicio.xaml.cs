using IoTSuper_DesktopApp.Config;
using IoTSuper_DesktopApp.Modelos;
using IoTSuper_DesktopApp.Servicios.Cliente;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
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
using IoTSuper_DesktopApp.Helpers;

namespace IoTSuper_DesktopApp.Vistas.Administrador
{
    /// <summary>
    /// Lógica de interacción para AdminInicio.xaml
    /// </summary>
    public partial class AdminInicio : UserControl
    {
        private List<Modelos.Cliente> TodosLosUsuarios = new();

        private ICollectionView VistaClientes;

        private int PaginaActual = 1;
        private int NumeroDeElementosPorPagina = 15;
        private int NumeroDePagina = 0;

        public AdminInicio()
        {
            InitializeComponent();
            this.Loaded += AdminInicio_Loaded;
        }

        private async void AdminInicio_Loaded(object? sender, RoutedEventArgs e)
        {
            this.Loaded -= AdminInicio_Loaded;

            TodosLosUsuarios = await ClienteService.ObtenerClientes() ?? new List<Modelos.Cliente>();

            NumeroDePagina = (int)Math.Ceiling((double)TodosLosUsuarios.Count / NumeroDeElementosPorPagina);
            if (NumeroDePagina == 0) NumeroDePagina = 1;

            txbQuePagina.Text = $"{PaginaActual} de {NumeroDePagina}";

            VistaClientes = CollectionViewSource.GetDefaultView(TodosLosUsuarios.Skip((PaginaActual - 1) * NumeroDeElementosPorPagina).Take(NumeroDeElementosPorPagina));
            VistaClientes.Filter = filtrarClientes;
            dgClientes.ItemsSource = VistaClientes;

            camBuscador.txtEntrada.TextChanged += TxtEntrada_TextChanged;
        }

        private void TxtEntrada_TextChanged(object sender, TextChangedEventArgs e)
        {
            VistaClientes.Refresh();
        }

        private bool filtrarClientes(object obj)
        {     
            if (obj is not Modelos.Cliente cliente) { return false; }

            string filtro = camBuscador.Texto.ToLower();

            if (string.IsNullOrWhiteSpace(filtro)) { return true; } 

            return cliente.Nombre.ToLower().Contains(filtro) || cliente.Login.ToLower().Contains(filtro);
        }

        private void EditarCliente_Click(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is Modelos.Cliente cliente)
            {
                Navegacion.IrA(new FormularioCliente(cliente));
            }
        }

        private async void EliminarCliente_ClickAsync(object sender, MouseButtonEventArgs e)
        {
            if (((FrameworkElement)sender).DataContext is Modelos.Cliente cliente)
            {
                if(await ClienteService.eliminarCliente(cliente.IdCliente))
                {
                    TodosLosUsuarios.Remove(cliente);
                    VistaClientes.Refresh();
                }
            }
        }

        private void Left_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(1 < PaginaActual)
            {
                PaginaActual--;
                txbQuePagina.Text = $"{PaginaActual} de {NumeroDePagina}";
                VistaClientes = CollectionViewSource.GetDefaultView(TodosLosUsuarios.Skip((PaginaActual - 1) * NumeroDeElementosPorPagina).Take(NumeroDeElementosPorPagina));
                dgClientes.ItemsSource = VistaClientes;
            }
        }

        private void Right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(PaginaActual < NumeroDePagina)
            {
                PaginaActual++;
                txbQuePagina.Text = $"{PaginaActual} de {NumeroDePagina}";
                VistaClientes = CollectionViewSource.GetDefaultView(TodosLosUsuarios.Skip((PaginaActual - 1) * NumeroDeElementosPorPagina).Take(NumeroDeElementosPorPagina));
                dgClientes.ItemsSource = VistaClientes;
            }
        }

        private void CrearClienteView_Click(object sender, RoutedEventArgs e)
        {
            Navegacion.IrA(new FormularioCliente());
        }
    }
}
