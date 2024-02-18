using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
    [SerializeField] private Transform globeSphereTransform;

    [SerializeField] private float minRotationDegree = 100;
    [SerializeField] private float maxRotationDegree = 720;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private float timePerRotationDegree;

    private Vector2 startXYRot;
    private Coroutine globeAnimationCoroutine;

    private void Awake()
    {
        startXYRot = globeSphereTransform.localEulerAngles;
    }

    public void RandomizePosition()
    {
        Debug.Log("Glove got clicked");

        if (globeAnimationCoroutine != null)
            StopCoroutine(globeAnimationCoroutine);

        globeAnimationCoroutine = StartCoroutine(GlobeAnimation());
    }

    IEnumerator GlobeAnimation()
    {
        float startZRot = globeSphereTransform.localEulerAngles.z;

        float rotationDistance = Random.Range(minRotationDegree, maxRotationDegree);
        Debug.Log($"Rotation Distance = {rotationDistance}");

        float endZRot = startZRot + rotationDistance;

        float rotationTime = rotationDistance * timePerRotationDegree;
        Debug.Log($"Rotation Time = {rotationTime}");

        float timer = 0;

        while(timer < rotationTime)
        {
            timer += Time.deltaTime;

            float blend = timer / rotationTime;
            globeSphereTransform.localEulerAngles = new Vector3(startXYRot.x, startXYRot.y, Mathf.Lerp(startZRot, endZRot, rotationCurve.Evaluate(blend) % 360));
            yield return null;
        }

        globeAnimationCoroutine = null;
    }
}
