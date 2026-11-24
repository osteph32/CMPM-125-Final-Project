using UnityEngine;

// Brings the camera closer when a wall would block its view
namespace ScrapEscape.RiverBlockout
{
    [DefaultExecutionOrder(100)]
    public class BlockoutCameraClearance : MonoBehaviour
    {
        public CameraFollow follow;
        public Rigidbody scrap;

        private void LateUpdate()
        {
            Vector3 target = follow.player.position + Vector3.up * follow.height;
            Vector3 offset = transform.position - target;
            float distance = offset.magnitude;
            if (distance < 0.01f) return;
            foreach (RaycastHit hit in Physics.SphereCastAll(target, 0.18f, offset.normalized,
                distance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform == follow.player || hit.transform.IsChildOf(follow.player)
                    || hit.rigidbody == scrap) continue;
                distance = Mathf.Min(distance, Mathf.Max(0.3f, hit.distance - 0.1f));
            }
            transform.position = target + offset.normalized * distance;
            transform.LookAt(target);
        }
    }
}
