using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Hourglass : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private MeshRenderer topSandRenderer;
    private const string TOP_SAND_FILL_KEY = "_Fill_Level";
    [SerializeField] private SkinnedMeshRenderer bottomSandRenderer;
    [SerializeField] private Animator hourglassAnimator;
    [SerializeField] private string startTimerAnim;

    private float timeToPass;
    private float timePassed;

    private bool isTimerOn;
    private bool updateVisuals;

    private Action callbackAction;

    public void StartTimer(float timeToPass, Action callbackAction)
    {
        if (hourglassAnimator != null)
            hourglassAnimator.Play(startTimerAnim);

        this.timeToPass = timeToPass;
        this.callbackAction = callbackAction;

        timePassed = 0;

        isTimerOn = true;
    }

    public void StopTimer()
    {
        isTimerOn = false;

        UpdateVisuals();
    }

    public void StartVisualUpdate() => updateVisuals = true;
    public void StopVisualUpdate() => updateVisuals = false;

    private void Update()
    {
        if (!isTimerOn)
            return;

        timePassed += Time.deltaTime;

        if (updateVisuals) UpdateVisuals();

        if(timePassed >= timeToPass)
        {
            isTimerOn = false;
            callbackAction?.Invoke();
        }
    }

    private void UpdateVisuals()
    {
        float progress = 1 - Mathf.Clamp01(timePassed / timeToPass);

        topSandRenderer.material.SetFloat(TOP_SAND_FILL_KEY, progress);
        bottomSandRenderer.SetBlendShapeWeight(0, progress * 100);
    }
}
