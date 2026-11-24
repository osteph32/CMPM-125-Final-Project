using UnityEngine;

namespace ScrapEscape.RiverBlockout
{
    public class BlockoutZone : MonoBehaviour
    {
        public BlockoutProgress progress;
        public bool hazard;
        public int checkpoint = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.attachedRigidbody == progress.player)
            {
                if (hazard) progress.RecoverPlayer();
                else progress.ReachCheckpoint(checkpoint);
            }
            else if (hazard && other.attachedRigidbody == progress.scrap)
                progress.RecoverCube();
        }
    }
}
