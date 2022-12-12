using UnityEngine;

public class P_CarryObject : MonoBehaviour
{
    public bool completed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "PA_04_GasCanister_01")
        {
            Completed();
        }
    }

    private void Completed()
    {
        completed = true;
    }
}
