using System;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public new Light light;
    public bool lightOnOff;
    public Item item = Item.FLASHLIGHT;

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(item);
            //_renderer.materials[1].SetColor("_EmissionColor", Color.black); //todo Flashlight Emission 
            
            CmdInteract();
        }
    }
    
    [Server]
    private void CmdInteract()
    {
        NetworkServer.Destroy(gameObject);
    }
}
