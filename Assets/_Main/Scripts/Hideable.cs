using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class Hideable : NetworkBehaviour
{
    public Transform referansPoint;

    public GameObject uiPanel;

    public float radius = 10f;

    [SyncVar] public bool isEmpty;

    
    

    public void CheckPlayer(NetworkIdentity id)
    {
        CharacterMovement player = id.GetComponent<CharacterMovement>();
        
        if (!player.thisPlayerInside && !isEmpty)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = this.transform.position;
            player.thisPlayerInside = true;
            CmdCheckPlayer(true);
        }

        else if (player.thisPlayerInside)
        {
            player.transform.position = referansPoint.position;
            player.GetComponent<CharacterController>().enabled = true;
            player.thisPlayerInside = false;
            CmdCheckPlayer(false);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdCheckPlayer(bool empty)
    {
        isEmpty = empty;
    }

    
}
    
        

