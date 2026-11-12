
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float distance = 5f;
    public float height = 2f;
    public float sensitivity = 0.10f; //camera sensitivity for mouse movement

    private float vertRotation;
    private float horRotation = 15f;

    void Start()
    {
        // Start facing the same direction as the player
        horRotation = player.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate() // We want the camera to update after the player has moved, so we use LateUpdate
    {
        if (player == null || Mouse.current == null)
            return;

        // Get mouse movement
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        // Rotate camera horizontally and vertically
        horRotation += mouseDelta.x * sensitivity;
        vertRotation -= mouseDelta.y * sensitivity;

        vertRotation = Mathf.Clamp(vertRotation, -20f, 70f);

        // Rotate the player to match the camera's horizontal direction
        player.rotation = Quaternion.Euler(0, horRotation, 0);

        //Calculate camera position relative to player based on rotation and distance
        Quaternion rotation = Quaternion.Euler(vertRotation, horRotation, 0);
        Vector3 targetPosition = player.position + Vector3.up * height;
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        transform.position = targetPosition + offset; // Set camera position behind the player

        // Make camera look at the player
        transform.LookAt(targetPosition);
    }
}
