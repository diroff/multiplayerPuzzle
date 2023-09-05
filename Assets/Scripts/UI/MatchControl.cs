using Mirror;

public class MatchControl : NetworkBehaviour
{
    public void Disconnect()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            NetworkManager.singleton.StopHost();
        else if(NetworkClient.isConnected)
            NetworkManager.singleton.StopClient();
        else if(NetworkServer.active)
            NetworkManager.singleton.StopServer();
    }
}