using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Allows selection of values between minValue and maxValue in the form of a slider

public class MenuSlider : MenuElement
{
    [SerializeField] float step = 0.1f;
    [SerializeField] float minValue = 0f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] private float displayedValueMultiplier = 100;

    [SerializeField] TMP_Text displayText;
    [SerializeField] GameObject lowerButton;
    [SerializeField] GameObject raiseButton;


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

    private void Start()
    {
        RefreshVisuals();
    }

    public override void HeldDownAction(Vector3 hitPoint)
    {
        ClickAction(hitPoint);
        base.HeldDownAction(hitPoint);
    }

    public override void ClickAction(Vector3 hitPoint)
    {
        Vector3 sliderDirection = endPoint.position - startPoint.position;
        Vector3 hitPointDirection = hitPoint - startPoint.position;

        SetValue(Vector3.Dot(hitPointDirection, sliderDirection.normalized) / sliderDirection.magnitude);

        base.ClickAction(hitPoint);
    }

    public void Raise() => SetValue(value + step);
    public void Lower() => SetValue(value - step);

    public void SetValue(float newValue, bool playSound)
    {
        newValue = Mathf.Round(newValue * 100f) / 100f;
        if (snapToStep)
            newValue = Mathf.Round(newValue / step) * step;
        newValue = Mathf.Clamp(newValue, minValue, maxValue);

        if (value == newValue)
            return;

        if (playSound)
            PlayClickSound();

        value = newValue;

        onValueChanged?.Invoke(value);

        RefreshVisuals();
    }

    void RefreshVisuals()
    {
        if (displayText != null)
            displayText.text = $"{Mathf.Round(value * 100f) / 100f * displayedValueMultiplier}";
        // This has to be rounded again so min or max value don't affect the display

        if (lowerButton != null)
        {
            if (value <= minValue)
                lowerButton.SetActive(false);
            else
                lowerButton.SetActive(true);
        }

        if (raiseButton != null)
        {
            if (value >= maxValue)
                raiseButton.SetActive(false);
            else
                raiseButton.SetActive(true);
        }

        if (sliderValueTransform != null)
        {
            float sliderValue = maxValue > 0 ? value / maxValue : 1f;
            if (mode == SliderModes.ScaleBar)
                sliderValueTransform.localScale = new Vector3(sliderValue, sliderValueTransform.localScale.y, sliderValueTransform.localScale.z);
            else
                sliderValueTransform.position = Vector3.Lerp(startPoint.position, endPoint.position, sliderValue);
        }
    }

    // For use in UnityEvents set through the inspector
    public void SetValue(float newValue) => SetValue(newValue, true);

    public void SetMaxValue(float newMaxValue)
    {
        maxValue = newMaxValue;
        SetValue(Mathf.Clamp(value, minValue, maxValue), false);
    }

    public void SetMinValue(float newMinValue)
    {
        minValue = newMinValue;
        SetValue(Mathf.Clamp(value, minValue, maxValue), false);
    }
}
