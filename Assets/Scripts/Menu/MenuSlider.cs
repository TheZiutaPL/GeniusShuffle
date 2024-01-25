using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Allows selection of values between minValue and maxValue in the form of a slider

public class MenuSlider : InteractionHandler
{
    [SerializeField] float step = 0.1f;
    public float minValue = 0f;
    public float maxValue = 1f;
    [SerializeField] private float displayedValueMultiplier = 100;

    [SerializeField] TMP_Text displayText;

    [Space(10)]
    public UnityEvent<float> onValueChanged;
    public float value = 0f;

    enum SliderModes
    {
        ScaleBar,
        MovePoint
    }
    [Header("Slider bar settings")]
    [SerializeField] SliderModes mode = SliderModes.MovePoint;
    [SerializeField] bool snapToStep = true;
    [SerializeField] Transform sliderValueTransform; // The part that's scaled/moved based on the value
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint; // The position where the slider's value should be the highest - used to calculate slider's direction

    public override void ClickAction(Vector3 hitPoint)
    {
        Vector3 sliderDirection = endPoint.position - startPoint.position;
        Vector3 hitPointDirection = hitPoint - startPoint.position;

        SetValue(Vector3.Dot(hitPointDirection, sliderDirection.normalized) / sliderDirection.magnitude);

        base.ClickAction(hitPoint);
    }

    public void Raise() => SetValue(value + step);
    public void Lower() => SetValue(value - step);

    public void SetValue(float newValue)
    {
        value = Mathf.Round(Mathf.Clamp(newValue, minValue, maxValue) * 100f) / 100f;
        if (snapToStep)
            value = Mathf.Round(value / step) * step;

        onValueChanged?.Invoke(value);

        if (displayText != null)
            displayText.text = $"{value * displayedValueMultiplier}";

        if (sliderValueTransform != null)
        {
            if (mode == SliderModes.ScaleBar)
                sliderValueTransform.localScale = new Vector3(value / maxValue, sliderValueTransform.localScale.y, sliderValueTransform.localScale.z);
            else
                sliderValueTransform.position = Vector3.Lerp(startPoint.position, endPoint.position, value / maxValue);
        }
    }
}
