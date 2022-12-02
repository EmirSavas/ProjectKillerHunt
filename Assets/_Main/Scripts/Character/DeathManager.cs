using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DeathManager : MonoBehaviour
{
    public static DeathManager instance;
    public List<CinemachineVirtualCamera> cam;
    public CinemachineVirtualCamera playerCam;

    private void Awake()
    {
        instance = this;
    }

    public void FindPlayerCam()
    {
        foreach (var var in cam)
        {
            if (var.gameObject.activeInHierarchy)
            {
                playerCam = var;
            }
        }
    }
}
