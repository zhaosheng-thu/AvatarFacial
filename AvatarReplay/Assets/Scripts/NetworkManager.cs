using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    private const int port = 8888;
    private const string serverIP = "服务器的IP地址";

    private Socket clientSocket;
    private StateObject state;

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            // 创建客户端Socket
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 连接服务器
            IPAddress ipAddress = IPAddress.Parse(serverIP);
            IPEndPoint remoteEndPoint = new IPEndPoint(ipAddress, port);
            clientSocket.BeginConnect(remoteEndPoint, ConnectCallback, null);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // 完成异步连接操作
            clientSocket.EndConnect(ar);

            Debug.Log("已连接到服务器：" + clientSocket.RemoteEndPoint);

            // 开始接收服务器的响应
            state = new StateObject();
            state.WorkSocket = clientSocket;
            clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void Update()
    {
        // 检查是否有数据可接收
        if (state != null && clientSocket != null && clientSocket.Connected && clientSocket.Available > 0)
        {
            clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // 完成异步接收操作
            StateObject state = (StateObject)ar.AsyncState;
            Socket serverSocket = state.WorkSocket;

            // 读取接收的数据
            int bytesRead = serverSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                string message = Encoding.ASCII.GetString(state.Buffer, 0, bytesRead);
                Debug.Log("接收到的响应：" + message);

                // 处理接收到的响应
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void SendDataToServer(string message)
    {
        try
        {
            // 发送数据到服务器
            byte[] data = Encoding.ASCII.GetBytes(message);
            clientSocket.BeginSend(data, 0, data.Length, 0, SendCallback, clientSocket);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        try
        {
            // 完成异步发送操作
            Socket serverSocket = (Socket)ar.AsyncState;
            int bytesSent = serverSocket.EndSend(ar);
            Debug.Log("向服务器发送了 " + bytesSent + " 字节的数据");
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
}

public class StateObject
{
    public const int BufferSize = 1024;
    public byte[] Buffer = new byte[BufferSize];
    public Socket WorkSocket;
}
