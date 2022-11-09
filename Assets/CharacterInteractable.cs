using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CharacterInteractable : NetworkBehaviour
{
    public CharacterMovement player;
    public LayerMask hideableObjLayer;
    private Hideable hideableObj;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        DetectingHideableObject();
    }
    
    private void DetectingHideableObject()
    {
        RaycastHit objectHit;        

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.forward, out objectHit, 20, hideableObjLayer))
        {
            hideableObj = objectHit.collider.GetComponent<Hideable>();
            
            if (Input.GetKeyDown(KeyCode.E) && !player.thisPlayerInside)
            {
                hideableObj.CheckPlayer(netIdentity);

            }
        }
        else
        {

            if (Input.GetKeyDown(KeyCode.E) && player.thisPlayerInside)
            {
                hideableObj.CheckPlayer(netIdentity);
            }
        }
    }
}
