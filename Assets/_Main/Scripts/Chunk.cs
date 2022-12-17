using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public List<Chunk> neighborChunks;
    public List<StatusEffectType> effects;
    
    
    // private bool isPoisonedStage1 = false;
    // private bool isPoisonedStage2 = false;
    // private float timerPoison;
    // private float waitToResetPoison;
    // private bool resetPoisonTimer;
    // private bool stateReset = false;

    private void OnTriggerStay(Collider other)
    {
        if (effects.Count <= 0) return;
        
        if (other.CompareTag("Player"))
        {
            if (effects.Contains(StatusEffectType.POISON))
            {
                var effectHandler = other.GetComponent<PlayerStatusEffectHandler>();

                effectHandler.poisonTimer += Time.deltaTime;
                
                if (effectHandler.poisonTimer >= 5 && !effectHandler.currentEffects.Contains(StatusEffectType.POISON))
                {
                    StatusEffect.AddStatusEffect(StatusEffectType.POISON, effectHandler);
                }
            }
        }
    }
    
    /*private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Poison"))
        {
            resetPoisonTimer = false;
            stateReset = false;
            waitToResetPoison = 0;

            if (timerPoison >= 0 && timerPoison <= 10)
            {
                timerPoison += Time.deltaTime;
                
                if (timerPoison >= 5)
                {
                    isPoisonedStage1 = true;
                    poisonImage[0].enabled = true;
                    poisonImage[1].enabled = false;
                }
            
                if (timerPoison >= 10)
                {
                    isPoisonedStage2 = true;
                    poisonImage[0].enabled = false;
                    poisonImage[1].enabled = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Poison"))
        {
            if (timerPoison > 0 && timerPoison < 5)
            {
                resetPoisonTimer = true;
            }

            else if (timerPoison > 5 && timerPoison < 10)
            {
                resetPoisonTimer = true;
                stateReset = true;
            }
        }
    }

    private void ReducerToPoisonTimer()
    {
        if (resetPoisonTimer && waitToResetPoison <= 2 && timerPoison != 0)
        {
            waitToResetPoison += Time.deltaTime;
        }

        if (waitToResetPoison >= 2)
        {
            if (timerPoison > 0)
            {
                timerPoison -= Time.deltaTime;
            }

            if (timerPoison < 0)
            {
                timerPoison = 0;
                resetPoisonTimer = false;
                waitToResetPoison = 0;
            }

            if (timerPoison < 5 && stateReset)
            {
                timerPoison = 5;
                resetPoisonTimer = false;
                waitToResetPoison = 0;
                stateReset = false;
            }
        }
    }

    private void HealingPoisonReset()
    {
        if (isPoisonedStage1 && !isPoisonedStage2)
        {
            isPoisonedStage1 = false;
           
            poisonImage[0].enabled = false;
            
            timerPoison = 0;
           
            resetPoisonTimer = false;
           
            stateReset = false;
           
            waitToResetPoison = 0;
            
            inventorySystem.DeleteItemToSlot(selectedItem);

            ResetItemInHand();
        }
    }*/
}
