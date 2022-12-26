using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserBehaviour : AIBehaviour
{
    protected override Transform Target { get; set; }
    public override NavMeshAgent Agent { get; set; }
    protected override List<Transform> Players { get; set; }
    protected override AIStats Stats { get; set; }

    public AIStats stats;

    private AISight _sight;

    private AIBrain _brain;

    private bool _canMove;


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
        if (Input.GetKeyDown(KeyCode.B))
        {
            _canMove = !_canMove;
        }

        if (_canMove)
        {
            Movement();
        }
    }
}
