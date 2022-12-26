using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerStatusEffectHandler : MonoBehaviour
{
    public float baseCheckFrequency;
    
    public Image[] poisonImage;
    public float poisonTimer;
    public List<StatusEffectType> currentEffects = new List<StatusEffectType>();

    private readonly List<StatusEffectType> _checkCurrentEffects = new List<StatusEffectType>();

    private float _checkFrequency;

    private void Awake()
    {
        _checkFrequency = baseCheckFrequency;
    }


    private void Update()
    {
        _checkFrequency -= Time.deltaTime;
        
        if (_checkFrequency <= 0)
        {
            CheckForNewStatusEffect();
            _checkFrequency = baseCheckFrequency;
        }
        
        if (Input.GetKeyDown(KeyCode.U))
        {
            StatusEffect.AddStatusEffect(StatusEffectType.SLOW, this);
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StatusEffect.RemoveStatusEffect(StatusEffectType.SLOW, this);
        }
    }
    
    private void UseStatusEffect(StatusEffectType effectType, UseType type)
    {
        switch (effectType)
        {
            case StatusEffectType.POISON:
                Effects.PoisonEffect(type, poisonImage[0], poisonTimer, FindObjectOfType<Volume>());
                break;
            
            case StatusEffectType.SLOW:
                Effects.ExhaustedEffect(type, GetComponent<CharacterMovement>());
                break;
        }
    }

    #region Status Effect Check

    private void CheckForNewStatusEffect()
    {
        if (currentEffects.Count == _checkCurrentEffects.Count)
        {
            Debug.Log("STABLE");
            return;
        }

        CheckForNewEffects();
        CheckForRemovedEffects();
    }

    private void CheckForNewEffects()
    {
        if(_checkCurrentEffects.Count > currentEffects.Count) return;
        
        for (int i = 0; i < currentEffects.Count; i++)
        {
            if (!_checkCurrentEffects.Contains(currentEffects[i]))
            {
                Debug.Log(currentEffects[i] + " Added");
                _checkCurrentEffects.Add(currentEffects[i]);
                UseStatusEffect(currentEffects[i], UseType.ADD);
            }
        }
    }

    private void CheckForRemovedEffects()
    {
        if(_checkCurrentEffects.Count <= currentEffects.Count) return;
        
        for (int i = 0; i < _checkCurrentEffects.Count; i++)
        {
            if (!currentEffects.Contains(_checkCurrentEffects[i]))
            {
                Debug.Log(_checkCurrentEffects[i] + " Removed");
                UseStatusEffect(_checkCurrentEffects[i], UseType.REMOVE);
                _checkCurrentEffects.RemoveAt(i);
            }
        }
    }

    #endregion
}

public enum UseType
{
    ADD,
    REMOVE
}
