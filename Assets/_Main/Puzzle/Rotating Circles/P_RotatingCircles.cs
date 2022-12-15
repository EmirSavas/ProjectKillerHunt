using System;
using System.Collections.Generic;
using GenericPuzzleMechanics;
using UnityEngine;
using Random = UnityEngine.Random;

public class P_RotatingCircles : MonoBehaviour, IInteractable
{
    [SerializeField] private new bool enabled;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float minSuccessAngle;
    [SerializeField] private float maxSuccessAngle;
    
    private int _trueCount;


    public List<Transform> circles;
    public bool completed;
    
    private Color _oldColor; //For Now
    private int _selectedIndex;
    private Transform _selectedCircle;

    private void Start()
    {
        ChangeCircleSelection(_selectedIndex);

        foreach (var circle in circles)
        {
            circle.Rotate(new Vector3(0, Random.Range(0, 360)));
        }
    }
    public void Interact(CharacterMechanic cm, CharacterMovement characterMovement)
    {
        //throw new Exception("Not Finished");
        //todo Zoom Player

        enabled = !enabled;
    }
    private void Update()
    {
        if(!enabled) return;
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, circles.Count, Mechanics.IncreaseIndex);
            
            ChangeCircleSelection(_selectedIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, circles.Count, Mechanics.DecreaseIndex);
            
            ChangeCircleSelection(_selectedIndex);
        }
        
        if (Input.GetKey(KeyCode.J))
        {
            circles[_selectedIndex].Rotate(new Vector3(0, Time.deltaTime * -rotateSpeed), Space.Self);
        }
        else if (Input.GetKey(KeyCode.L))
        {
            circles[_selectedIndex].Rotate(new Vector3(0, Time.deltaTime * rotateSpeed), Space.Self);
        }

        _trueCount = 0;

        for (int i = 0; i < circles.Count; i++)
        {
            var renderer = circles[i].GetChild(0).GetComponent<Renderer>();
            
            if (circles[i].eulerAngles.x > minSuccessAngle && circles[i].eulerAngles.x < maxSuccessAngle)
            {
                renderer.material.color = Color.green;
                
                circles[i].Rotate(new Vector3(0, 0), Space.Self);
            }
            else
            {
                renderer.material.color = Color.red;
            }

            if (renderer.material.color == Color.green)
            {
                _trueCount++;

                if (_trueCount == circles.Count)
                {
                    Completed();
                }
            }
        }
    }
    private void ChangeCircleSelection(int index)
    {
        if (_selectedCircle != null)
        {
            var oldRenderer = _selectedCircle.GetComponent<Renderer>();
            oldRenderer.material.color = _oldColor;
        }
        _selectedCircle = circles[index];
        var renderer = _selectedCircle.GetComponent<Renderer>();
        _oldColor = renderer.material.color;
        renderer.material.color = Color.yellow;
    }
    
    private void Completed()
    {
        completed = true;
    }
}
