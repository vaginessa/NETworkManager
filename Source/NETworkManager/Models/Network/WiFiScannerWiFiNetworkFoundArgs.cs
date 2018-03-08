using Windows.Devices.WiFi;

namespace NETworkManager.Models.Network
{
    public class WiFiScannerWiFiNetworkFoundArgs : System.EventArgs
    {
        public string MACAddress { get; set; }
        public string SSID { get; set; }
        public byte Signal { get; set; }
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        public WiFiNetworkKind NetworkKind { get; set; }
        public WiFiPhyKind PhyKind { get; set; }

        public WiFiScannerWiFiNetworkFoundArgs()
        {

        }

        public WiFiScannerWiFiNetworkFoundArgs(string macAddress, string ssid, byte signal, int channelCenterFrequencyInKilohertz, WiFiNetworkKind networkKind, WiFiPhyKind phyKind)
        {
            MACAddress = macAddress;
            SSID = ssid;
            Signal = signal;
            ChannelCenterFrequencyInKilohertz = channelCenterFrequencyInKilohertz;
            NetworkKind = networkKind;
            PhyKind = phyKind;
        }
    }
}
