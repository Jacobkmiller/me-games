using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	private Text healthText;
	private Player player;
	// [SerializeField]
	// PostProcessLayer deathShadow;
	private GameObject playerUIInstance;
	[SerializeField]
	private GameObject pauseMenu;

	void Start() {
		PauseMenu.isOn = false;
	}


	void Update() {

		if (Input.GetKeyDown(KeyCode.Tab)) {
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu() {
		// pauseMenu = playerUIInstance.transform.Find("PauseMenu").gameObject;
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.isOn = pauseMenu.activeSelf;
		// player.TogglePauseMenu();
		// Debug.Log(playerUIInstance.transform.Find("PauseMenu").gameObject);
	}

	public void SetPlayer(Player _player) {
		player = _player;
		playerUIInstance = player.GetPlayerUIInstance();
	}

	public void SetHealth(int value){
		// GetComponent<Text>().text = value + "%";
		healthText.text = value + "%";
		// playerUIInstance.GetComponentInChildren<Text>().text = value + "%";
	}

	


}

