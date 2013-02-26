using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace server_bluetoothexample
{
  class Bluetooth
  {
    private BluetoothClient client = null;
    private BluetoothListener serverSocket = null;
    private BinaryWriter bWriter = null;
    //  private BinaryReader bReader = null;

    private MainWindow mwObject = null;
    private BackgroundWorker serverWorker = null;


    
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
      serverSocket = new BluetoothListener(applicationGuid);
      serverSocket.Start();

      serverWorker = new BackgroundWorker();

      this.mwObject = mwObject;
    }

    public void Init()
    {
      isRunning = true;

      serverWorker.DoWork += new DoWorkEventHandler(ServerWorker_DoWork);
      serverWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ServerWorker_RunWorkerCompleted);
      serverWorker.RunWorkerAsync();
    }

    void ServerWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      try
      {
        client = serverSocket.AcceptBluetoothClient();
        Stream peerStream = client.GetStream();
        bWriter = new BinaryWriter(peerStream, Encoding.ASCII);
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
      else if (isRunning)
        serverWorker.RunWorkerAsync();
    }

    public void SendRGB(string color_string)
    {

      bWriter.Write(color_string);
      bWriter.Flush();
      //sample 2nd string being passed for testing !!
      string temp = "FFFFFF";
      
      bWriter.Write(temp);
      bWriter.Flush();
    }

    ///// <summary>
    ///// BitEndian/LittleEndian incompatibility for Win -vs- Android
    ///// </summary>
    ///// <param name="intValue"></param>
    //private void WriteInt(int intValue)
    //{
    //    byte[] intValueBytes = BitConverter.GetBytes(intValue);
    //    bWriter.Write(intValueBytes[3]);
    //    bWriter.Write(intValueBytes[2]);
    //    bWriter.Write(intValueBytes[1]);
    //    bWriter.Write(intValueBytes[0]);
    //    bWriter.Flush();
    //}

    private bool isRunning = false;

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

