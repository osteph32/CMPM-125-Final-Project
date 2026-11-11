using UnityEngine;
using UnityEngine.InputSystem;

// References:
// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19/manual/Mouse.html
// https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.AddForce.html

public class SimplePushPull : MonoBehaviour
{
    public Rigidbody scrap;
    public Transform holdPoint;
    public float pullStrength = 20f;
    public float damping = 8f;
    public float pushStrength = 5f;

    private bool pulling;

    private void Update()
    {
        if (Mouse.current == null || scrap == null || holdPoint == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame) pulling = true;
        if (!Mouse.current.leftButton.isPressed) pulling = false;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            pulling = false;
            Vector3 away = scrap.worldCenterOfMass - transform.position;
            away.y = 0f;
            if (away.sqrMagnitude < 0.001f) away = transform.forward;
            scrap.AddForce(away.normalized * pushStrength, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        if (!pulling || scrap == null || holdPoint == null || scrap.isKinematic) return;
        Vector3 force = (holdPoint.position - scrap.worldCenterOfMass) * pullStrength;
        force -= scrap.linearVelocity * damping;
        if (scrap.useGravity) force -= Physics.gravity;
        scrap.AddForce(force, ForceMode.Acceleration);
    }

    private void OnDisable() => pulling = false;
}
