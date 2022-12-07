using System;
using UnityEngine;
using UnityEngine.AI;

public static class ActionManager
{
    public static bool IsPlayerOnSight(AISight sight)
    {
        if (sight.Objects.Count > 0)
        {
            return true;
        }

        return false;
    }

    public static int HowManyPlayerOnSight(AISight sight)
    {
        if (sight.Objects.Count > 0) throw new Exception("There is no player on sight");

        return sight.Objects.Count;
    }

    public static Transform ClosestPlayerOnSight(AISight sight, Transform ai)
    {
        var minDistance = float.MaxValue;

        var playerTransform = sight.Objects[0];
        
        foreach (var player in sight.Objects)
        {
            var distance = Vector3.Distance(player.position, ai.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                playerTransform = player;
            }
        }

        return playerTransform;
        
    }
    
    public static Transform ClosestPlayerOnPathfinding(AISight sight, Transform ai)
    {
        //Debug.Log($"Distance: {distance} Player: {player.name}");
        Transform closestPlayer = null;
        NavMeshPath path = null;
        var closestTargetDistance = float.MaxValue;
        
        var agent = ai.GetComponent<NavMeshAgent>();

        for (int i = 0; i < sight.Objects.Count; i++)
        {
            path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(ai.position, sight.Objects[i].position, agent.areaMask, path))
            {
                float distance = Vector3.Distance(ai.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestPlayer = sight.Objects[i];
                }
            }
        }

        return closestPlayer;
    }

    public static NavMeshPath CreatePath(Transform target, Transform ai)
    {
        NavMeshPath path = new NavMeshPath();
        
        var agent = ai.GetComponent<NavMeshAgent>();

        NavMesh.CalculatePath(ai.position, target.position, agent.areaMask, path);

        return path;
    }
    
    public static AIBehaviour FindAI()
    {
        var ai = GameObject.FindObjectOfType(typeof(AIBehaviour));

        return (AIBehaviour)ai;
    }
}
