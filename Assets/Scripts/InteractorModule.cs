using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorModule<T>
{
    public bool ValidateObject(GameObject gameObject, out T component)
    {
        component = gameObject.GetComponent<T>();
        return component != null;
    }
}
