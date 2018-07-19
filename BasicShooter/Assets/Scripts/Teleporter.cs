using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

	[SerializeField]
	public Vector3 teleportTo;
	/// <summary>
	/// OnTriggerExit is called when the Collider other exits the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerExit(Collider other) {
		Vector3 positionVector = this.transform.position - other.transform.position;
		float dotProduct = Vector3.Dot(this.transform.up, positionVector);
		Debug.Log("TELEPORT");
		Debug.Log(dotProduct);
		if (dotProduct > 0) {
			other.transform.position = other.transform.position + teleportTo;
		}

	}

}
	
