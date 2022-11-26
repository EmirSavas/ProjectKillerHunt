using System;
using StateMachine;
using UnityEngine;

public class Search : State
{
    public Search(AIBrain _brain, Animator _animator) : base(_brain, _animator)
    {
        state = STATE.IDLE;
    }

    public override void Enter()
    {
        Debug.Log("Search Enter");
        stage = EVENT.NULL;

        //todo Search Animation Play
    }

    public override void Exit()
    {
        Debug.Log("Search Exit");
        //todo Search Animation Stop
        
        if(nextState == null) throw new NullReferenceException("Next State Is Null");
    }
}
