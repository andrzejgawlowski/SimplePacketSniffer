using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows.Media;

namespace packetSniffer
{
    class UIControl : MainWindow
    {
        public static uint senderCounter = 0;

        public static uint recvCounter = 0;

        public static void UpdateItemSourceList(IEnumerable<Packet> packets)
        {
            App.Current.Dispatcher.Invoke(()=>{ list.ItemsSource = packets; });
        }
        public static void DynamicUpdate(Packet packet)
        {
            App.Current.Dispatcher.Invoke(() => { Monitor.packets.Add(packet); });
        }
        public static void UpdateHexView(string[] str)
        {
            App.Current.Dispatcher.Invoke(() => { list2.Items.Add(str); list2.UpdateLayout(); });
        }
        public static void ClearItemSourceList()
        {
            App.Current.Dispatcher.Invoke(()=> { list.ItemsSource = null; /*if (list.HasItems) { try { list.Items.Clear();  } catch (System.InvalidOperationException) { list.ItemsSource = null; } } Monitor.packets.Clear();*/ });
        }
        public static void ClearHexView()
        {
            App.Current.Dispatcher.Invoke(() => { list2.Items.Clear(); });
        } 
        public static void SetUpInterfaces()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.OperationalStatus == OperationalStatus.Up)
                        _NetInterfaces.Items.Add(nic);
                }
            });
        }
        public static void ResetControlsToDefault()
        {
            App.Current.Dispatcher.Invoke(()=> {
                _NetInterfaces.SelectedIndex = 0;
                _MaxPcktCountBox.Text = "Inf";
                _MaxPcktSizeBox.Text = "Inf";
                _CapDur.Text = "Inf";
                _DialogFileBox.Text = "";
                _PacketType.SelectedIndex = 0;
            });
        }
        public static void ResetFilterOptions()
        {
            _fchbox1.IsChecked = true;
            _fchbox2.IsChecked = true;
            _fchbox3.IsChecked = true;

            _txtBox1.Text = string.Empty;
            _txtBox2.Text = string.Empty;
            _txtBox3.Text = string.Empty;
            _txtBox4.Text = string.Empty;
            _lenght1.Text = string.Empty;
            _lenght2.Text = string.Empty;

            _cbbox1.SelectedIndex = 0;
        }
        public static void SniffTurnOff()
        {
            App.Current.Dispatcher.Invoke(() => _CaptureStatus.BorderBrush = new SolidColorBrush(Colors.Red));
        }
        public static void UpdateTotalSended()
        {
            App.Current.Dispatcher.Invoke(() => _TotalPacketSended.Content = $"Total Packet Sended: {senderCounter}");
        }
        public static void UpdateTotalSize(string byteString)
        {
            App.Current.Dispatcher.Invoke(() => _TotalPacketSize.Content = $"Total Packet Size: {byteString}");
        }

    }
}
