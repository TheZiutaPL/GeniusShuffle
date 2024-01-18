using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MenuSlider : InteractionHandler
{
    [SerializeField] float step = 0.1f;
    public float minValue = 0f;
    public float maxValue = 1f;

    [SerializeField] TMP_Text displayText;

    public UnityEvent<float> onValueChanged;
    public float value = 0f;

    public void Raise() => SetValue(value + step);
    public void Lower() => SetValue(value - step);
    public void SetValue(float newValue)
    {
        value = Mathf.Round(Mathf.Clamp(newValue, minValue, maxValue) * 100f) / 100f;

        onValueChanged?.Invoke(value);

        if (displayText != null)
            displayText.text = value.ToString(); // TODO: Some animations
    }
}
