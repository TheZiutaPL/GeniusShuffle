using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// This script provides a way to move objects around specific points of the scene

public class PointBasedMovement : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] AnimationCurve speedCurve;

    [Space(10)]
    [SerializeField] UnityEvent onMovementStarted;
    [SerializeField] UnityEvent onDestinationReached;

    Vector3 startPosition;
    Quaternion startRotation;
    Transform destination;

    bool moving = false;

    private void Update()
    {
        if (moving)
        {
            float distancePercentage = Vector3.Distance(startPosition, transform.position) / Vector3.Distance(startPosition, destination.position);
            float t = distancePercentage + (Time.deltaTime * speed * speedCurve.Evaluate(distancePercentage));

            transform.position = Vector3.Lerp(startPosition, destination.position, t);
            transform.rotation = Quaternion.Slerp(startRotation, destination.rotation, t);

            if (distancePercentage == 1f) // When the destination is reached
            {
                if (onDestinationReached != null)
                    onDestinationReached.Invoke();

                moving = false;
            }
        }
    }
    
    // Please use MoveTo() unless you specifically want to skip checking if it's already moving
    public void ForceMoveTo(Transform destination)
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        this.destination = destination;

        if (Vector3.Distance(startPosition, destination.position) == 0) // If it's already at the destination
            return;

        if (onMovementStarted != null)
            onMovementStarted.Invoke();

        moving = true;
    }

    public void MoveTo(Transform destination)
    {
        if (moving)
            return;

        ForceMoveTo(destination);
    }
}
