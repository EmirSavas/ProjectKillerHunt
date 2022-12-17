using Mirror;
using UnityEngine;
using UnityEngine.Internal;

public enum StatusEffectType
{
    POISON,
    SLOW
}

public static class StatusEffect
{
    public static void AddStatusEffect(StatusEffectType statusEffectType, PlayerStatusEffectHandler handler)
    {
        if (handler.currentEffects.Contains(statusEffectType))
        {
            Debug.LogWarning($"{handler.transform.GetComponent<Mirror.NetworkIdentity>().netId} network id player already has {statusEffectType} effect");
            return;
        }
        
        handler.currentEffects.Add(statusEffectType);
    }
    
    [ExcludeFromDocs]
    public static void AddStatusEffect(StatusEffectType[] statusEffectTypes, PlayerStatusEffectHandler handler)
    {
        foreach (var effect in statusEffectTypes)
        {
            if (handler.currentEffects.Contains(effect))
            {
                Debug.LogWarning($"{handler.transform.GetComponent<NetworkIdentity>().netId} network id player already has {effect} effect");
                return;
            }
        
            handler.currentEffects.Add(effect);
        }
    }

    public static void RemoveStatusEffect(StatusEffectType statusEffectType, PlayerStatusEffectHandler handler)
    {
        if (!handler.currentEffects.Contains(statusEffectType))
        {
            Debug.LogWarning($"{handler.transform.GetComponent<Mirror.NetworkIdentity>().netId} network id player don't have {statusEffectType} effect");
            return;
        }

        handler.currentEffects.Remove(statusEffectType);
    }
    
    [ExcludeFromDocs]
    public static void RemoveStatusEffect(StatusEffectType[] statusEffectTypes, PlayerStatusEffectHandler handler)
    {
        foreach (var effect in statusEffectTypes)
        {
            if (!handler.currentEffects.Contains(effect))
            {
                Debug.LogWarning($"{handler.transform.GetComponent<NetworkIdentity>().netId} network id player don't have {effect} effect");
                return;
            }

            handler.currentEffects.Remove(effect);
        }
    }

    public static void RemoveAllStatusEffects(PlayerStatusEffectHandler handler)
    {
        if (handler.currentEffects.Count <= 0)
        {
            Debug.LogWarning($"{handler.transform.GetComponent<Mirror.NetworkIdentity>().netId} network id player don't have a status effect");
            return;
        }
        
        handler.currentEffects.Clear();
    }

    public static bool ContainStatusEffect(StatusEffectType statusEffectType, PlayerStatusEffectHandler handler)
    {
        return handler.currentEffects.Contains(statusEffectType);
    }
    
    [ExcludeFromDocs]
    public static bool ContainStatusEffect(StatusEffectType[] statusEffectTypes, PlayerStatusEffectHandler handler)
    {
        foreach (var effect in statusEffectTypes)
        {
            if (!handler.currentEffects.Contains(effect))
            {
                return false;
            }
        }
        
        return true;
    }
}
