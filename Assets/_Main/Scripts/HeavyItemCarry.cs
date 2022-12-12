using System;
using System.Collections;
using UnityEngine;
using Mirror;
using UnityEditor.Experimental.GraphView;

public class HeavyItemCarry : NetworkBehaviour, IInteractable
{
    private Rigidbody rb;
    private CharacterMechanic _characterMechanic;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(CharacterMechanic cm)
    {
        if (Input.GetKeyUp(KeyCode.E) && !cm.carryHeavyItem)
        {
            _characterMechanic = cm;
            CmdCarryItem(true, _characterMechanic.transform, _characterMechanic);
            rb.isKinematic = true;
            cm.carryHeavyItem = true;
            gameObject.layer = 2;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && _characterMechanic.carryHeavyItem)
        {
            CmdCarryItem(false, null, _characterMechanic);
            rb.isKinematic = false;
            _characterMechanic.carryHeavyItem = false;
            StartCoroutine(BackToInteractable());
        }
    }

    private IEnumerator BackToInteractable()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.layer = 8;
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
        
        _characterMechanic.carryHeavyItem = boolean;

        transform.parent = _transform;
        
        if (_characterMechanic.carryHeavyItem)
        {
            transform.localPosition = new Vector3(0, 0.75f, 2);
        }

        cm.CarryHeavyItemChangeSpeed(_characterMechanic.carryHeavyItem);
    }
}
