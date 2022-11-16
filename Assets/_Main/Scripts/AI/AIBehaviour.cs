using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIBehaviour : MonoBehaviour
{
    public abstract PointData PointData { get; set; }
    protected abstract Transform Target { get; set; }
    public abstract NavMeshAgent Agent { get; set; }
    protected abstract List<Transform> Players { get; set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

        Players = new List<Transform>();
        
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            Players.Add(players[i].transform);
        }
    }

    protected virtual void Movement()
    {
        
    }
}
