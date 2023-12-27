using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    private const int port = 8888;
    private const string serverIP = "��������IP��ַ";

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
            // �����ͻ���Socket
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // ���ӷ�����
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
            // ����첽���Ӳ���
            clientSocket.EndConnect(ar);

            Debug.Log("�����ӵ���������" + clientSocket.RemoteEndPoint);

            // ��ʼ���շ���������Ӧ
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
        // ����Ƿ������ݿɽ���
        if (state != null && clientSocket != null && clientSocket.Connected && clientSocket.Available > 0)
        {
            clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // ����첽���ղ���
            StateObject state = (StateObject)ar.AsyncState;
            Socket serverSocket = state.WorkSocket;

            // ��ȡ���յ�����
            int bytesRead = serverSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                string message = Encoding.ASCII.GetString(state.Buffer, 0, bytesRead);
                Debug.Log("���յ�����Ӧ��" + message);

                // ������յ�����Ӧ
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
            // �������ݵ�������
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
            // ����첽���Ͳ���
            Socket serverSocket = (Socket)ar.AsyncState;
            int bytesSent = serverSocket.EndSend(ar);
            Debug.Log("������������� " + bytesSent + " �ֽڵ�����");
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
