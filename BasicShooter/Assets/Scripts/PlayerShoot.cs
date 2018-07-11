using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	private const string PLAYER_TAG = "Player";

	public PlayerWeapon weapon;

	[SerializeField]
	private Camera cam;

	[SerializeField]
	private AudioSource gunSound;

	[SerializeField]
	private LayerMask mask;

	private Transform dynamic;

	private bool secondaryFireMode = true;

	void Start()
	{
		if (cam == null)
		{
			Debug.LogError("Player Shoot no camera referenced");
			this.enabled = false;
		}
		if (dynamic == null) {
			dynamic = GameObject.Find("_Dynamic").transform;
		}
	}

	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			Shoot();
		}
		if (Input.GetButtonDown("ToggleFireMode")) {
			ToggleFireMode();
		}
	}

	private void ToggleFireMode() {
		secondaryFireMode = !secondaryFireMode;
	}

	[Client]
	void Shoot() {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        gunSound.PlayOneShot(gunSound.clip, 1);
        CmdPlayWeaponEffects();
        if (secondaryFireMode) {
			// TODO: Lets make this a specific weapon
			//shooting using objects
			// the weapon should be doing the shooting
			// forward firing start location
			Vector3 start = cam.transform.position;
			start += cam.transform.forward.normalized * 1;
			var bullet = (GameObject)Instantiate(weapon.Ammo, start, cam.transform.rotation, dynamic);
			bullet.GetComponent<Rigidbody>().velocity = cam.transform.forward * weapon.Speed;
		} else {
			//raycast shooting[old]
			RaycastHit _hit;
			if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask)) {
				// We hit something
				if (_hit.collider.tag == PLAYER_TAG) {
					CmdPlayerShot(_hit.collider.name, weapon.Damage);
				}
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _playerID, int _damage) {
		// Debug.Log(_playerID + " has been shot!");

		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage);
	}

	[Command]
	void CmdPlayWeaponEffects () {
		// Debug.Log(_playerID + " has been shot!");
		var _player = GetComponent<Player>();
		// Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcPlayWeaponEffects();
	}

}
