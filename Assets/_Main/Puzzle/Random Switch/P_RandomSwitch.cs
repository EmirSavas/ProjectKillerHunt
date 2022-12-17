using System;
using System.Collections.Generic;
using GenericPuzzleMechanics;
using UnityEngine;
using Random = System.Random;

[Serializable]
public class RandomSwitch
{
    public Transform button;
    public Renderer alarm;
}

public class P_RandomSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private new bool enabled;
    
    public List<RandomSwitch> buttons;
    
    public bool completed;
    
    private int _selectedIndex;
    private RandomSwitch _selectedSwitch;
    private Color _oldColor;

    private List<int> _selectSequence = new List<int>();

    private int _targetIndex;
    private int _trueCount;

    public void Interact(CharacterMechanic cm, CharacterMovement characterMovement)
    {
        //throw new Exception("Not Finished");
        //todo Zoom Player

        enabled = !enabled;
    }

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            _selectSequence.Add(i);
        }

        _selectSequence.Shuffle();

        for (int i = 0; i < _selectSequence.Count; i++)
        {
            //Debug.Log(_selectSequence[i]);
        }

        ChangeButtonSelection(_selectedIndex);
    }

    private void Update()
    {
        if(!enabled) return;

        if (Input.GetKeyDown(KeyCode.L))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, buttons.Count, Mechanics.IncreaseIndex);
            
            ChangeButtonSelection(_selectedIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, buttons.Count, Mechanics.DecreaseIndex);
            
            ChangeButtonSelection(_selectedIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(CheckAllSwitch()) OpenSwitch(_selectedSwitch);
            else CloseAllSwitch();
            
            CheckForCompletion();
        }
    }

    private void CheckForCompletion()
    {
        _trueCount = 0;
        
        foreach (var button in buttons)
        {
            if (button.alarm.material.color == Color.green)
            {
                _trueCount++;

                //Debug.Log($"{trueCount} {buttons.Count}");
                
                if (_trueCount == buttons.Count)
                {
                    Completed();
                    break;
                }
            }
        }
    }
    
    private void ChangeButtonSelection(int index)
    {
        if (_selectedSwitch != null)
        {
            var oldRenderer = _selectedSwitch.button.GetComponent<Renderer>();
            oldRenderer.material.color = _oldColor;
        }
        
        _selectedSwitch = buttons[index];
        var renderer = _selectedSwitch.button.GetComponent<Renderer>();
        _oldColor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }

    private bool CheckAllSwitch()
    {
        Debug.Log($"{buttons[_selectSequence[_targetIndex]].button.name} {_selectedSwitch.button.name}");
        
        if (buttons[_selectSequence[_targetIndex]] == _selectedSwitch)
        {
            return true;
        }
        
        return false;
    }

    private void OpenSwitch(RandomSwitch randomSwitch)
    {
        randomSwitch.alarm.material.color = Color.green;

        _targetIndex++;
    }

    private void CloseAllSwitch()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].alarm.material.color = Color.red;
        }

        _targetIndex = 0;
    }
    
    private void Completed()
    {
        completed = true;
    }
}

public static class Randomizer
{
    private static Random rng = new Random();  

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
