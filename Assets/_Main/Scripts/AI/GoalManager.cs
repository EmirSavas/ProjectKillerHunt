using System;
using UnityEngine;
using UnityEngine.AI;

public static class GoalManager
{
    public static Transform FindClosestPlayer(AISight sight, Transform ai)
    {
        if (ActionManager.IsPlayerOnSight(sight))
        {
            //return ActionManager.ClosestPlayerOnSight(sight, ai);,
            return ActionManager.ClosestPlayerOnPathfinding(sight, ai);
        }

        return null;

        throw new Exception("There is no player in sight");
    }
    
    public static NavMeshPath ChasePlayer(Transform target, Transform ai)
    {
        return ActionManager.CreatePath(target, ai);
    }
}
