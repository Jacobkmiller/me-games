using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
	// [SerializeField]
	// GameObject playerUIPrefab;
	// private GameObject playerUIInstance;

	public void Setup() {
		wasEnabled = new bool[disableOnDeath.Length];
		for (int i =0; i < wasEnabled.Length; i++){
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		// this.playerUI = Instantiate(playerUIPrefab);
		SetDefaults();
	}

	public void Update() {
		bool _k = Input.GetKey("k");
		if (_k) {
			RpcTakeDamage(10);
		}
	}
	private IEnumerator Respawn() {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;
	}
	public void SetDefaults() {
		currentHealth = maxHealth;
		PlayerSetup _playerSetup = GetComponent<PlayerSetup>();
		_playerSetup.ChangeHealth(currentHealth);
		isDead = false;
		for (int i = 0; i < disableOnDeath.Length; i++){
			 disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null){
			_col.enabled = true;
		}
	}
	[ClientRpc]
	public void RpcTakeDamage(int _amount) {
		if (isDead) {
			return;
		}
		currentHealth -= _amount;

		currentHealth = Mathf.Max(0, currentHealth);

		// Debug.Log(transform.name + " now has " + currentHealth + " health!");
		PlayerSetup _playerSetup;
		//Get PlayerSetup Object that has the PlayerUIInstance. Then run the function to change the health.
		_playerSetup = GetComponent<PlayerSetup>();
		_playerSetup.ChangeHealth(currentHealth);

		if (currentHealth <= 0) {
			Die();
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

		Debug.Log(transform.name + " is dead!");
		//RESPAWN
		StartCoroutine(Respawn());

	}
}
