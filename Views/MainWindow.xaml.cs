using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Data;
using System.ComponentModel;

namespace packetSniffer
{
    public partial class MainWindow : Window
    {
        public static DataGrid list;
        public static DataGrid list2;
        //public static ComboBox processList;

        public static CheckBox _fchbox1;
        public static CheckBox _fchbox2;
        public static CheckBox _fchbox3;
        public static TextBox _txtBox1;
        public static TextBox _txtBox2;
        public static TextBox _txtBox3;
        public static TextBox _txtBox4;
        public static ComboBox _cbbox1;

        public static TextBox _lenght1;
        public static TextBox _lenght2;

        public static Button _start;
        public static Button _stop;
        public static ComboBox _PacketType;
        public static ComboBox _NetInterfaces;
        public static TextBox _CapDur;
        public static TextBox _DialogFileBox;
        public static TextBox _MaxPcktSizeBox;
        public static Border _CaptureStatus;
        public static Label _TotalPacketSize;
        public static Label _TotalPacketSended;
        public static TextBox _MaxPcktCountBox;

        public static ICollectionView viewSource;

        // Filter controls

        public IPAddress IP_Address { get; set; }

        public static SniffMonitor Monitor { get; set; }
        public PacketFilter Filter { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            /*Examples to datagrids

                //List.Items.Add(new { Id = 1, SourceIp = "192.168.0.1", DestinationIp = "192.168.0.1", PacketType = "IGMP", PortSource = "443", PortDestination = "443", TotalLenght = "192" });

                //contentBytes.Items.Add(new string[] {"10","FF","CD","AD", "AD", "AD", "AD", "AD", "AD", "AD", "AD", "AD", "CD", "CD", "CD", "CD", "CD" ,"FF" ,"Lorem ipsum, nom omnis moriar"});
            
             */
            SetUpControlVariables(); // SetUpMachine XD
        }
        private void SetUpControlVariables()
        {
            list = List;
            _DialogFileBox = FileDialogText;
            _MaxPcktSizeBox = MaxPcktSize;
            _MaxPcktCountBox = MaxPcktCount;
            _CapDur = CapDur;
            _PacketType = PacketType;
            _NetInterfaces = NetInterfaces;
            list2 = contentBytes;
            _stop = CaptureStop;

            _CaptureStatus = CaptureStatus;
            _TotalPacketSended = TotalSended;

            _fchbox1 = CHTCP;
            _fchbox2 = CHUDP;
            _fchbox3 = CHIGMP;
            _txtBox1 = TXDest;
            _txtBox2 = TXDestIp;
            _txtBox3 = TXSourceIp;
            _txtBox4 = TXSource;
            _lenght1 = TXFrom;
            _lenght2 = TXTo;
            _cbbox1 = CBType;
            _TotalPacketSize = TotalSize;
            ProcessList.ItemsSource = Process.GetProcesses();
            Options.SetUpInterfaces();
            _stop.Click += (o, e) => { StopEvent(); CaptureStatus.BorderBrush = new SolidColorBrush(Colors.Red); };
            FilterResetButt.Click += (o, e) => { StopEvent(); PacketFilter.Reset(Monitor.packets); };
            StopSendingButton.Click += (o, e) => StopEvent();
            list.MouseLeftButtonUp += (o, e) => 
            {
                if (list.SelectedItem != null)
                {
                    UIControl.ClearHexView();
                    var currPacket = list.SelectedItem as Packet;
                    PacketReader.DivideContentBytesToHexView(PacketReader.getHexString(currPacket.rawByteConent), PacketReader.getEncodedString(currPacket.rawByteConent)/*currPacket.ContentBytes, currPacket.ContentEncoded*/);
                }
            };
            Buttt.Click += (o, e) => { BrowseFileDialog.OpenFileDialog(o, e, FileDialogText); };
            AppClose.Click += (o, e) => Application.Current.Shutdown();
            OptionButton.Click += (o, e) => StopEvent();
            ResetButt.Click += (o,e) => Options.ResetOptions();
            SaveButt.Click += (o, e) => SaveButtonEvent();
            NetInterfaces.SelectionChanged += (o, e) =>
            {
                var addresesList2 = (NetInterfaces.SelectedItem as NetworkInterface).GetIPProperties().UnicastAddresses;
                IP_Address = addresesList2[addresesList2.Count - 1].Address;
            };

            var addresesList = (NetInterfaces.SelectedItem as NetworkInterface).GetIPProperties().UnicastAddresses;
            IP_Address = addresesList[addresesList.Count - 1].Address;
            IPaddressControl.Content = IP_Address.ToString();
            Monitor = new SniffMonitor(ListenPortFinder.FindBy((ProcessList.SelectedItem as Process).Id.ToString()), IP_Address, new UnlimitedReceive());
        }
       
        private void StopEvent()
        {
            Monitor.StopRecv();
            viewSource = CollectionViewSource.GetDefaultView(List.ItemsSource);
        }

