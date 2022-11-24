using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public Light light;

    [SyncVar]private bool lightOnOff;
    
    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(0);
            NetworkServer.Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CmdOpenFlashlight();
            lightOnOff = !lightOnOff;
        }
    }

    [Command]
    private void CmdOpenFlashlight()
    {
        RpcOpenFlashlight();
    }

    [ClientRpc]
    private void RpcOpenFlashlight()
    {
        light.enabled = lightOnOff;
    }

}
