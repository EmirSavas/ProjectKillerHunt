using UnityEngine;

public abstract class State
{

    public enum STATE
    {
        IDLE,
        WAIT,
        ACTION,
        HIT,
        DEAD
    }

    public enum EVENT
    {
        NULL,
        ENTER,
        UPDATE,
        PERSECOND,
        EXIT
    }

    public STATE state;
    protected EVENT stage; //todo change name
    protected static State nextState;
    protected Animator anim;

    private float _timer = 1f;
    

    public State(Animator _anim)
    {
        anim = _anim;
        stage = EVENT.ENTER;
    }

    public abstract void Enter();
    public virtual void PerSecond() { stage = EVENT.PERSECOND; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public abstract void Exit();

    public State Process()
    {
        if(stage == EVENT.ENTER) { Enter(); }
        if(stage == EVENT.UPDATE) { Update(); }
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        if (stage == EVENT.PERSECOND)
        {
            _timer += Time.deltaTime;
            
            if (_timer >= 1f)
            {
                PerSecond();
                _timer = 0f;
            }
        }

        return this;
    }

    /*public static void ChangeState(STATE state)
    {
        switch (state)
        {
            case STATE.IDLE:
                nextState = new Idle(entity.GetComponent<Animator>());
                break;
            
            case STATE.WAIT:
                nextState = new Wait(entity.GetComponent<Animator>());
                break;
            
            case STATE.ACTION:
                nextState = new Action(entity.GetComponent<Animator>());
                break;
            
            case STATE.HIT:
                nextState = new Hit(entity.GetComponent<Animator>());
                break;
            
            case STATE.DEAD:
                nextState = new Dead(entity.GetComponent<Animator>());
                break;
        }

        entity.currentState = nextState;
    }*/
}
