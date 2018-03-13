using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;

namespace NETworkManager.Models.Network
{
    public class WiFiNetworkInfo
    {
        public string MACAddress { get; set; }
        public string SSID { get; set; }
        public byte Signal { get; set; }
        public NetworkAuthenticationType AuthenticationType { get; set; }
        public NetworkEncryptionType EncryptionType { get; set; }
        public int ChannelCenterFrequencyInKilohertz { get; set; }
        public WiFiNetworkKind NetworkKind { get; set; }
        public WiFiPhyKind PhyKind { get; set; }

        public WiFiNetworkInfo()
        {

        }

        public WiFiNetworkInfo(string macAddress, string ssid, byte signal, NetworkAuthenticationType authenticationType, NetworkEncryptionType encryptionType, int channelCenterFrequencyInKilohertz, WiFiNetworkKind networkKind, WiFiPhyKind phyKind)
        {
            MACAddress = macAddress;
            SSID = ssid;
            Signal = signal;
            AuthenticationType = authenticationType;
            EncryptionType = encryptionType;
            ChannelCenterFrequencyInKilohertz = channelCenterFrequencyInKilohertz;
            NetworkKind = networkKind;
            PhyKind = phyKind;
        }

        public static WiFiNetworkInfo Parse(WiFiNetworkFoundArgs e)
        {
            return new WiFiNetworkInfo(e.MACAddress, e.SSID, e.Signal,e.AuthenticationType, e.EncryptionType, e.ChannelCenterFrequencyInKilohertz, e.NetworkKind, e.PhyKind);
        }
    }
}
