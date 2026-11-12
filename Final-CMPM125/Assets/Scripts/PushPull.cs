
using UnityEngine;
using UnityEngine.InputSystem;

// References:
// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19/manual/Mouse.html
// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.AddForce.html

public class SimplePushPull : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPoint;

    public float pullRange = 10f;
    public float pullStrength = 20f;
    public float damping = 8f;
    public float pushStrength = 5f;
    public float extraDistance = 1f; // Distance to hold the object away from the Player

    private Rigidbody pulledObject;
    private bool pulling;

    private void Update()
    {
        if (Mouse.current == null || playerCamera == null || holdPoint == null)
            return;

        // Hold left-click to pull an object
        if (Mouse.current.leftButton.wasPressedThisFrame && pulledObject == null)
            PullObject();

        // Release left-click to stop pulling
        if (!Mouse.current.leftButton.isPressed)
            pulling = false;

        // Right-click to throw the object away
        if (Mouse.current.rightButton.wasPressedThisFrame && pulledObject != null)
            ThrowObject();
    }

    private void FixedUpdate()
    {
        // Keep pulling the object toward the HoldPoint
        if (!pulling || pulledObject == null || pulledObject.isKinematic)
            return;

        UpdateHoldPoint();

        Vector3 force =
            (holdPoint.position - pulledObject.worldCenterOfMass) * pullStrength;

        // Reduce movement as the object approaches the HoldPoint
        force -= pulledObject.linearVelocity * damping;

        // Compensate for gravity while pulling
        if (pulledObject.useGravity)
            force -= Physics.gravity;

        pulledObject.AddForce(force, ForceMode.Acceleration);
    }

    private void PullObject()
    {
        // Find the closest object in front of the camera
        RaycastHit closestHit = FindClosestObject();

        // Check whether the object has a Pullable component
        Pullable pullable = closestHit.collider != null
            ? closestHit.collider.GetComponentInParent<Pullable>()
            : null;

        if (pullable == null)
            return;

        // Find the object's Rigidbody
        pulledObject = pullable.GetComponent<Rigidbody>();

        if (pulledObject == null)
            pulledObject = pullable.GetComponentInParent<Rigidbody>();

        if (pulledObject == null || pulledObject.isKinematic)
        {
            pulledObject = null;
            return;
        }

        pulling = true;

        // Calculate the HoldPoint based on the object's size
        UpdateHoldPoint();
    }

    private RaycastHit FindClosestObject()
    {
        // Shoot a ray through the center of the camera
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit[] hits = Physics.RaycastAll(ray, pullRange);

        RaycastHit closestHit = new RaycastHit();
        float closestDistance = pullRange;

        // Find the closest object, ignoring the Player
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform))
                continue;

            if (hit.distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = hit.distance;
            }
        }

        return closestHit;
    }

    private void UpdateHoldPoint()
    {
        // Get the object's collider
        Collider objectCollider = pulledObject.GetComponentInChildren<Collider>();

        if (objectCollider == null)
            return;

        // Calculate the distance from the center to a corner of the bounding box
        float objectSize = objectCollider.bounds.extents.magnitude;

        // Get the direction the Player is facing
        Vector3 direction = transform.forward;

        // Calculate the position in front of the Player
        Vector3 targetPosition = transform.position
            + direction * (extraDistance + objectSize);

        // Keep the object's height
        targetPosition.y = holdPoint.position.y;

        holdPoint.position = targetPosition;
    }

    private void ThrowObject()
    {
        // Aim away from the robot
        Vector3 throwDirection = (pulledObject.worldCenterOfMass - transform.position).normalized;

        // Add the ThrownObject component if it doesn't already exist
        if (pulledObject.GetComponent<ThrownObject>() == null)
            pulledObject.gameObject.AddComponent<ThrownObject>();

        // Launch the object
        pulledObject.AddForce(
            throwDirection * pushStrength,
            ForceMode.Impulse
        );

        // Stop pulling the object
        pulledObject = null;
        pulling = false;
    }
}
