using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
// [RequireComponent(typeof(PlayerUI))]
public class PlayerSetup : NetworkBehaviour {
	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;
	[SerializeField]
	GameObject playerUIPrefab;
	private GameObject playerUIInstance;
	PlayerUI playerUI;
	Camera sceneCamera;
	void Start() {
		if (!isLocalPlayer) {
			DisableComponents();
			AssignRemoteLayer();
		} else {
			sceneCamera = Camera.main;
			if (sceneCamera != null) {
				sceneCamera.gameObject.SetActive(false);
			}

			//Disable player graphics for local player
			SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
			//Create Player UI
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;
			playerUI = playerUIInstance.GetComponent<PlayerUI>();
			if (playerUI == null) {
				Debug.Log("No PlayerUI component on playerUI prefab");
			}
			GetComponent<Player>().PlayerSetup();
		}
	}

	void SetLayerRecursively(GameObject obj, int newLayer){
		obj.layer = newLayer;
		foreach (Transform child in obj.transform){
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	public override void OnStartClient() {
		base.OnStartClient();

		string _netID = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();

		GameManager.RegisterPlayer(_netID, _player);
	}

	void AssignRemoteLayer () {
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents () {
    for (int i = 0; i < componentsToDisable.Length; i++)
    {
      componentsToDisable[i].enabled = false;
    }
	// Player _player = GetComponent<Player>();
	// _player.GetComponent.enabled = false;
	}
	
	void OnDisable()
	{
		if (sceneCamera != null) {
			sceneCamera.gameObject.SetActive(true);
		}

		GameManager.UnRegisterPlayer(transform.name);
	}

	public PlayerUI getUI() {
		return playerUI;
	}

}
