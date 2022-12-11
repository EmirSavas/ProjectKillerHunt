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


    public List<Transform> circles;
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
    public void Interact(CharacterMechanic cm)
    {
        //throw new Exception("Not Finished");
        //todo Zoom Player

        enabled = !enabled;
    }
    private void Update()
    {
        if(!enabled) return;
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, circles.Count, Mechanics.IncreaseIndex);
            
            ChangeCircleSelection(_selectedIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedIndex = Mechanics.ChangeIndex(_selectedIndex, 0, circles.Count, Mechanics.DecreaseIndex);
            
            ChangeCircleSelection(_selectedIndex);
        }
        
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            circles[_selectedIndex].Rotate(new Vector3(0, Time.deltaTime * -rotateSpeed), Space.Self);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            circles[_selectedIndex].Rotate(new Vector3(0, Time.deltaTime * rotateSpeed), Space.Self);
        }

        for (int i = 0; i < circles.Count; i++)
        {
            if (circles[i].eulerAngles.x > minSuccessAngle && circles[i].eulerAngles.x < maxSuccessAngle)
            {
                var renderer = circles[i].GetChild(0).GetComponent<Renderer>();
            
                renderer.material.color = Color.green;

                //circles[i].localEulerAngles = Vector3.Lerp(circles[i].eulerAngles, new Vector3(0, 90, 0), 0.5f);
                circles[i].Rotate(new Vector3(0, 0), Space.Self);
            }
            else
            {
                var renderer = circles[i].GetChild(0).GetComponent<Renderer>();
            
                renderer.material.color = Color.red;
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
}
