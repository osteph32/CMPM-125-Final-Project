using UnityEngine;

namespace ScrapEscape.RiverBlockout
{
    public class BlockoutBreakPieces : MonoBehaviour
    {
        public GameObject pieces;
        private bool broken;

        private void OnCollisionEnter(Collision hit)
        {
            if (broken || hit.rigidbody == null || hit.rigidbody.GetComponent<ThrownObject>() == null) return;
            broken = true;
            pieces.SetActive(true);
            foreach (Rigidbody body in pieces.GetComponentsInChildren<Rigidbody>())
                body.AddExplosionForce(6f, hit.rigidbody.position, 5f, 0.6f, ForceMode.Impulse);
        }
    }
}
