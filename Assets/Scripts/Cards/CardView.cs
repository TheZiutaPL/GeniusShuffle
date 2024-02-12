using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class CardView : MonoBehaviour
{
    private static CardView instance;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private CardObject cardObject;
    [SerializeField] private float animationTime;
    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField] private Transform cardViewReferenceTransform;
    [SerializeField] private CardViewUI cardViewUI;

    private static bool isOn;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 startScale;

    private InputActionMap gameMap;

    private CardObject gameCardObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        gameMap = playerInput.actions.FindActionMap("Game");
    }

    public static void ShowCardView(CardObject cardObject, Action callbackAction = null)
    {
        instance.cardViewUI.SetCardViewUI(cardObject.cardData);

        instance.cardObject.SetCardData(cardObject.cardData, cardObject.innerBackgroundColor, cardObject.outerBackgroundColor);
        instance.StartCoroutine(instance.CardViewOnAnimation(cardObject.GetRealCardTransform(), callbackAction));

        instance.gameCardObject = cardObject;
        instance.gameCardObject.EnableRenderer(false);
    }

    private IEnumerator CardViewOnAnimation(Transform cardTransform, Action callbackAction)
    {
        gameMap.Disable();

        cardViewUI.ShowUI(true);

        isOn = true;

        cardObject.gameObject.SetActive(true);

        startPos = cardTransform.position;
        startRot = cardTransform.rotation;
        startScale = cardTransform.lossyScale;

        float timer = 0;
        while(timer < animationTime)
        {
            timer += Time.deltaTime;

            float blend = animationCurve.Evaluate(timer / animationTime);

            transform.position = Vector3.Lerp(startPos, cardViewReferenceTransform.position, blend);
            transform.rotation = Quaternion.Lerp(startRot, cardViewReferenceTransform.rotation, blend);
            transform.localScale = Vector3.Lerp(startScale, cardViewReferenceTransform.localScale, blend);

            yield return null;
        }

        callbackAction?.Invoke();
    }

    /*public void HideCardViewInput(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled)
            return;

        HideCardView();
    }*/

    public static void HideCardView(Action callbackAction = null)
    {
        if (!isOn)
            return;

        instance.StartCoroutine(instance.CardViewOffAnimation(callbackAction));
    }

    private IEnumerator CardViewOffAnimation(Action callbackAction)
    {
        cardViewUI.ShowUI(false);

        isOn = false;

        float timer = 0;
        while (timer < animationTime)
        {
            timer += Time.deltaTime;

            float blend = animationCurve.Evaluate(timer / animationTime);

            transform.position = Vector3.Lerp(cardViewReferenceTransform.position, startPos, blend);
            transform.rotation = Quaternion.Lerp(cardViewReferenceTransform.rotation, startRot, blend);
            transform.localScale = Vector3.Lerp(cardViewReferenceTransform.localScale, startScale, blend);

            yield return null;
        }

        cardObject.gameObject.SetActive(false);

        if(instance.gameCardObject != null)
            instance.gameCardObject.EnableRenderer(true);

        callbackAction?.Invoke();

        gameMap.Enable();
    }
}
