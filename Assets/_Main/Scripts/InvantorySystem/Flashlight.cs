using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public Light light;
    public bool lightOnOff;
    [SyncVar(hook = nameof(OnLightStatusChanged))]
    public bool lightStatusSynced;

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(0);
            NetworkServer.Destroy(gameObject);
        }
    }
    
    [Command(requiresAuthority = false)]
    public void CmdOpenFlashlight(bool value)
    {
        lightStatusSynced = value;
    }

    void OnLightStatusChanged(bool _Old, bool _New)
    {
        light.enabled = lightOnOff;
    }
}
