using UnityEngine;

[System.Serializable]
public class PlayerWeapon : MonoBehaviour {

    public GameObject Ammo;
	public string name = "Magnum Dong";

    public int Damage
    {
        get { return Ammo.GetComponent<Ammo>().Damage; }
    }
    public int Speed
    {
        get { return Ammo.GetComponent<Ammo>().Speed; }
    }
	public float range = 100f;
}
