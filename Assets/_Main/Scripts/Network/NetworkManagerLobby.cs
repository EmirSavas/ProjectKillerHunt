using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [Scene] private string menuScene = "Scene_Lobby"; //Lobi adiyla esit olmasi lazim!

    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    
    [SerializeField] private int minPlayers = 2;
    
    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> roomPlayers { get; } = new List<NetworkRoomPlayerLobby>();

    public override void OnStartServer()
    {
        spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();
    }

    public override void OnStartClient()
    {
        var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

        foreach (var prefab in spawnablePrefabs)
        {
            NetworkClient.RegisterPrefab(prefab);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnClientConnected?.Invoke();
    }
    
    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnClientDisconnected?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (numPlayers >= maxConnections)
        {
            conn.Disconnect();
            return;;
        }

        if (SceneManager.GetActiveScene().name != menuScene)
        {
            conn.Disconnect();
            return;
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            bool isLeader = roomPlayers.Count == 0;
            
            NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

            roomPlayerInstance.IsLeader = isLeader; 

            NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);

            if (!roomPlayerInstance.hasAuthority)
            {
                roomPlayerInstance.gameObject.SetActive(false);
            }
        }
    }
    
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (conn.identity != null)
        {
            var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

            roomPlayers.Remove(player);

            NotifyPlayersOfReadyState();
            
            base.OnServerDisconnect(conn);
        }
    }

    public override void OnStopServer()
    {
        roomPlayers.Clear();
    }
    
    public void NotifyPlayersOfReadyState()
    {
        foreach (var player in roomPlayers)
        {
            player.HandleReadyToStart(IsReadyToStart());
        }
    }

    private bool IsReadyToStart()
    {
        if (numPlayers < minPlayers)
        {
            foreach (var player in roomPlayers)
            {
                if (!player.IsReady) { return false; }
            }
        }
        
        return true;
    }
}
