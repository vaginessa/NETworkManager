using System.Windows.Input;
using System;
using System.Windows.Threading;
using System.Diagnostics;
using NETworkManager.Models.Network;
using System.ComponentModel;
using System.Windows.Data;
using Windows.Devices.Enumeration;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;

namespace NETworkManager.ViewModels.Applications
{
    public class WiFiScannerViewModel : ViewModelBase
    {
        #region Variables
        WiFiNetwork wiFiNetwork;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopwatch = new Stopwatch();

        private bool _isLoading = true;

        private ICollectionView _wiFiAdaptersView;
        public ICollectionView WiFiAdaptersView
        {
            get { return _wiFiAdaptersView; }
        }

        private DeviceInformation _selectedWiFiAdapter;
        public DeviceInformation SelectedWiFiAdapter
        {
            get { return _selectedWiFiAdapter; }
            set
            {
                if (value == _selectedWiFiAdapter)
                    return;

                if (value != null)
                    wiFiNetwork.SetAdpater(value.Id);

                _selectedWiFiAdapter = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<WiFiNetworkInfo> _wiFiNetworks = new ObservableCollection<WiFiNetworkInfo>();
        public ObservableCollection<WiFiNetworkInfo> WiFiNetworks
        {
            get { return _wiFiNetworks; }
            set
            {
                if (value == _wiFiNetworks)
                    return;

                _wiFiNetworks = value;
                OnPropertyChanged();
            }
        }

        private ICollectionView _wiFiNetworksView;
        public ICollectionView WiFiNetworksView
        {
            get { return _wiFiNetworksView; }
        }

        private bool _isScanRunning;
        public bool IsScanRunning
        {
            get { return _isScanRunning; }
            set
            {
                if (value == _isScanRunning)
                    return;

                _isScanRunning = value;
                OnPropertyChanged();
            }
        }

        private bool _displayStatusMessage;
        public bool DisplayStatusMessage
        {
            get { return _displayStatusMessage; }
            set
            {
                if (value == _displayStatusMessage)
                    return;

                _displayStatusMessage = value;
                OnPropertyChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                if (value == _statusMessage)
                    return;

                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _startTime;
        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                if (value == _startTime)
                    return;

                _startTime = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _duration;
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (value == _duration)
                    return;

                _duration = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endTime;
        public DateTime? EndTime
        {
            get { return _endTime; }
            set
            {
                if (value == _endTime)
                    return;

                _endTime = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Constructor, load settings, shutdown
        public WiFiScannerViewModel()
        {
            InitializeWiFi();

            _wiFiNetworksView = CollectionViewSource.GetDefaultView(WiFiNetworks);

            LoadSettings();

            _isLoading = false;
        }

        private void LoadSettings()
        {

        }
        #endregion

        #region ICommands & Actions
        public ICommand ScanCommand
        {
            get { return new RelayCommand(p => ScanAction()); }
        }

        private void ScanAction()
        {
            Scan();
        }
        #endregion

        #region Methods
        private async void InitializeWiFi()
        {
            wiFiNetwork = new WiFiNetwork();

            // Get access
            try
            {
                await wiFiNetwork.TryGetAccess();
            }
            catch (WiFiAdapterAccessDeniedException)
            {
                throw;
            }

            await wiFiNetwork.FindAdapters();

            // Set the collection view source
            _wiFiAdaptersView = CollectionViewSource.GetDefaultView(wiFiNetwork.WiFiAdapters);
        }

        private void Scan()
        {
            DisplayStatusMessage = false;
            IsScanRunning = true;

            // Measure the time
            StartTime = DateTime.Now;
            stopwatch.Start();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
            EndTime = null;

            WiFiNetworks.Clear();

            Debug.WriteLine("Scan");

            wiFiNetwork.WiFiNetworkFound += WiFiScanner_WiFiNetworkFound;
            wiFiNetwork.Complete += WiFiScanner_Complete;

            wiFiNetwork.ScanAsync();
        }

        private void ScanFinished()
        {
            // Stop timer and stopwatch
            stopwatch.Stop();
            dispatcherTimer.Stop();

            Duration = stopwatch.Elapsed;
            EndTime = DateTime.Now;

            stopwatch.Reset();

            IsScanRunning = false;
        }
        #endregion

        #region Events
        private void WiFiScanner_WiFiNetworkFound(object sender, WiFiNetworkFoundArgs e)
        {
            WiFiNetworkInfo wiFiNetworkInfo = WiFiNetworkInfo.Parse(e);

            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                WiFiNetworks.Add(wiFiNetworkInfo);
            }));
        }

        private void WiFiScanner_Complete(object sender, EventArgs e)
        {
            ScanFinished();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Duration = stopwatch.Elapsed;
        }
        #endregion
    }
}
