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
    [SerializeField] private NetworkGamePlayerLobby[] gamePlayerPrefab = null;
    private int _gamePlayerSelect = 0;


    [SerializeField] private int minPlayers = 2;

    public static event Action OnClientConnected;
    public static event Action OnClientDisconnected;

    public List<NetworkRoomPlayerLobby> roomPlayers = new List<NetworkRoomPlayerLobby>();
    public List<NetworkGamePlayerLobby> gamePlayers = new List<NetworkGamePlayerLobby>();
    public int readyPlayerCount;

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
        if (numPlayers > readyPlayerCount)
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
            
            ServerChangeScene("Gameplay");
        }
    }

    public override void ServerChangeScene(string newSceneName)
    {
        if (SceneManager.GetActiveScene().name == menuScene &&  newSceneName.StartsWith("Gameplay"))
        {
            for (int i = roomPlayers.Count - 1; i >= 0 ; i--)
            {
                var conn = roomPlayers[i].connectionToClient;
                
                if (roomPlayers.Count == 1)
                {
                    spawnPoint = new Vector3(0,2.65f,3.15f);
                    _gamePlayerSelect = roomPlayers[0].characterInt;
                }
                
                else if(roomPlayers.Count == 2)
                {
                    spawnPoint = new Vector3(1,2.65f,3.15f);
                    _gamePlayerSelect = roomPlayers[1].characterInt;
                }
                
                else if(roomPlayers.Count == 3)
                {
                    spawnPoint = new Vector3(2,2.65f,3.15f);
                    _gamePlayerSelect = roomPlayers[2].characterInt;
                }
                
                else if(roomPlayers.Count == 4)
                {
                    spawnPoint = new Vector3(3,2.65f,3.15f);
                    _gamePlayerSelect = roomPlayers[3].characterInt;
                }
                
                var gameplayerInstance = Instantiate(gamePlayerPrefab[_gamePlayerSelect], spawnPoint, quaternion.identity);
                gameplayerInstance.SetDisplayName(roomPlayers[i].DisplayName);
                NetworkServer.Destroy(conn.identity.gameObject);
                NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
            }
        }
        
        base.ServerChangeScene(newSceneName);
    }
}
