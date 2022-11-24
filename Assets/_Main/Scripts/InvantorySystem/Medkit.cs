using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Medkit : NetworkBehaviour, IInteractable
{
    public Item item = Item.MEDKIT;
    
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
