using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("projectile collided with object");
        Destroy(this);
    }

    public int Damage = 10;
    public int Speed = 100;
}