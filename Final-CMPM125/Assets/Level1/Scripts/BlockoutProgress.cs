using UnityEngine;
using UnityEngine.InputSystem;

namespace ScrapEscape.RiverBlockout
{
        public class BlockoutProgress : MonoBehaviour
    {
        public Rigidbody player;
        public Rigidbody scrap;
        public SimplePushPull magnet;
        public Transform[] playerSpawns;
        public Transform[] scrapSpawns;
        public int CurrentCheckpoint { get; private set; }
        private string message;
        private float messageUntil;
        
        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) RecoverCube();
            if (player.position.y < -8f) RecoverPlayer();
            else if (scrap.position.y < -8f) RecoverCube();
        }

        public void ReachCheckpoint(int index)
        {
            if (index > CurrentCheckpoint && index < playerSpawns.Length && index < scrapSpawns.Length)
                CurrentCheckpoint = index;
        }

        public void RecoverPlayer()
        {
            player.linearVelocity = Vector3.zero;
            player.angularVelocity = Vector3.zero;
            player.position = playerSpawns[CurrentCheckpoint].position;
            RecoverCube();
            ShowMessage("Back on safe ground. Your puzzle progress is kept.");
        }

        public void RecoverCube()
        {
            Vector3 position = scrapSpawns[CurrentCheckpoint].position;
            Bounds cubeBounds = new Bounds(position, Vector3.one);
            if (player.GetComponent<Collider>().bounds.Intersects(cubeBounds))
            {
                ShowMessage("Step away from the cyan recovery pad and press R again.");
                return;
            }
            magnet.ReleaseObject();
            ThrownObject thrown = scrap.GetComponent<ThrownObject>();
            if (thrown != null) Destroy(thrown);
            scrap.linearVelocity = Vector3.zero;
            scrap.angularVelocity = Vector3.zero;
            scrap.position = position;
            scrap.rotation = Quaternion.identity;
            ShowMessage("Scrap returned to the cyan pad.");
        }

        private void ShowMessage(string text)
        {
            message = text;
            messageUntil = Time.time + 3f;
        }
    }
}
