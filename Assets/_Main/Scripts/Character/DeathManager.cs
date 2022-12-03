using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;
    public List<CinemachineVirtualCamera> cam;
    //private CinemachineVirtualCamera playerCam;
    private int _oldCamSelect = 0;
    private int _newCamSelect = 0;

    private void Awake()
    {
        instance = this;
    }

    // public void FindPlayerCam()
    // {
    //     foreach (var var in cam)
    //     {
    //         if (var.gameObject.activeInHierarchy)
    //         {
    //             playerCam = var;
    //             cam.Remove(playerCam);
    //         }
    //     }
    // }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _oldCamSelect = _newCamSelect;
            
            _newCamSelect += 1;
            
            if (_newCamSelect > cam.Capacity - 1)
            {
                _newCamSelect = 0;
            }
            
            cam[_newCamSelect].gameObject.SetActive(true);
            
            cam[_oldCamSelect].gameObject.SetActive(false);
            
            //playerCam.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _oldCamSelect = _newCamSelect;
            
            _newCamSelect -= 1;
            
            if (_newCamSelect < 0)
            {
                _newCamSelect = cam.Capacity -1;
            }
            
            cam[_newCamSelect].gameObject.SetActive(true);
            
            cam[_oldCamSelect].gameObject.SetActive(false);
            
           //playerCam.gameObject.SetActive(false);
        }
    }
}
