using System;
using StateMachine;
using UnityEngine;

public class Chase : State
{
    public Chase(AIBrain _brain, Animator _animator) : base(_brain, _animator)
    {
        state = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Chase Enter");
        stage = EVENT.UPDATE;

        //todo Chase Animation Play
    }

    public override void Update()
    {
        Debug.Log("Target Player: " + brain.targetPlayer);
        brain.Path = GoalManager.ChasePlayer(brain.targetPlayer, brain.transform);
        
        brain.initialCooldown -= Time.deltaTime;
        
        if (brain.initialCooldown < 0)
        {
            brain.SelectNewPath();
            brain.initialCooldown = brain.selectNewTargetCooldown;
        }
    }

    public override void Exit()
    {
        Debug.Log("Chase Exit");
        //todo Chase Animation Stop
        
        if(nextState == null) throw new NullReferenceException("Next State Is Null");
    }
}
