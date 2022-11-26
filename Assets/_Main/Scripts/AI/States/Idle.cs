using System;
using StateMachine;
using UnityEngine;

public class Idle : State
{
    public Idle(AIBrain _brain, Animator _animator) : base(_brain, _animator)
    {
        state = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Idle Enter");
        stage = EVENT.NULL;

        //todo Idle Animation Play
    }

    public override void Exit()
    {
        Debug.Log("Idle Exit");
        //todo Idle Animation Stop
        
        if(nextState == null) throw new NullReferenceException("Next State Is Null");
    }
}
