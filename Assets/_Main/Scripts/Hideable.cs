using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mirror;
using UnityEngine;


public class Hideable : NetworkBehaviour
{
    [SyncVar] public bool isOpen;
    public float endValue;
    public Ease ease;

    private void Start()
    {
        //InvokeRepeating(nameof(Interact), 2, 2);
    }

    [Command(requiresAuthority = false)]
    public void CmdInteract()
    {
        RpcInteract();
    }
    
    [ClientRpc]
    public void RpcInteract()
    {
        if (isOpen) Close();
        
        else Move();

        isOpen = !isOpen;
    }
    

    public void Move()
    {
        transform.DOLocalRotate(new Vector3(0, endValue, 0), 0.85f).SetEase(ease);
    }
    
    public void Close()
    {
        transform.DOLocalRotate(Vector3.zero,  0.85f).SetEase(ease);
    }
}
    
        

