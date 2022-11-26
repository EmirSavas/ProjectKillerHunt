using StateMachine;
using UnityEngine;
using UnityEngine.AI;

[AddComponentMenu("AI/AI Brain")]
public class AIBrain : MonoBehaviour
{
    [Header("SETTINGS")]
    [Space(5)]
    public float selectNewTargetCooldown;
    
    public NavMeshPath Path;
    public State CurrentState;

    [HideInInspector] public float initialCooldown;
    [HideInInspector] public Transform targetPlayer;
    private AISight _sight;
    private Animator _animator;
    
    
    private void Start()
    {
        _sight = GetComponent<AISight>();
        _animator = GetComponentInChildren<Animator>();

        State.ChangeState(this, State.STATE.IDLE, _animator);

        initialCooldown = selectNewTargetCooldown;
        
        Invoke(nameof(Decide), 0.1f);
    }

    public void Decide()
    {
        Path = new NavMeshPath();
        
        SelectNewPath();
        State.ChangeState(this, State.STATE.CHASE, _animator);
    }

    public void SelectNewPath()
    {
        var closestPlayer = GoalManager.FindClosestPlayer(_sight, transform); //No Player In Sight

        if (closestPlayer == null)
        {
            Debug.Log("Closest Player Is Null");
            targetPlayer = _sight.lastSeenTransform;
            return;
        }

        targetPlayer = closestPlayer;
    }

    private void Update()
    {
        CurrentState.Process();
    }
}
