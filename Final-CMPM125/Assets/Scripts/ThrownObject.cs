
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasStartedMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object hit something breakable
        Breakable breakable =
            collision.collider.GetComponentInParent<Breakable>();

        if (breakable != null)
            Destroy(breakable.gameObject);

        // Reduce the object's velocity after collision
        rb.linearVelocity *= 0.3f;
        rb.angularVelocity *= 0.3f;
    }
    void FixedUpdate()
    {
        // Check if the object has started moving
        if (rb.linearVelocity.magnitude > 0.1f)
            hasStartedMoving = true;

        // Remove this component once the object stops moving
        if (hasStartedMoving && rb.linearVelocity.magnitude < 0.1f)
            Destroy(this);
    }
}
