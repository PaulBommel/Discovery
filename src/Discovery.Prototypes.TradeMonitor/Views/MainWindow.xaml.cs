using System.Text;
using System.Windows.Controls.Ribbon;

namespace Discovery.Prototypes.TradeMonitor.Views
{
    using Darkstat;
    using ViewModels;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(new DarkstatClient(new DarkstatHttpClientFactory()));
        }
    }
}