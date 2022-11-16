using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goal
{
    public int TotalPoints;
    public readonly List<Action> Actions;

    private readonly int _totalPoints;

    public Goal(List<Action> actions)
    {
        Actions = actions;

        foreach (var action in Actions)
        {
            _totalPoints += action.Point;

        }
        
        TotalPoints = _totalPoints;
    }
}

public static class GoalManager
{
    public static Vector3 FindClosestPlayer(AIBehaviour ai = null)
    {
        if (ai == null)
        {
            ai = FindAI();
        }
        
        var players = GameObject.FindGameObjectsWithTag("Player");
        var minDistance = float.MaxValue;
        var position = Vector3.zero;
        var speed = ai.Agent.speed;

        foreach (var player in players)
        {
            var path = new NavMeshPath();

             ai.Agent.CalculatePath(player.transform.position, path);
             
             var distance = ai.Agent.remainingDistance;

            
            // ai.Agent.speed = 0;
            // ai.Agent.SetDestination(player.transform.position);
            // var distance = CalculateDistance(ai.Agent.path.corners);
            
            if (minDistance > distance)
            {
                minDistance = distance;
                position = player.transform.position;
            }
        }


        //ai.Agent.speed = speed;
        
        return position;
    }

    private static float CalculateDistance(Vector3[] corners)
    {
        float distance = 0;
        
        for (int i = 0; i < corners.Length; i++)
        {
            if (i == corners.Length - 1)
            {
                continue;
            }

            distance += Mathf.Abs(Vector3.Distance(corners[i], corners[i + 1]));
        }

        return distance;
    }

    private static AIBehaviour FindAI()
    {
        var ai = GameObject.FindObjectOfType(typeof(AIBehaviour));

        return (AIBehaviour)ai;
    }
}
