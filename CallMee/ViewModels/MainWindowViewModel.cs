using System;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
// bunu her zaman eklememiz lazim
using System.IO; //StreamReader ve StreamWriter siniflari için
using System.Net.Sockets;
using System.Threading;

// Socket, TcpListener ve NetworkStrem siniflari için


namespace CallMee.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "SERVER";
        private ObservableCollection<string> list;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public DelegateCommand ClickMe { get; private set; }

        public MainWindowViewModel()
        {
            List = new ObservableCollection<string>();
            Thread t = new Thread(new ThreadStart(Start));
            t.Start();
            ClickMe = new DelegateCommand(Start);
        }


        public ObservableCollection<string> List
        {
            get { return list; }
            set { SetProperty(ref list, value); }
        }
        private void Start()
        {
         

            TcpListener TcpDinleyicisi = new TcpListener(1234);
            TcpDinleyicisi.Start();

            List.Add("Sunucu baslatildi...");

          
            Socket IstemciSoketi = TcpDinleyicisi.AcceptSocket();


            
            if (!IstemciSoketi.Connected)
            {
                List.Add("Sunucu baslatilamiyor...");
            }
            else
            {
         
                while (true)
                {
                   // List.Add("Istemci baglantisi saglandi...");

         
                    NetworkStream AgAkimi = new NetworkStream(IstemciSoketi);

                   
                    StreamWriter AkimYazici = new StreamWriter(AgAkimi);
                    StreamReader AkimOkuyucu = new StreamReader(AgAkimi);


                 
                    try
                    {
                        string IstemciString = AkimOkuyucu.ReadLine();

                        MessageBox.Show("Gelen Bilgi:" + IstemciString);

                        
                        int uzunluk = IstemciString.Length;

                        
                        AkimYazici.WriteLine(uzunluk.ToString());

                        AkimYazici.Flush();
                    }

                    catch
                    {
                        List.Add("Sunucu kapatiliyor...");
                        return;
                    }
                }
            }
        }
    }
}
