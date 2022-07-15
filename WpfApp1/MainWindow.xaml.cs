using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Cb_act.Items.Add("List");
            Cb_act.Items.Add("Kill");
            Cb_act.Items.Add("Create");
            Cb_act.Items.Add("Help");
        }
        public int port ;       
        public TcpClient client;
        public IPAddress ip ;
        public ObservableCollection<string> prcesses { get; set; } = new();
        public IPEndPoint ep;
        public BinaryReader br;
        public BinaryWriter bw;
        public void LOAD()
        {
            try
            {
               
                var msg = "";   
                var stream=client.GetStream();
                br = new BinaryReader(stream);
                msg =br.ReadString();
                prcesses = JsonConvert.DeserializeObject<ObservableCollection<string>>(msg);
                foreach (var pr in prcesses)
                {
                    lst.Items.Add(pr.ToString());
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ip = IPAddress.Parse(ipp.Text);
                port = int.Parse(portt.Text.ToString());
                ep = new IPEndPoint(ip, port);
                client=new TcpClient();
                client.Connect(ep);
                if (client.Connected)
                {
                    MessageBox.Show("Connected");
                }
                else
                {
                    MessageBox.Show("Cannot connect.");
                }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(Cb_act.SelectedItem != null)
            {
                if(Cb_act.SelectedItem.ToString() == "List")
                {
                    LOAD();
                }
                else if(Cb_act.SelectedItem.ToString() == "Create")
                {
                    var stream = client.GetStream();
                    var br = new BinaryWriter(stream);
                    br.Write(prcName.Text);
                }
                else if (Cb_act.SelectedItem.ToString() == "Kill")
                {
                    var stream = client.GetStream();
                    var br = new BinaryWriter(stream);
                    br.Write(prcName.Text);
                }
                else if (Cb_act.SelectedItem.ToString() == "Help")
                {
                    var stream = client.GetStream();
                    var br = new BinaryWriter(stream);
                    br.Write(prcName.Text);
                }
            }
        }

        private void Cb_act_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var stream = client.GetStream();
                var br = new BinaryWriter(stream);
                br.Write(Cb_act.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
