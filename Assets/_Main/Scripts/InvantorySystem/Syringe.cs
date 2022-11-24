using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Syringe : NetworkBehaviour, IInteractable
{
    public Item item = Item.Syringe;
    
    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(item);
            CmdInteract();
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdInteract()
    {
        NetworkServer.Destroy(gameObject);
    }
}
