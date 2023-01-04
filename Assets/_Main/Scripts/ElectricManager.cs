using System.Collections.Generic;
using GenericPuzzleMechanics;
using UnityEngine;

[System.Serializable]
public class Lever
{
    public GameObject leverObject;
    public LeverState state;
}

public enum LeverState
{
    OPEN,
    CLOSE
}

public class ElectricManager : MonoBehaviour, IInteractable
{
    [SerializeField] private new bool enabled;
    public List<Lever> levers;

    public int powerCount;

    private List<Lever> _openedLevers = new List<Lever>();

    private int _selectedIndex;
    private Lever _selectedLever;
    private Color _oldColor;

    private void Start()
    {
        foreach (var lever in levers)
        {
            lever.state = LeverState.CLOSE;
        }
    }

    public void Interact(CharacterMechanic characterMechanic, CharacterMovement characterMovement)
    {
        enabled = !enabled;
    }

    private void Update()
    {
        if(!enabled) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, levers.Count, Mechanics.IncreaseIndex);
            
            ChangeLeverSelection(_selectedIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, levers.Count, Mechanics.DecreaseIndex);
            
            ChangeLeverSelection(_selectedIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenSwitch(_selectedLever);
        }
    }
    
    private void ChangeLeverSelection(int index)
    {
        if (_selectedLever != null)
        {
            var oldRenderer = _selectedLever.leverObject.GetComponent<Renderer>();
            oldRenderer.material.color = _oldColor;
        }
        
        _selectedLever = levers[index];
        var renderer = _selectedLever.leverObject.GetComponent<Renderer>();
        _oldColor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }
    
    private void OpenSwitch(Lever lever)
    {
        switch (lever.state)
        {
            case LeverState.OPEN:
                lever.state = LeverState.CLOSE;
                break;
            
            case LeverState.CLOSE:
                lever.state = LeverState.OPEN;
                break;
        }

        if (_openedLevers.Count <= powerCount)
        {
            //REMOVE LAST INDEX AND ADD FIRST INDEX
            if(_openedLevers.Count > 0) _openedLevers.RemoveAt(_openedLevers.Count - 1);
            _openedLevers.Add(lever);
        }
    }
}
