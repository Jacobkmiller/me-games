using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	private Text healthText;
	private Player player;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start() {
		// UI = Instantiate(playerUIPrefab);
		// UI.name = playerUIPrefab.name;
	}

	public void SetPlayer(Player _player) {
		player = _player;
	}

	public void SetHealth(int value){
		// UI.GetComponentInChildren<Text>().text = value + "%";
		healthText.text = value + "%";
	}

	


}

