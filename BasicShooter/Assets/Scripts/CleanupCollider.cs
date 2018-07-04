using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        var bullet = other.GetComponent<Ammo>();
        if (bullet != null)
        {
            print("caught a bullet");
            Destroy(bullet.gameObject);
        }
        else
        {
            print("could not cast bullet");
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
