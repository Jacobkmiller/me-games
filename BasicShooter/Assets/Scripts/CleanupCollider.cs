using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanupCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        GameObject bullet = other.gameObject;
        if (bullet != null && !bullet.CompareTag("Player"))
        {
            print("caught a bullet");
            Destroy(bullet);
        }
        else
        {
            print("could not cast bullet");
        }
    }
}
