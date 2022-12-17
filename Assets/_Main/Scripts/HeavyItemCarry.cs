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
        if (Input.GetKeyUp(KeyCode.E))
        {
            CmdCarryItem(true, cm.transform, cm);
            rb.isKinematic = true;
        }
        
        if (Input.GetMouseButton(0))
        {
            rb.isKinematic = false;
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

        transform.parent = _transform;

        if (boolean)
        {
            transform.localPosition = new Vector3(0, 0.75f, 1.25f);
            StatusEffect.AddStatusEffect(StatusEffectType.SLOW, cm.gameObject.GetComponent<PlayerStatusEffectHandler>());
            return;
        }
        
        StatusEffect.RemoveStatusEffect(StatusEffectType.SLOW, cm.gameObject.GetComponent<PlayerStatusEffectHandler>());
    }
}
