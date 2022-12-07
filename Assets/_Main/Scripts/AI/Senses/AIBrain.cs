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
        var closestPlayer = GoalManager.FindClosestPlayer(_sight, transform);

        if (closestPlayer == null)
        {
            Debug.Log("Closest Player Is Null");
            targetPlayer = _sight.lastSeenTransform;
            return;
        }

        targetPlayer = closestPlayer;
    }

    public void SearchForAllPLayers()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        var random = Random.Range(0, players.Length);

        targetPlayer = players[random].transform;
    }

    private void Update()
    {
        CurrentState.Process();

        if (Vector3.Distance(transform.position, _sight.lastSeenTransform.position) < 3 && targetPlayer == _sight.lastSeenTransform)
        {
            Debug.Log("Couldn't Find Player");
            State.ChangeState(this, State.STATE.SEARCH, _animator);
        }
    }
}
