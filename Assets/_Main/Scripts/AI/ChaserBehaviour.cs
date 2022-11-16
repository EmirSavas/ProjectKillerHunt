using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : AIBehaviour
{
    public override PointData PointData { get { return pointData;} set{} }
    protected override Transform Target { get; set; }
    public override NavMeshAgent Agent { get; set; }
    protected override List<Transform> Players { get; set; }

    public PointData pointData;

    private AISight _sight;

    private void Start()
    {
        _sight = GetComponent<AISight>();
    }

    protected override void Movement()
    {
        base.Movement();
        
        //Agent.SetDestination(GoalManager.FindClosestPlayer(this));
        if(_sight.Objects.Count > 0) Agent.SetDestination(_sight.Objects[0].transform.position);
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > 5)
        {
            Movement();   
        }
    }
}
