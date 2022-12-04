using UnityEngine;
using Mirror;

public class HeavyItemCarry : NetworkBehaviour, IInteractable
{
    private bool playerCarryThis = false;

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyUp(KeyCode.E) && !playerCarryThis)
        {
            CmdCarryItem(true, cm.transform, cm);
        }
        
        else if (Input.GetKeyUp(KeyCode.E) && playerCarryThis)
        {
            CmdCarryItem(false, null, cm);
        }
    }

    [Command(requiresAuthority = false)]
    private void CmdCarryItem(bool boolean, Transform transform, CharacterMechanic cm)
    {
        RpcCarryItem(boolean, transform, cm);
    }
    
    [ClientRpc]
    private void RpcCarryItem(bool boolean, Transform _transform, CharacterMechanic cm)
    {
        playerCarryThis = boolean;

        transform.parent = _transform;
        
        if (playerCarryThis)
        {
            transform.localPosition = new Vector3(0, 1, 2);
        }

        cm.CarryHeavyItemChangeSpeed(playerCarryThis);
    }
}
