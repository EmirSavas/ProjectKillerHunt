using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Pong;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")] [SerializeField] private GameObject lobbyUI = null;
    [SerializeField] private Text[] playerNameText = new Text[4];
    [SerializeField] private Text[] playerReadyTexts = new Text[4];
    [SerializeField] private Text readyButton = null;
    [SerializeField] private Button startGameButton = null;

    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    private bool isLeader;

    public bool IsLeader
    {
        set
        {
            isLeader = value;
            startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;

    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room;}

            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartAuthority()
    {
        CmdSetDisplayName(PlayerNameInput.DisplayName);

        lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.roomPlayers.Add(this);

        UpdateDisplay();
    }

    public override void OnStopServer()
    {
        Room.roomPlayers.Remove(this);
        
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue)
    {
        UpdateDisplay();
    }

    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (!hasAuthority)
        {
            foreach (var player in Room.roomPlayers)
            {
                if (player.hasAuthority)
                {
                    player.UpdateDisplay();
                    break;
                }
            }
            
            return;
        }

        for (int i = 0; i < playerNameText.Length; i++)
        {
            playerNameText[i].text = "Waiting For Player...";
            playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.roomPlayers.Count; i++)
        {
            playerNameText[i].text = Room.roomPlayers[i].DisplayName;
            playerReadyTexts[i].text = Room.roomPlayers[i].IsReady
                ? "<color=green>Ready</color>"
                : "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!isLeader)
        {
            return;
        }

        startGameButton.interactable = readyToStart;
    }

    [Command]
    public void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }
    
    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        if (IsReady)
        {
            readyButton.text = "Not Ready";
        }

        else
        {
            readyButton.text = "Ready";
        }

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.roomPlayers[0].connectionToClient != connectionToClient) { return;}

        //Start Game
         if (Room.roomPlayers.Count >= 1)
         {
             Debug.Log("GameStarting");
             Room.StartGame();
         }
    }
}
