using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Player player = other.GetComponentInParent<Player>();
			player.Kill();
		}
	}

}
