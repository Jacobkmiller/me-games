using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;

	void Start()
	{
		if (cam == null)
		{
			Debug.LogError("Player Shoot no camera referenced");
			this.enabled = false;
		}
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Shoot();
		}
	}

	[Client]
	void Shoot() {
        //shooting using objects
        //the weapon should be doing the shooting
        // forward firing start location
        Vector3 start = cam.transform.position;
        start += cam.transform.forward.normalized * 1;
        var bullet = (GameObject)Instantiate(weapon.Ammo, start, cam.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = cam.transform.forward * weapon.Speed;

        //raycast shooting[old]
		RaycastHit _hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
			// We hit something
			if (_hit.collider.tag == PLAYER_TAG) {
				CmdPlayerShot(_hit.collider.name, weapon.Damage);
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage) {
		// Debug.Log(_playerID + " has been shot!");

		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);
	}

}