        private void StartClickEvent(object sender, EventArgs ea)
        {
            UIControl.ClearItemSourceList();

            if (Options.MaxPacketCount > 0 || Options.MaxPacketSize > 0)
            {
                Monitor = new SniffMonitor(ListenPortFinder.GetPortList(), IP_Address, new LimitedReceive(Options.MaxPacketSize, Options.MaxPacketCount));
                Monitor.Start();
            }
            else
            {
                Monitor = new SniffMonitor(ListenPortFinder.GetPortList(), IP_Address, new UnlimitedReceive());
                Monitor.Start();
            }

            CaptureStatus.BorderBrush = new SolidColorBrush(Colors.Green);
            List.ItemsSource = Monitor.packets;
        }

        private void SaveButtonEvent()
        {
            int.TryParse(_CapDur.Text, out int duration);
            int.TryParse(_MaxPcktSizeBox.Text, out int size);
            int.TryParse(_MaxPcktCountBox.Text, out int count);

            Options.SaveOptions(duration, _DialogFileBox.Text, size, count);
        }

        private void SelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            if (ProcessList.SelectedIndex != -1)
            {
                var addressList = (NetInterfaces.SelectedItem as NetworkInterface).GetIPProperties().UnicastAddresses;
                IP_Address = addressList[addressList.Count - 1].Address;
                Monitor = new SniffMonitor(ListenPortFinder.FindBy((ProcessList.SelectedItem as Process).Id.ToString()), IP_Address, new LimitedReceive());
                IPaddressControl.Content = IP_Address.ToString();
            }
        }
        private void AddPacketEvent(object sender, RoutedEventArgs e)
        {
            foreach (var item in List.SelectedItems)
            {
                if (item is Udp)
                    Monitor.udpPackets.Add((Udp)item);
                else
                    MessageBox.Show("You only can add UDP packets", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Sendpacket_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(PacketCount.Text, out int count);
            int.TryParse(PacketDelay.Text, out int delay);
            Monitor.Send(count, delay);
        }

        private void FilterSaveEvent(object o, EventArgs ea)
        {
            if (Monitor != null)
            {
                Monitor.StopRecv();

                var protoList = new List<Protocol>();

                if ((bool)CHIGMP.IsChecked)
                    protoList.Add(Protocol.IGMP);
                if ((bool)CHTCP.IsChecked)
                    protoList.Add(Protocol.TCP);
                if ((bool)CHUDP.IsChecked)
                    protoList.Add(Protocol.UDP);

                // Filtering by protocols -- adding if checkbox is checked

                var filteredPacketList = new PacketFilter(new ProtocolPacketFilter(protoList)).Filter(Monitor.packets);

                IPAddress.TryParse(TXSourceIp.Text, out IPAddress srcIP);
                IPAddress.TryParse(TXDestIp.Text, out IPAddress dstIP);

                filteredPacketList = new PacketFilter(new IpPacketFilter(srcIP, dstIP)).Filter(filteredPacketList);

                filteredPacketList = new PacketFilter(new PortPacketFilter(TXSource.Text, TXDest.Text)).Filter(filteredPacketList);

                filteredPacketList = new PacketFilter(new LengthPacketFilter(TXFrom.Text, TXTo.Text)).Filter(filteredPacketList);

                if (CBType.SelectedIndex != 0)
                {
                    if (CBType.SelectedIndex == 1)
                        filteredPacketList = filteredPacketList.Where(x => x.SourceIp.Equals(IPAddress.Loopback) || x.SourceIp.Equals(IP_Address));
                    else if (CBType.SelectedIndex == 2)
                        filteredPacketList = filteredPacketList.Where(x => x.DestinationIp.Equals(IPAddress.Loopback) || x.DestinationIp.Equals(IP_Address));
                }
                UIControl.UpdateItemSourceList(filteredPacketList.ToList());
            }
        }

        // Navigation event ---------------------------
        
        private void OpenMenuEvent(object e, EventArgs ea)
        {
            var button = (System.Windows.Controls.Primitives.ToggleButton)e;
            if (button.Name == "OptionButton")
            {
                OptionButton.IsChecked = true;
                OptionMenu.Visibility = Visibility.Visible;
                HomeMenu.Visibility = Visibility.Hidden;
                HomeButton.IsChecked = false;
            }
            else
            {
                OptionButton.IsChecked = false;
                OptionMenu.Visibility = Visibility.Hidden;
                HomeMenu.Visibility = Visibility.Visible;
                HomeButton.IsChecked = true;
            }
        }
        // Write to file button click
        private void WriteButtonEvent(object e, EventArgs ea)
        {
            try
            {
                if ((bool)Rjson.IsChecked)
                    new PacketSaver(FileDialogText.Text, new JsonPacketSaver()).Save(Monitor.packets);
                else if ((bool)Rtxt.IsChecked)
                    new PacketSaver(FileDialogText.Text, new TxtPacketSaver()).Save(Monitor.packets);
            }
            catch (ArgumentException p)
            {
                MessageBox.Show(p.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ComboboxOpen(object e, EventArgs ea)
        {
           var item = (e as ComboBox).SelectedIndex;
           ProcessList.ItemsSource = Process.GetProcesses();
           ProcessList.SelectedIndex = item;
        }
        private void CheckIfNumber(object o, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[\\.0-9]*$");
            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
