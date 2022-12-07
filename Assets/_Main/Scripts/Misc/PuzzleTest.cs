using UnityEngine;

public class PuzzleTest : MonoBehaviour
{
    public Renderer exampleCube;
    
    private Puzzle _puzzle;
    
    private void Awake()
    {
        _puzzle = GetComponent<Puzzle>();
    }

    void Start()
    {
        var position = exampleCube.bounds.max;
        
        _puzzle.CallQuickTimeEvent(KeyCode.E, 1, 1, position);
        //_puzzle.CallPulse(KeyCode.E, 3);
        //_puzzle.CallHold(KeyCode.E, 3f);
    }
    
}
