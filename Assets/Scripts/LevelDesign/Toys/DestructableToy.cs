using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructableToy : MonoBehaviour
{
    [SerializeField] private bool destructedOnStart;
    [SerializeField] private int destructableHealth = 3;
    private int currentHealth;

    [Space(10)]

    [SerializeField] private UnityEvent onHitEvent;
    [SerializeField] private UnityEvent onDestructionEvent;
    [SerializeField] private UnityEvent onRepairEvent;

    private void Awake()
    {
        if (!destructedOnStart) currentHealth = destructableHealth;
    }

    public void DamageToy()
    {
        if (currentHealth <= 0)
            return;

        currentHealth--;

        if (currentHealth > 0) onHitEvent?.Invoke();
        else onDestructionEvent?.Invoke();
    }

    public void RepairToy()
    {
        currentHealth = destructableHealth;
        onRepairEvent?.Invoke();
    }
}
