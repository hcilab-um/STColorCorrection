using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace bluetoothClient
{
  class Bluetooth
  {
    private BluetoothClient client = null;
    private BluetoothListener clientSocket = null;
    private BinaryReader bReader = null;

    private MainWindow mwObject = null;
    private BackgroundWorker clientWorker = null;

    static readonly Guid MyServiceUUID = new Guid("{a0000000-a000-a000-a000-a00000000000}");


    public Boolean IsConnected
    {
      get
      {
        if (client == null)
          return false;
        return client.Connected;
      }
    }

    public Bluetooth(Guid applicationGuid, MainWindow mwObject)
    {
      clientSocket = new BluetoothListener(applicationGuid);
      clientWorker = new BackgroundWorker();

      this.mwObject = mwObject;
    }

    public void Init()
    {
      isRunning = true;
      clientWorker.RunWorkerAsync();

      clientWorker.DoWork += new DoWorkEventHandler(ClientWorker_DoWork);
      clientWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ServerWorker_RunWorkerCompleted);
    }

    void ClientWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        BluetoothAddress addr = BluetoothAddress.Parse("000272334418");

        var ep = new BluetoothEndPoint(addr, MyServiceUUID);// new Guid("{00000000-5820-0932-80f8-ffff00000000}"), );

        client = new BluetoothClient();
        client.Connect(ep);

        Stream peerStream = client.GetStream();

        //Stream peerStream = client.GetStream();
        //bWriter = new BinaryWriter(peerStream, Encoding.ASCII);
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
      if (e.Error != null)
      {
        Console.WriteLine("error");
      }
      //else if (isRunning)
      //  clientWorker.RunWorkerAsync();
    }

    public string ReciveColor()
    {
      string temp1;//,temp2;
      temp1 = bReader.ReadString();
      //temp2 = bReader.ReadString();

      //sample 2nd string being passed for testing !!
      return temp1;
    }

    private bool isRunning = false;

    internal void Dispose()
    {
      try
      {
        isRunning = false;
        clientSocket.Stop();
      }
      catch (Exception exception)
      { }
    }
  }
}

