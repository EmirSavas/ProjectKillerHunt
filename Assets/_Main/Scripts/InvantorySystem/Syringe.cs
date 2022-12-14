using Mirror;
using UnityEngine;

public class Syringe : NetworkBehaviour, IInteractable
{
    public Item item = Item.SYRINGE;
    
    public void Interact(CharacterMechanic cm, CharacterMovement characterMovement)
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
