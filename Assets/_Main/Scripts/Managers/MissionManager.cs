using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private static MissionManager instance;
    public static MissionManager Instance { get { return instance; } }
    
    public List<string> missions;

    public TextMeshProUGUI missionText;

    private int _missionIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        missionText.text = MissionFormatter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(SetNextMission());
        }
    }

    public IEnumerator SetNextMission()
    {
        _missionIndex++;

        missionText.text = MissionFormatter();
        
        // yield return DOTween.ToAlpha(()=> missionText.color, x=> missionText.color = x, 255, 5).WaitForCompletion();
        //
        // yield return new WaitForSeconds(2f);
        //
        // DOTween.ToAlpha(()=> missionText.color, x=> missionText.color = x, 0, 5);

        yield return null;
    }

    private string MissionFormatter()
    {
        var text = "-" + missions[_missionIndex];

        return text;
    }
}
