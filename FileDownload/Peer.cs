using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

namespace FileDownload;

public class Peer{
    private readonly TcpListener _listener;
    private TcpClient _client;
    private const int port = 8080;

    public Peer(){
        _listener = new TcpListener(IPAddress.Any, port);

    }

    public async Task DownloadFile(string peerIP, int peerPort, string fileName, string savePath, CancellationToken cancellationToken){
        _client = new TcpClient(peerIP, peerPort);
        await using var stream = _client.GetStream();
        var request = Encoding.UTF8.GetBytes(fileName);
        await stream.WriteAsync(request, cancellationToken);

        await using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
        var buffer = new byte[1024];
        int bytesRead;
        while((bytesRead = await stream.ReadAsync(buffer, cancellationToken))>0){
            await fs.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
        }

        Console.WriteLine($"El archivo {fileName} se ha descargado en la ruta {savePath}");
    }

    public async Task Start(CancellationToken cancellationToken){
        _listener.Start();
        while (true)
        {
            _client = await _listener.AcceptTcpClientAsync(cancellationToken);
            await HandleClient(cancellationToken);
        }
    }

    public async Task HandleClient(CancellationToken cancellationToken){
        await using var stream = _client.GetStream();
        var buffer = new byte[1024];
        var bytesRead = await stream.ReadAsync(buffer, cancellationToken);
        var fileName = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        if (File.Exists(fileName))
        {
            var fileData = await File.ReadAllBytesAsync(fileName, cancellationToken);
            await stream.WriteAsync(fileData, cancellationToken);
            Console.WriteLine($"File {fileName} sent to client");
        }else{
            var errorMessage = Encoding.UTF8.GetBytes("File not found");
            await stream.WriteAsync(errorMessage, cancellationToken);
            Console.WriteLine($"File {fileName} not found");
        }
    }
}