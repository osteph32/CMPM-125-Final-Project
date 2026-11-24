using UnityEngine;

// Moves doors and the bridge to their raised positions
namespace ScrapEscape.RiverBlockout
{
    [RequireComponent(typeof(Rigidbody))]
    public class BlockoutMover : MonoBehaviour
    {
        public Vector3 travel = new Vector3(0, 4, 0);
        public float speed = 2f;
        public bool Activated { get; private set; }
        private Rigidbody body;
        private Vector3 destination;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;
            destination = body.position + travel;
        }

        public void Activate() { Activated = true; }

        private void FixedUpdate()
        {
            if (Activated)
                body.MovePosition(Vector3.MoveTowards(body.position, destination, speed * Time.fixedDeltaTime));
        }
    }
}
