using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    public class ForwardCommon
    {
        public const int VERSION = 1;
        public const int HEADER_SIZE = 256;
        public const int BUFFER_SIZE = 1 * 1024 * 1024; // 1MB
        public const string HOST_DOMAIN = "api.realwith.com";
        public const int HOST_CLIENT_PORT = 23389;
        public const int HOST_SERVER_PORT = 23390;

        protected void log(string msg)
        {
            Console.WriteLine($"[V][-----------] {msg}");
        }
        protected void logs(Socket clientSocket, string msg)
        {
            if (clientSocket == null || clientSocket.RemoteEndPoint == null || ((IPEndPoint)clientSocket.RemoteEndPoint).Address == null) log("NULL ==>> " + msg);
            else Console.WriteLine($"[S][{((IPEndPoint)clientSocket.RemoteEndPoint).Address.MapToIPv4()}] {msg}");
        }
        protected void logc(Socket clientSocket, string msg)
        {
            if (clientSocket == null || clientSocket.RemoteEndPoint == null || ((IPEndPoint)clientSocket.RemoteEndPoint).Address == null) log("NULL ==>> " + msg);
            else Console.WriteLine($"[C][{((IPEndPoint)clientSocket.RemoteEndPoint).Address.MapToIPv4()}] {msg}");
        }
    }

    public class ForwardServer : ForwardCommon
    {
        public class ServerRdpClient
        {
            public string name;
            public Socket socket;
            public NetworkStream stream;

            internal void Dispose()
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both); socket.Dispose();
                }
                catch { }
            }
        }

        private Dictionary<string, string> matchSources = new Dictionary<string, string>();
        private Dictionary<string, ServerRdpClient> serverRdpList = new Dictionary<string, ServerRdpClient>();

        public async Task Start()
        {
            ReadMatchSource();

            log($"Start Client Rdp Server ({HOST_CLIENT_PORT})");
            var clientRdpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientRdpServer.Bind(new IPEndPoint(IPAddress.Any, HOST_CLIENT_PORT));
            clientRdpServer.Listen(-100);
            Task t1 = AcceptClientRdpAsync(clientRdpServer);

            log($"Start Server Rdp Server ({HOST_SERVER_PORT})");
            var serverRdpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverRdpServer.Bind(new IPEndPoint(IPAddress.Any, HOST_SERVER_PORT));
            serverRdpServer.Listen(-100);
            Task t2 = AcceptServerRdpAsync(serverRdpServer);

            while (!t1.IsCompleted && !t1.IsFaulted && !t2.IsCompleted && !t2.IsFaulted)
            {
                Thread.Sleep(100);
            }
            log("End of Servers");
        }

        private async Task AcceptServerRdpAsync(Socket server)
        {
            while (true)
            {
                var acceptSocket = await server.AcceptAsync();
                if (acceptSocket != null)
                {
                    try
                    {
                        logs(acceptSocket, "Server Rdp Accept");
                        var acceptStream = new NetworkStream(acceptSocket);

                        // 데이터 읽기
                        byte[] dataReader = new byte[256];
                        int size = await acceptStream.ReadAsync(dataReader, 0, dataReader.Length);
                        using (var st = new MemoryStream(dataReader, false))
                        using (var reader = new BinaryReader(st, Encoding.UTF8))
                        {
                            int version = reader.ReadInt32();
                            if (version != VERSION)
                            {
                                logs(acceptSocket, $"Version missmatch version ({version} != {VERSION})");
                            }
                            string name = reader.ReadString();

                            // 이전 소켓 제거
                            if (serverRdpList.ContainsKey(name))
                            {
                                serverRdpList[name].Dispose();
                            }

                            // 새로운 소켓 삽입
                            serverRdpList[name] = new ServerRdpClient
                            {
                                socket = acceptSocket,
                                stream = acceptStream
                            };
                        }
                    }
                    catch (Exception e)
                    {
                        logs(acceptSocket, e.Message);
                    }
                }
            }
            log("End of server rdp accept");
        }

        private async Task AcceptClientRdpAsync(Socket server)
        {
            while (true)
            {
                var clientSocket = await server.AcceptAsync();
                try
                {
                    ClientRun(clientSocket);
                }
                catch (Exception e)
                {
                    logs(clientSocket, e.Message);
                }
            }
            log("End of client rdp accept");
        }

        private async Task ClientRun(Socket remote)
        {
            logc(remote, "Client Rdp Accept");

            // Wait or match connection
            ServerRdpClient server = await GetServerRdpClient(remote);
            if (server == null) return;

            logc(remote, "Server Rdp Found");

            using (var remoteStream = new NetworkStream(remote))
            using (var localStream = server.stream)
            {
                // 데이터 송수신
                Task receive = WorkStream("Receive", remoteStream, localStream);
                Task send = WorkStream("Send", localStream, remoteStream);
                while (!receive.IsCompleted && !receive.IsFaulted && !send.IsCompleted && !send.IsFaulted)
                {
                    Thread.Sleep(1);
                }
            }
        }

        private async Task<ServerRdpClient> GetServerRdpClient(Socket socket)
        {
            string ip = ((IPEndPoint)socket.RemoteEndPoint).Address.MapToIPv4().ToString();
            while (true)
            {
                ReadMatchSource();
                if (!matchSources.TryGetValue(ip, out var name))
                {
                    logc(socket, $"Not available ip address. {ip}");
                    socket.Shutdown(SocketShutdown.Both);
                    return null;
                }

                if (serverRdpList.TryGetValue(name, out var server))
                {
                    await server.stream.WriteAsync(new byte[] { 1 }); // 데이터 전송용 데이터
                    serverRdpList.Remove(name);
                    return server;
                }
                else
                {
                    logc(socket, $"Wait for server rdp connection. (name={name})");
                    await server.stream.WriteAsync(new byte[] { 0 }); // 소켓 유지용 데이터
                    await Task.Delay(1000);
                }
            }
        }

        private void ReadMatchSource()
        {
            matchSources.Clear();
            try
            {
                foreach (var line in File.ReadAllLines("matches.txt", Encoding.UTF8))
                {
                    if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                    {
                        continue;
                    }

                    var param = line.Split("\t");
                    matchSources.Add(param[0], param[1]);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Not found matches.txt (ip\tname)");
            }
        }

        private async Task WorkStream(string tag, NetworkStream read, NetworkStream write)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            try
            {
                while (read.CanRead && write.CanWrite)
                {
                    int size = await read.ReadAsync(buffer, 0, buffer.Length);
                    if (size > 0) 
                    {
                        //log($"{tag} => {size}");
                        await write.WriteAsync(buffer, 0, size);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(tag + " / " + e);
            }
        }
    }

    /// <summary>
    /// RDP를 전달받을 원격지 컴퓨터에서 실행되며,
    /// 언제나 서버에 접속하려고한다.
    /// 서버에서 신호가 전달되면 곧바로 내부 RDP서비스에 접속하여 중계한다.
    /// </summary>
    public class ForwardClient : ForwardCommon
    {
        Socket proxy;
        Socket localRdpSocket;

        public string name; // 1~32자

        public async Task Start()
        {
            EnsureName();

            while (true)
            {
                try { proxy.Shutdown(SocketShutdown.Both); proxy.Disconnect(false); } catch { } finally { proxy = null; }
                try { localRdpSocket.Shutdown(SocketShutdown.Both); proxy.Disconnect(false); } catch { } finally { localRdpSocket = null; }
                try
                {
                    using (proxy = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        log($"Connecting to {HOST_DOMAIN}:{HOST_SERVER_PORT}");
                        await proxy.ConnectAsync(HOST_DOMAIN, HOST_SERVER_PORT);
                        if (!proxy.Connected)
                        {
                            await Task.Delay(1000);
                            continue;
                        }

                        using (var remoteStream = new NetworkStream(proxy))
                        {
                            // 헤더 데이터 전송
                            logc(proxy, "Send RdpServerHeader");
                            byte[] data = MakeHeader();
                            await remoteStream.WriteAsync(data, 0, data.Length);

                            // 접속까지 대기
                            logc(proxy, "Wait for connection");
                            if (await WaitForConnection(remoteStream))
                            {
                                // 원격용 소켓 접속
                                await ConnectLocalRdp();
                                logs(localRdpSocket, "connecting");

                                using (var localStream = new NetworkStream(localRdpSocket))
                                {
                                    // 데이터 송수신
                                    Task receive = WorkStream("Receive", remoteStream, localStream);
                                    Task send = WorkStream("Send", localStream, remoteStream);
                                    while (!receive.IsCompleted && !receive.IsFaulted && !send.IsCompleted && !send.IsFaulted)
                                    {
                                        Thread.Sleep(1);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private async Task<bool> WaitForConnection(NetworkStream remoteStream)
        {
            byte[] data = new byte[1];
            while (true)
            {
                int readBytes = await remoteStream.ReadAsync(data, 0, 1);
                if (readBytes > 0)
                {
                    if (data[0] == 1) // 데이터가 1이 넘어오면
                    {
                        return true;
                    }
                }
            }
        }

        private void EnsureName()
        {
            if (!File.Exists("name.txt")) throw new FileNotFoundException("Not found 'name.txt'");
            name = File.ReadAllText("name.txt", Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(name) || name.Length > 32)
            {
                throw new FileNotFoundException("'name.txt' file is empty or white space or length over than 32");
            }
        }

        private byte[] MakeHeader()
        {
            EnsureName();
            byte[] data = new byte[HEADER_SIZE];
            using (var st = new MemoryStream(data, true))
            using (var w = new BinaryWriter(st, Encoding.UTF8))
            {
                w.Write(VERSION);
                w.Write(name);
            }

            return data;
        }

        private async Task ConnectLocalRdp()
        {
            localRdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await localRdpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3389));
        }

        private async Task WorkStream(string tag, NetworkStream read, NetworkStream write)
        {
            byte[] buffer = new byte[BUFFER_SIZE];
            try
            {
                while (read.CanRead && write.CanWrite)
                {
                    int size = await read.ReadAsync(buffer, 0, buffer.Length);
                    if (size > 0)
                    {
                        //log($"{tag} => {size}");
                        await write.WriteAsync(buffer, 0, size);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(tag + " / " + e);
            }
        }
    }
}
