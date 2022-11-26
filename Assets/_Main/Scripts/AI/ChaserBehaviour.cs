using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : AIBehaviour
{
    public override PointData PointData { get { return pointData;} set{} }
    protected override Transform Target { get; set; }
    public override NavMeshAgent Agent { get; set; }
    protected override List<Transform> Players { get; set; }

    protected override AIStats Stats { get; set; }

    public AIStats stats;

    public PointData pointData;

    private AISight _sight;

    private AIBrain _brain;


    private void Start()
    {
        Stats = stats;
        _sight = GetComponent<AISight>();
        _brain = GetComponent<AIBrain>();
        
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void Movement()
    {
        base.Movement();
        
        Agent.SetPath(_brain.Path);
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > 15)
        {
            Movement();
        }
    }
}
