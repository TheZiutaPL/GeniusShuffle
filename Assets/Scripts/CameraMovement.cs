using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script provides a way to move the camera around specific points of the scene

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float rotationSpeed = 0.5f;

    Transform cam;
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
            cam.transform.position = Vector3.Slerp(cam.transform.position, destination.position, moveSpeed / 100f);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, destination.rotation, rotationSpeed / 100f);

            // Because == operator between vectors is really unreliable this uses the difference between
            // current position and the destination to determine when to stop moving
            if ((cam.transform.position - destination.position).magnitude < 0.0001f)
                moving = false;
        }
    }

    public void MoveTo(Transform destination)
    {
        this.destination = destination;
        moving = true;
    }
}
