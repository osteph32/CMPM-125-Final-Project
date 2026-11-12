
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPull : MonoBehaviour
{
    public Camera playerCamera;
    public Transform holdPoint;

    public float pullRange = 10f;
    public float throwForce = 40f;

    private Rigidbody heldObject;

    void Update()
    {
        if (Mouse.current == null) return;

        // Right-click to pull
        if (Mouse.current.rightButton.wasPressedThisFrame && heldObject == null)
            PullObject();

        // Left-click to throw
        if (Mouse.current.leftButton.wasPressedThisFrame && heldObject != null)
            ThrowObject();
    }

    void FixedUpdate()
    {
        // Keep the object at the HoldPoint
        if (heldObject != null)
            heldObject.MovePosition(holdPoint.position);
    }

    void PullObject()
    {
        // Find the closest object in front of the camera
        RaycastHit closestHit = FindClosestObject();

        // Check if the object has a Pullable component
        Pullable pullable = closestHit.collider != null
            ? closestHit.collider.GetComponentInParent<Pullable>()
            : null;

        if (pullable == null) return;

        // Find the object's Rigidbody
        heldObject = pullable.GetComponent<Rigidbody>();

        if (heldObject == null)
            heldObject = pullable.GetComponentInParent<Rigidbody>();

        if (heldObject == null)
            return;

        // Disable physics while holding it
        heldObject.isKinematic = true;
        heldObject.useGravity = false;
    }

    RaycastHit FindClosestObject()
    {
        // Shoot a ray through the center of the camera
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
    void ThrowObject()
    {
        // Aim in the direction the camera is facing
        Vector3 throwDirection = playerCamera.transform.forward;

        // Release the object
        heldObject.isKinematic = false;
        heldObject.useGravity = true;
        heldObject.linearVelocity = Vector3.zero;

        // Add the ThrownObject component
        if (heldObject.GetComponent<ThrownObject>() == null)
            heldObject.gameObject.AddComponent<ThrownObject>();

        // Launch the object
        heldObject.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        heldObject = null;
    }
}
