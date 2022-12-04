using Mirror;
using UnityEngine;

public class Key : NetworkBehaviour, IInteractable
{
    public Item item = Item.KEY;
    
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
