using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeath : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Player player = other.GetComponentInParent<Player>();
			// player.GetComponent<PlayerUI>().SetHealth(0);
			player.RpcTakeDamage(101);
			// player.Die();
		}
	}

}
