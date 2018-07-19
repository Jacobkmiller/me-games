using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour {

	// Use this for initialization
	[SerializeField]
	private int maxHealth = 100;
	[SyncVar]
	private int currentHealth;
	[SyncVar]
	private bool _isDead = false;
	public bool isDead {
		get {return _isDead;}
		protected set {_isDead = value;}
	}
	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	[SerializeField]
	private Vector3 launchVelocity;
	private PlayerUI playerUI;
	[SerializeField]
	Camera playerCamera;
	[SerializeField]
	PostProcessLayer deathShadow;
	private Scene scene;

	private bool firstSetup = true;
	public void PlayerSetup() {
		CmdBroadcastNewPlayerSetup();
	}
	public void Start() {
		if (isLocalPlayer) {
			playerUI = GetComponent<PlayerSetup>().getUI();
		}
		scene = SceneManager.GetActiveScene();
	}
	[Command]
	private void CmdBroadcastNewPlayerSetup() {
		RpcSetupPlayerOnAllClients();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients() {
		if (firstSetup) {
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i =0; i < wasEnabled.Length; i++){
				wasEnabled[i] = disableOnDeath[i].enabled;
			}
			firstSetup = false;
		}
		SetDefaults();
	}

	[ClientRpc]
	public void RpcPlayWeaponEffects() {
		if (!isLocalPlayer){
    		gameObject.GetComponentInChildren<ParticleSystem>().Play();
			AudioSource _audiosource = gameObject.GetComponentInChildren<AudioSource>();
			_audiosource.PlayOneShot(_audiosource.clip, 1);

		}
	}

	public void Update() {
		bool _k = Input.GetKey("k");
		if (_k) {
			RpcTakeDamage(10);
		}
	}
	private IEnumerator Respawn() {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
		PlayerSetup();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
	}

	public void SetDefaults() {
		currentHealth = maxHealth;
		deathShadow.enabled = false;
		if (isLocalPlayer) {
			playerUI.SetHealth(currentHealth);
		}
		isDead = false;
		for (int i = 0; i < disableOnDeath.Length; i++){
			 disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null){
			_col.enabled = true;
		}
		Rigidbody rb = GetComponent<Rigidbody>();
		if (scene.name == "RandomLevel") {
			rb.velocity = launchVelocity;
		} else {
			rb.velocity = new Vector3(0f,0f,0f);
		}
	}
	[ClientRpc]
	public void RpcTakeDamage(int _amount) {
		if (isDead) {
			return;
		}
		currentHealth -= _amount;

		currentHealth = Mathf.Max(0, currentHealth);

		if (isLocalPlayer) {
			playerUI.SetHealth(currentHealth);
			deathShadow.enabled = true;
			StartCoroutine(ClearVignette(1));
		}
		if (currentHealth <= 0) {
			Die();
		}
	}

	[ClientRpc]
	public void RpcShootBullet(Vector3 position, Vector3 velocity, Quaternion rotation) {
		if (!isLocalPlayer){ 
			PlayerShoot _shooter = GetComponent<PlayerShoot>();
			var bullet = (GameObject)Instantiate(_shooter.weapon.Ammo, position, rotation);
			bullet.GetComponent<Rigidbody>().velocity = velocity;
			
		}
	}


	public void Die() {
		isDead = true;
		//DISABLE COMPONENTS
		for (int i = 0; i < disableOnDeath.Length; i++){
			disableOnDeath[i].enabled = false;
		}

		Collider[] _cols;
		_cols = GetComponents<Collider>();
		foreach (Collider _col in _cols){
			if (_col != null) {
				_col.enabled = false;
			}
		}

		deathShadow.enabled = true;
		Debug.Log(transform.name + " is dead!");
		//RESPAWN
		StartCoroutine(Respawn());

	}

	private IEnumerator ClearVignette(int seconds) {
		yield return new WaitForSeconds(seconds);
		deathShadow.enabled = false;
	}

	public void Kill(){
		isDead = true;
		playerUI.SetHealth(0);
		Die();
	}

}
