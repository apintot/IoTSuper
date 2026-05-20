using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace carousel
{
    public partial class MainWindow : Window
    {
        private int _currentIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => RefreshCards();
        }

        private void RefreshCards()
        {
            var vm = (MainViewModel)DataContext;
            var centros = vm.Centros;
            int count = centros.Count;
            if (count == 0) return;

            int left = (_currentIndex - 1 + count) % count;
            int center = _currentIndex;
            int right = (_currentIndex + 1) % count;

            SetCard(TitleLeft, ImgLeft, SecLeft, DispLeft, centros[left]);
            SetCard(TitleCenter, ImgCenter, SecCenter, DispCenter, centros[center]);
            SetCard(TitleRight, ImgRight, SecRight, DispRight, centros[right]);
        }

        private void SetCard(System.Windows.Controls.TextBlock title, System.Windows.Controls.Image img, System.Windows.Controls.TextBlock sec, System.Windows.Controls.TextBlock disp, Centro c)
        {
            title.Text = c.Nombre;
            sec.Text = c.NumSecciones.ToString();
            disp.Text = c.NumDispositivos.ToString();
            if (!string.IsNullOrEmpty(c.ImagenUrl))
                img.Source = new BitmapImage(new System.Uri(c.ImagenUrl, System.UriKind.RelativeOrAbsolute));
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            _currentIndex = (_currentIndex - 1 + vm.Centros.Count) % vm.Centros.Count;
            RefreshCards();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)DataContext;
            _currentIndex = (_currentIndex + 1) % vm.Centros.Count;
            RefreshCards();
        }
    }

    public class Centro : INotifyPropertyChanged
    {
        public string Nombre { get; set; } = "";
        public string ImagenUrl { get; set; } = "";
        public int NumSecciones { get; set; }
        public int NumDispositivos { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Centro> Centros { get; set; }

        public MainViewModel()
        {
            Centros = new ObservableCollection<Centro>
            {
                new Centro { Nombre = "Centro A", ImagenUrl = "", NumSecciones = 8,  NumDispositivos = 8  },
                new Centro { Nombre = "Centro B", ImagenUrl = "", NumSecciones = 5,  NumDispositivos = 12 },
                new Centro { Nombre = "Centro C", ImagenUrl = "", NumSecciones = 3,  NumDispositivos = 6  },
                new Centro { Nombre = "Centro D", ImagenUrl = "", NumSecciones = 10, NumDispositivos = 20 },
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
