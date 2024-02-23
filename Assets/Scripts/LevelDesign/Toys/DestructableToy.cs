using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructableToy : MonoBehaviour
{
    [Header("Destruction")]
    [SerializeField] private bool destructedOnStart;
    [SerializeField] private int destructableHealth = 3;
    private int currentHealth;

    [Header("Repair")]
    [SerializeField] private int repairsToProgress = 1;
    private int repairProgress;

    [Space(10)]

    [SerializeField] private UnityEvent onHitEvent;
    [SerializeField] private UnityEvent onDestructionEvent;
    [SerializeField] private UnityEvent onRepairingProgressEvent;
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
        if (currentHealth > 0)
            return;

        repairProgress++;
        if (repairProgress < repairsToProgress)
        {
            onRepairingProgressEvent?.Invoke();
            return;
        }

        currentHealth = destructableHealth;
        repairProgress = 0;
        onRepairEvent?.Invoke();
    }
}
