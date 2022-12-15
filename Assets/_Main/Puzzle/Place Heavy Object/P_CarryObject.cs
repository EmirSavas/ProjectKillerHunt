using System;
using UnityEngine;

public class P_CarryObject : MonoBehaviour
{
    public bool completed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            Completed();
        }
    }

    private void Completed()
    {
        completed = true;
    }
}
