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
        rb.isKinematic = !rb.isKinematic;
        
        if (rb.isKinematic) CmdCarryItem(true, cm.transform, cm);
        else CmdCarryItem(false, null, cm);
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
