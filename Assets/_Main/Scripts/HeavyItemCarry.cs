using System;
using UnityEngine;
using Mirror;

public class HeavyItemCarry : NetworkBehaviour, IInteractable
{
    private bool playerCarryThis = false;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyUp(KeyCode.E) && !playerCarryThis)
        {
            CmdCarryItem(true, cm.transform, cm);
            rb.isKinematic = true;
        }
        
        else if (Input.GetKeyUp(KeyCode.E) && playerCarryThis)
        {
            CmdCarryItem(false, null, cm);
            rb.isKinematic = false;
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
            transform.localPosition = new Vector3(0, 0.75f, 2);
        }

        cm.CarryHeavyItemChangeSpeed(playerCarryThis);
    }
}
