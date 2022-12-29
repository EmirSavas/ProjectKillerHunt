using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


public class Hideable : NetworkBehaviour
{
    public float aimRotation;
    public float rotationSpeed;
    public float value;

    public void Update()
    {
        if (aimRotation >= transform.localEulerAngles.y)
        {
            value = Time.deltaTime * rotationSpeed;
            transform.eulerAngles += new Vector3(0, value, 0);
        }
        
        if (aimRotation <= transform.localEulerAngles.y)
        {
            value = Time.deltaTime * rotationSpeed;
            transform.eulerAngles += new Vector3(0, value, 0);
        }
    }

    /*public Transform referansPoint;

    public float radius = 10f;

    [SyncVar] public bool isEmpty;
    
    public void CheckPlayer(NetworkIdentity id)
    {
        CharacterMovement player = id.GetComponent<CharacterMovement>();
        
        if (!player.playerHiding && !isEmpty)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = this.transform.position;
            player.playerHiding = true;
            CmdCheckPlayer(true);
        }

        else if (player.playerHiding)
        {
            player.transform.position = referansPoint.position;
            player.GetComponent<CharacterController>().enabled = true;
            player.playerHiding = false;
            CmdCheckPlayer(false);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdCheckPlayer(bool empty)
    {
        isEmpty = empty;
    }*/
}
    
        

