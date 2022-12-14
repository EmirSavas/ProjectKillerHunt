using System;
using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public Light light;
    public bool lightOnOff;
    public Item item = Item.FLASHLIGHT;

    public void Interact(CharacterMechanic cm, CharacterMovement characterMovement)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(item);
            //_renderer.materials[1].SetColor("_EmissionColor", Color.black); //todo Flashlight Emission 
            
            CmdInteract();
        }
    }
    
    [Command(requiresAuthority = false)]
    private void CmdInteract()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnDisable()
    {
        light.enabled = false;
    }
}
