using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class HeavyItemCarry : NetworkBehaviour, IInteractable
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(CharacterMechanic cm, CharacterMovement characterMovement)
    {
        if (Input.GetKeyUp(KeyCode.E) && !cm.carryHeavyItem)
        {
            CmdCarryItem(true, cm.transform, cm);
            rb.isKinematic = true;
            cm.carryHeavyItem = true;
        }
        
        if (Input.GetMouseButton(0) && cm.carryHeavyItem)
        {
            rb.isKinematic = false;
            cm.carryHeavyItem = false;
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
        cm.ResetItemInHand();
        
        cm.carryHeavyItem = boolean;

        transform.parent = _transform;
        
        if (cm.carryHeavyItem)
        {
            transform.localPosition = new Vector3(0, 0.75f, 1.25f);
        }

        cm.CarryHeavyItemChangeSpeed(cm.carryHeavyItem);
    }
}
