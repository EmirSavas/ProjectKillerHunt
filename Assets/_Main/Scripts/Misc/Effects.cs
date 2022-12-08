using DG.Tweening;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private static Effects instance;
    public static Effects Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }

    public void FadeOut(Color obj)
    {
        DOTween.ToAlpha(()=> obj, x=> obj = x, 0, 5);
    }
}
