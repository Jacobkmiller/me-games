﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FallDeathOneWay : MonoBehaviour {

	// / <summary>
	// / OnTriggerEnter is called when the Collider other enters the trigger.
	// / </summary>
	// / <param name="other">The other Collider involved in this collision.</param>
	List<string> players = new List<string>(4);
	void OnTriggerEnter(Collider other) {
		Debug.Log("TRIGGERED!");
		if (other.tag == "Player") {
			Player player = other.GetComponentInParent<Player>();
			// PlayerShoot shootObj = other.GetComponentInParent<PlayerShoot>();
			if (players.Contains(player.name)) {
				player.Kill();
				players.Remove(player.name);
			} else {
				players.Add(player.name);
			}
		}
	}

}
