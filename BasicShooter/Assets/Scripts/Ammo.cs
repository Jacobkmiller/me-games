using UnityEngine;
using UnityEngine.Networking;

public class Ammo : NetworkBehaviour
{
    public int Damage = 10;
    public int Speed = 100;
    public int Force = 1;

    [SerializeField]
    private GameObject ammo;
    private void OnCollisionEnter(Collision collision)
    {
        print("projectile collided with object: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player")) {
            CmdPlayerShot(collision.collider.name, Damage);
            collision.rigidbody.AddForce(collision.relativeVelocity/(-1 * Force), ForceMode.Impulse);
        } else {
            Destroy(ammo);
        }
        
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage)
    {
        // Debug.Log(_playerID + " has been shot!");

        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage);
    }

}