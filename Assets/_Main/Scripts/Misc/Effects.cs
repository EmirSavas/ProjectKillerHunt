using UnityEngine;
using UnityEngine.UI;

public static class Effects
{
    public static void PoisonEffect(UseType type, Image poisonImage, float poisonTimer)
    {
        switch (type)
        {
            case UseType.ADD:
                poisonImage.enabled = true;
                break;
            
            case UseType.REMOVE:
                poisonImage.enabled = false;
                poisonTimer = 0;
                break;
        }
    }
    
    public static void ExhaustedEffect(UseType type, CharacterMovement movement)
    {
        switch (type)
        {
            case UseType.ADD:
                Debug.Log("EXHAUSTED");
                movement.speed = 1;
                break;
            
            case UseType.REMOVE:
                Debug.Log("NO EXHAUSTED");
                movement.speed = 2;
                break;
        }
    }
}
