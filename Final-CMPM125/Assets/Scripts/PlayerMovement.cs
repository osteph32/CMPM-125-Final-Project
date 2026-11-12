using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private Vector2 movement;
    private bool jumpPressed;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get WASD input
        movement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            movement.y = 1;

        if (Keyboard.current.sKey.isPressed)
            movement.y = -1;

        if (Keyboard.current.aKey.isPressed)
            movement.x = -1;

        if (Keyboard.current.dKey.isPressed)
            movement.x = 1;

        // Get jump input
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpPressed = true;
    }

    void FixedUpdate()
    {
        // Check if the player is touching the ground
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            GetComponent<Collider>().bounds.extents.y + 0.15f
        );

        // Jump if the player is grounded
        if (jumpPressed && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        jumpPressed = false;

        // Calculate movement relative to player
        Vector3 moveDirection = transform.forward * movement.y +
                                transform.right * movement.x;

        // Prevent diagonal movement from being faster
        moveDirection.Normalize();

        // Stop horizontal movement when no input is pressed
        if (movement == Vector2.zero)
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

        // Move player using Rigidbody
        rb.MovePosition(
            rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime
        );
    }
}