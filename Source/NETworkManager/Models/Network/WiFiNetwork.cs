using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace NETworkManager.Models.Network
{
    public class WiFiNetwork
    {
        #region Variables
        private WiFiAdapter _wiFiAdapter;
        public WiFiAdapter WiFiAdapter
        {
            get { return _wiFiAdapter; }
            set
            {
                if (value == _wiFiAdapter)
                    return;

                _wiFiAdapter = value;
            }
        }

        private List<DeviceInformation> _wiFiAdapters;
        public List<DeviceInformation> WiFiAdapters
        {
            get { return _wiFiAdapters; }
            set
            {
                if (value == _wiFiAdapters)
                    return;

                _wiFiAdapters = value;
            }
        }
        #endregion

        #region Events
        public event EventHandler<WiFiNetworkFoundArgs> WiFiNetworkFound;

        protected virtual void OnWiFiNetworkFound(WiFiNetworkFoundArgs e)
        {
            WiFiNetworkFound?.Invoke(this, e);
        }

        public event EventHandler Complete;

        protected virtual void OnComplete()
        {
            Complete?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Constructor
        public WiFiNetwork()
        {

        }
        #endregion

        #region Methods
        public async Task TryGetAccess()
        {
            WiFiAccessStatus wiFiAccessStatus = await WiFiAdapter.RequestAccessAsync();

            if (wiFiAccessStatus != WiFiAccessStatus.Allowed)
                throw new WiFiAdapterAccessDeniedException();
        }

        public async Task FindAdapters()
        {
            WiFiAdapters = (await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector())).ToList();
        }

        public async void SetAdpater(string id = null)
        {
            if (WiFiAdapters.Count == 0)
                throw new Exception("No adapter available!");

            WiFiAdapter = await WiFiAdapter.FromIdAsync(id == null ? WiFiAdapters.First().Id : WiFiAdapters.First(x => x.Id == id).Id);
        }

        public void ScanAsync()
        {
            Task.Run(async () =>
            {
                if (WiFiAdapter == null)
                    throw new Exception("No adapter selected");

                // Do a scan
                await WiFiAdapter.ScanAsync();

                // Process the result
                foreach (WiFiAvailableNetwork network in WiFiAdapter.NetworkReport.AvailableNetworks)
                {
                    OnWiFiNetworkFound(new WiFiNetworkFoundArgs(network.Bssid, network.Ssid, network.SignalBars, network.SecuritySettings.NetworkAuthenticationType, network.SecuritySettings.NetworkEncryptionType, network.ChannelCenterFrequencyInKilohertz, network.NetworkKind, network.PhyKind));
                }

                OnComplete();
            });
        }
        #endregion
    }
}
