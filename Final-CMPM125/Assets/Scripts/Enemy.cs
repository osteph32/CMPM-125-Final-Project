using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
{
    PlayerHealth health =
        collision.gameObject.GetComponent<PlayerHealth>(); //get health component of player
        
    if (health != null)
    {
        health.TakeDamage(1); //subtract health from player
    }
}


}
