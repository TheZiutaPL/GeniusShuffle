using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script provides a way to move the camera around specific points of the scene

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] AnimationCurve speedCurve;

    Transform cam;

    Vector3 startPosition;
    Quaternion startRotation;
    Transform destination;

    bool moving = false;

    private void Start()
    {
        cam = Camera.main.transform;
    }

    private void Update()
    {
        if (moving)
        {
            float distancePercentage = Vector3.Distance(startPosition, cam.position) / Vector3.Distance(startPosition, destination.position);
            float t = distancePercentage + (Time.deltaTime * speed * speedCurve.Evaluate(distancePercentage));

            cam.transform.position = Vector3.Lerp(startPosition, destination.position, t);
            cam.transform.rotation = Quaternion.Slerp(startRotation, destination.rotation, t);

            if (distancePercentage == 1f) // So when the destination is reached
                moving = false;
        }
    }

    public void MoveTo(Transform destination)
    {
        if (moving)
            return;

        startPosition = cam.position;
        startRotation = cam.rotation;

        this.destination = destination;

        moving = true;
    }
}
