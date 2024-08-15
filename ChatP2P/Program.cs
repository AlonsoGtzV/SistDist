namespace ChatP2P;

public class Program{
    public static async Task Main(string[] args){
        var peer = new Peer();
        if (args.Length > 0 && args[0] == "connect" 
            && !string.IsNullOrEmpty(args[1]) 
            && !string.IsNullOrEmpty(args[2]))        {

            // Convertir args[2] a int
            if (int.TryParse(args[2], out int port)){
                await peer.ConnectToPeer(args[1], port);
            } else {
                Console.WriteLine("El segundo argumento debe ser un número válido (puerto).");
            }
        }
        else
        {
            await peer.StartListening();
        }
    }
}
