using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectTween : MonoBehaviour
{
    private RectTransform parentRect;
    [SerializeField] private RectTransform tweenedRect;
    [SerializeField] private bool tweenPosition;
    private Vector3 startPosition;
    private Vector2 startSize;
    [SerializeField, Range(0f, 1f)] private float tweenValue;
    private bool tweening;

    public void EnableTweening() 
    { 
        tweenedRect.sizeDelta = startSize; 

        if(tweenPosition)
            tweenedRect.position = startPosition; 

        enabled = true;
    }
    public void DisableTweening() => enabled = false;

    private void Awake()
    {
        parentRect = (RectTransform)transform;

        startPosition = tweenedRect.position;
        startSize = tweenedRect.sizeDelta;

        enabled = false;
    }

    private void Update()
    {
        tweenedRect.sizeDelta = Vector3.Lerp(startSize, parentRect.sizeDelta, tweenValue);
        
        if(tweenPosition)
            tweenedRect.position = Vector3.Lerp(startPosition, parentRect.position, tweenValue);
    }
}
