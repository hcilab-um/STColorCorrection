using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace STColorPerception.Util
{
    class bluetooth
    {
        private BluetoothClient client = null;
        private BluetoothListener serverSocket = null;
        private BinaryWriter bWriter = null;
        private BinaryReader bReader = null;

        private MainWindow mwObject = null;
        
       
        public Boolean IsConnected
        {
            get
            {
                if (client == null)
                    return false;
                return client.Connected;
            }
        }

        public bluetooth(Guid applicationGuid, MainWindow mwObject)
        {
            serverSocket = new BluetoothListener(applicationGuid);
            serverSocket.Start();
            
            serverWorker = new BackgroundWorker();
            serverWorker.DoWork += new DoWorkEventHandler(ServerWorker_DoWork);
            serverWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ServerWorker_RunWorkerCompleted);

            this.mwObject = mwObject;

           
        }

        public void Init()
        {
            isRunning = true;
            serverWorker.RunWorkerAsync();
        }

        void ServerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                client = serverSocket.AcceptBluetoothClient();
                Stream peerStream = client.GetStream();
                bWriter = new BinaryWriter(peerStream, Encoding.ASCII);
                bReader = new BinaryReader(peerStream, Encoding.ASCII);
                e.Result = true;
            }
            catch (Exception exception)
            {
                e.Result = false;
                Console.WriteLine(exception.Message);
            }
        }
        
        void ServerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isRunning)
                serverWorker.RunWorkerAsync();
        }

        private bool isRunning = false;
        private BackgroundWorker serverWorker = null;
        internal void Dispose()
        {
            try
            {
                isRunning = false;
                serverSocket.Stop();
            }
            catch (Exception exception)
            { }
        }
    }
}

 