using UnityEngine;

public class Ammo : MonoBehaviour
{   
    [SerializeField]
    private GameObject ammo;
    private void OnCollisionEnter(Collision collision)
    {
        // print("projectile collided with object");
        if (collision.gameObject.CompareTag("Player")) {
            collision.rigidbody.AddForce(collision.relativeVelocity/(-10), ForceMode.Impulse);
        } else {
            Destroy(ammo);
        }
        
    }

    public int Damage = 10;
    public int Speed = 100;
}