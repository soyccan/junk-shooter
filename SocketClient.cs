using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class SocketClient
{
    private Socket socketClient;
    private Thread thread;
    private byte[] data = new byte[1024];
    public bool isTrigger;
    public float x;
    public float y;
    public float z;
    public float w;
    private char[] delimiter = { ' ', '\n' };

    public SocketClient(string hostIP, int port) {
        thread = new Thread(() => {
        // while the status is "Disconnect", this loop will keep trying to connect.
        while (true) {
            try {
                socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socketClient.Connect(new IPEndPoint(IPAddress.Parse(hostIP), port));
                // while the connection
                float[] quaternion = { 0, 0, 0, 0, 0 };
                float[] initial = { 0, 0, 0, 0, 0 };
                bool init = false;
                while (true) {
                    /*********************************************************
                     * TODO: you need to modify receive function by yourself *
                     *********************************************************/
                    if (socketClient.Available < 100) {
                        Thread.Sleep(1);
                        continue;
                    }
                    int length = socketClient.Receive(data);
                    string message = Encoding.UTF8.GetString(data, 0, length);
                    //Debug.Log("Recieve message: " + message);
                    string[] results = message.Split(delimiter);
                    quaternion[0] = float.Parse(results[0]);
                    for (int i = 1; i < 5; i++) {
                            quaternion[i] = (float)System.BitConverter.ToDouble(System.BitConverter.GetBytes(Int64.Parse(results[i])), 0);
                        }
                        if (!init){
                            for (int i = 0; i < 5; i++)
                            {
                                initial[i] = quaternion[i];
                            }
                            init = true;
                        }
                        isTrigger = Convert.ToBoolean(quaternion[0]);
                        /*w = quaternion[1] - initial[1];
                        x = quaternion[2] - initial[2];
                        y = quaternion[3] - initial[3];
                        z = quaternion[4] - initial[4];*/
                        w = quaternion[1];
                        z = -quaternion[2];
                        x = quaternion[3];
                        y = -quaternion[4];
                        /*Debug.Log("Initial: " + initial[1] + " " + initial[2] + " " + initial[3] + " " + initial[4]);
                        Debug.Log("Quaternion: " + w + " " + x + " " + y + " " + z);*/
                        // */
                    }
                } catch (Exception ex) {
                    if (socketClient != null) {
                        socketClient.Close();
                    }
                    Debug.Log(ex.Message);
                }
            }
        });
        thread.IsBackground = true;
        thread.Start();
    }

    public void Close() {
        thread.Abort();
        if (socketClient != null) {
            socketClient.Close();
        }
    }
}
