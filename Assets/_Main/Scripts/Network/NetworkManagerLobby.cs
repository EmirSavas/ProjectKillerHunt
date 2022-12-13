using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkManagerLobby : NetworkManager
{
    [Scene] private string menuScene = "Scene_Lobby"; //Lobi adiyla esit olmasi lazim!

    [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;
    [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;


    [SerializeField] private int minPlayers = 2;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> roomPlayers = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> gamePlayers = new List<NetworkGamePlayerLobby>();

    public Vector3 spawnPoint;
    

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

    public void StartGame()
    {
        if (SceneManager.GetActiveScene().name == menuScene)
        {
            if (!IsReadyToStart())
            {
                return;
            }
            
            ServerChangeScene("Emre(Character)");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == menuScene &&  newSceneName.StartsWith("Emre(Character)"))
        {
            for (int i = roomPlayers.Count - 1; i >= 0 ; i--)
            {
                var conn = roomPlayers[i].connectionToClient;
                
                if (roomPlayers.Count == 1)
                {
                    spawnPoint = new Vector3(-3.036f,-1.8f,7.65f);
                }
                
                else if(roomPlayers.Count == 2)
                {
                    spawnPoint = new Vector3(-1.391495f,-1.8f,8.1f);
                }
                
                else if(roomPlayers.Count == 3)
                {
                    spawnPoint = new Vector3(1.391495f,-1.8f,8.5f);
                }
                
                else if(roomPlayers.Count == 4)
                {
                    spawnPoint = new Vector3(-5.391495f,-1.8f,9);
                }
                
                var gameplayerInstance = Instantiate(gamePlayerPrefab, spawnPoint, quaternion.identity);
                gameplayerInstance.SetDisplayName(roomPlayers[i].DisplayName);
                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }
        
        base.ServerChangeScene(newSceneName);
    }
}
