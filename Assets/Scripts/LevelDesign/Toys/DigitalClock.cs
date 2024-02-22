using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private TextMeshPro clockText;
    [SerializeField] private float refreshRate = 10;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateClock), 0, refreshRate);
    }

    private void UpdateClock()
    {
        DateTime currentTime = DateTime.Now;
        clockText.SetText($"{currentTime.Hour}:{currentTime.Minute}");
    }
}
