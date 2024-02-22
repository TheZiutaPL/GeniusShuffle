using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;
    [SerializeField] private bool activeOnStart;
    private bool active;

    private void Awake()
    {
        ToggleLight(activeOnStart);
    }

    public void ToggleLight()
    {
        active = !active;
        lightObject.SetActive(active);
    }

    public void ToggleLight(bool toggle)
    {
        active = toggle;
        lightObject.SetActive(active);
    }
}
