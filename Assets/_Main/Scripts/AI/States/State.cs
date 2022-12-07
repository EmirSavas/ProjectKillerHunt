using UnityEngine;

namespace StateMachine
{
    public abstract class State
    {
        public enum STATE
        {
            IDLE,
            SEARCH,
            CHASE,
            ACTION
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
        protected EVENT stage;
        protected static State nextState;
        protected Animator anim;
        protected AIBrain brain;

        private float _timer = 1f;


        public State(AIBrain _brain, Animator _anim)
        {
            brain = _brain;
            anim = _anim;
            stage = EVENT.ENTER;
        }

        public abstract void Enter();

        public virtual void PerSecond()
        {
            stage = EVENT.PERSECOND;
        }

        public virtual void Update()
        {
            stage = EVENT.UPDATE;
        }

        public abstract void Exit();

        public State Process()
        {
            if (stage == EVENT.ENTER)
            {
                Enter();
            }

            if (stage == EVENT.UPDATE)
            {
                Update();
            }

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

        public static void ChangeState(AIBrain brain, STATE state, Animator animator)
        {
            switch (state)
            {
                case STATE.IDLE:
                    nextState = new Idle(brain, animator);
                    break;

                case STATE.SEARCH:
                    nextState = new Search(brain, animator);
                    break;

                case STATE.CHASE:
                    nextState = new Chase(brain, animator);
                    break;

                case STATE.ACTION:
                    nextState = new EnemyAction(brain, animator);
                    break;
            }

            brain.CurrentState = nextState;
        }
    }
}
