using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace Processes
{
    public  class Program
    {

        public static void Main(string[] args)
        {
            string sts="";
            var ipAddress = IPAddress.Parse("192.168.0.109");
            var port = 63291;
            var endPoint = new IPEndPoint(ipAddress, port);
            var listener =new TcpListener(endPoint); 
            listener.Start();
            ObservableCollection<string> prcesses =new();
            
            foreach (var item in Process.GetProcesses())
            {   
                prcesses.Add(item.ProcessName);                
            }

            while (true)
            {

                var client = listener.AcceptTcpClient();
                var stream = client.GetStream();
                Task.Run(() =>
                {
                    while (true)
                    {
                        var br = new BinaryReader(stream);
                        var msgs = br.ReadString();
                        if (msgs == "List")
                        {
                            sts = "List";
                        }
                        else if (msgs == "Kill")
                        {
                            sts = "Kill";
                        }
                        else if (msgs == "Create")
                        {
                            sts = "Create";
                        }
                        else if (msgs == "Help")
                        {
                            sts = "Help";
                        }
                        
                        br = new BinaryReader(stream);
                        if (sts == "List")
                        {
                            var bwr = new BinaryWriter(stream);
                            var json = JsonConvert.SerializeObject(prcesses, Newtonsoft.Json.Formatting.Indented);
                            bwr.Write(json);
                            sts = "";
                        }
                        else if (sts == "Create")
                        {
                            var br1 = new BinaryReader(stream);
                            var msgs1 = br.ReadString();
                            Process.Start(new ProcessStartInfo(msgs1));
                            sts = "";
                        }
                        else if (sts == "Kill")
                        {
                            var br1 = new BinaryReader(stream);
                            var msgs1 = br.ReadString();
                            var lst=Process.GetProcessesByName(msgs1);
                            foreach (var proc in lst)
                            {
                                proc.Kill();
                            }
                            sts = "";
                        }
                        else if (sts == "Help")
                        {
                            var br1 = new BinaryReader(stream);
                            var msgs1 = br.ReadString();
                            Console.WriteLine(Process.GetProcesses().FirstOrDefault(t=>t.ProcessName==msgs1).Id);
                            sts = "";
                        }
                    }
                }); 
            
            }

        }
    }
}
