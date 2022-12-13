using System;
using System.Collections.Generic;
using UnityEngine;

public class ParentPuzzle : MonoBehaviour
{
    public Renderer alarm;
    public Renderer alarm2;
    public Renderer fire;

    public P_RandomSwitch randomSwitch;
    public P_RotatingCircles rotatingCircles;
    public P_CarryObject carryObject;

    private bool _doOnce;
    

    private void Update()
    {
        if (randomSwitch.completed)
        {
            alarm.material.color = Color.green;
        }

        if (rotatingCircles.completed)
        {
            alarm2.material.color = Color.green;
        }

        if (carryObject.completed)
        {
            fire.material.SetColor("_EmissionColor", Color.green);
        }

        if (randomSwitch.completed && rotatingCircles.completed && carryObject.completed && !_doOnce)
        {
            StartCoroutine(MissionManager.Instance.SetNextMission());
            _doOnce = true;
        }
    }
}
