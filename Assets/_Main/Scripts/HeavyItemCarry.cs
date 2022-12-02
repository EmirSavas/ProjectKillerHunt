using UnityEngine;
using Mirror;

public class HeavyItemCarry : NetworkBehaviour, IInteractable
{
    private bool playerCarryThis = false;
    private CharacterMechanic cm;
    public void Interact(CharacterMechanic _cm)
    {
        cm = _cm;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !playerCarryThis)
        {
            CmdCarryItem(true, cm.transform, cm);
        }
        
        else if (Input.GetKeyDown(KeyCode.E) && playerCarryThis)
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
            transform.localPosition = new Vector3(0, 0, 2);
        }

        cm.CarryHeavyItemChangeSpeed(playerCarryThis);
    }
}
