using Mirror;
using UnityEngine;

public class Flashlight : NetworkBehaviour, IInteractable
{
    public Light light;
    public bool lightOnOff;
    public Item item = Item.FLASHLIGHT;
    
    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            cm.AddItemToInventory(item);
            CmdInteract();
        }
    }
    
    [Server]
    private void CmdInteract()
    {
        NetworkServer.Destroy(gameObject);
    }
}
