using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("AI/AI Hear")]
public class AIHear : MonoBehaviour
{
    [Header("SETTINGS")]
    [Space(5)]
    [SerializeField] private bool drawMesh = true;
    [SerializeField] private Color meshColor;
    [SerializeField] private float radius;

    private void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        if(!drawMesh) return;
        
        Gizmos.color = meshColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
