using UnityEngine;

namespace ScrapEscape.RiverBlockout
{
    public class BlockoutButton : MonoBehaviour
    {
        public Rigidbody scrap;
        public BlockoutMover mechanism;
        public Renderer indicator;
        public bool needsThrow;
        public BlockoutProgress progress;
        public int checkpoint = -1;
        private bool activated;

        private void OnTriggerStay(Collider other)
        {
            if (!needsThrow && other.attachedRigidbody == scrap) Activate();
        }

        private void OnCollisionEnter(Collision hit)
        {
            if (needsThrow && hit.rigidbody == scrap && scrap.GetComponent<ThrownObject>() != null
                && hit.relativeVelocity.magnitude >= 2f) Activate();
        }

        private void Activate()
        {
            if (activated) return;
            activated = true;
            indicator.material.color = Color.green;
            mechanism.Activate();
            if (progress != null && checkpoint >= 0) progress.ReachCheckpoint(checkpoint);
        }
    }
}
