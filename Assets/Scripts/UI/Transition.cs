using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CanvasGroup))]
public class Transition : MonoBehaviour
{
    private static Transition instance;
    private CanvasGroup canvasGroup;
    [SerializeField] private bool transitionOutStart = true;
    [SerializeField] private float transitionTime = .35f;
    [SerializeField] private float endWaitTime = 0.07f;
    private Action callbackAction;

    private Coroutine transitionCoroutine;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = transitionOutStart ? 1 : 0;
    }

    private void Start()
    {
        SetTransition(false, null);
    }

    public static void SetTransition(bool toggle, Action callbackAction)
    {
        if (instance.transitionCoroutine != null)
            return;

        instance.callbackAction = callbackAction;
        instance.transitionCoroutine = instance.StartCoroutine(instance.StartTransition(toggle));
    }

    IEnumerator StartTransition(bool toggle)
    {
        float timer = 0;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;

            float blend = timer / transitionTime;
            if (!toggle) blend = 1 - blend;

            canvasGroup.alpha = blend;

            yield return null;
        }

        yield return new WaitForSeconds(endWaitTime);

        callbackAction?.Invoke();

        transitionCoroutine = null;
    }
}
