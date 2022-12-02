using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public Light light;
    public bool lightOnOff;

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(0);
            NetworkServer.Destroy(gameObject);
        }
    }
}
