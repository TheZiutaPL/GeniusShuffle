using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private GameObject lightObject;
    [SerializeField] private bool activeOnStart;
    private bool active;

    [Header("Switch")]
    [SerializeField] private Transform switchTransform;
    [SerializeField] private float tiltXDegees = 15;
    private Vector3 switchStartLocalRotation;

    private void Awake()
    {
        switchStartLocalRotation = switchTransform.localEulerAngles;

        ToggleLight(activeOnStart);
    }

    public void ToggleLight() => ToggleLight(!active);

    public void ToggleLight(bool toggle)
    {
        active = toggle;
        lightObject.SetActive(active);

        UpdateSwitch();
    }

    private void UpdateSwitch() => switchTransform.localEulerAngles = new Vector3(switchStartLocalRotation.x + (active ? tiltXDegees : -tiltXDegees), switchStartLocalRotation.y, switchStartLocalRotation.z);
}
