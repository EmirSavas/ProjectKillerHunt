using System;
using StateMachine;
using UnityEngine;

public class Action : State
{
    public Action(AIBrain _brain, Animator _animator) : base(_brain, _animator)
    {
        state = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Action Enter");
        stage = EVENT.NULL;

        //todo Action Animation Play
    }

    public override void Exit()
    {
        Debug.Log("Action Exit");
        //todo Action Animation Stop
        
        if(nextState == null) throw new NullReferenceException("Next State Is Null");
    }
}
