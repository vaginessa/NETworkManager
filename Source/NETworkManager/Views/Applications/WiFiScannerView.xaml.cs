using NETworkManager.ViewModels.Applications;
using System.Windows.Controls;

namespace NETworkManager.Views.Applications
{
    public partial class WiFiScannerView : UserControl
    {
        WiFiScannerViewModel viewModel = new WiFiScannerViewModel();

        public WiFiScannerView()
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
