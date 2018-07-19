using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RandomLevelGen : NetworkBehaviour {

	public int averageGap;
	public int sideLength;
	public float randomGapDivisor;
	public GameObject spawnPrefab;
	public GameObject platformPrefab;
	[SerializeField]
	private GameObject teleportationPrefab;
	private Transform plats;
	private float spawnDistance= 110f;
	private float maxDistance;
	[SyncVar]
	private int seed;
	void Start () {
		if (Network.isServer){
			seed = Random.Range(1, int.MaxValue);
			Debug.Log(seed);
		}
		Random.InitState(seed);
		plats = GameObject.Find("Platforms").transform;
		maxDistance = averageGap*sideLength+averageGap/randomGapDivisor/2;

		GenerateSpawnLocations();
		GeneratePlatforms(sideLength);
		DuplicateEnvironments();
		SetupTeleportationPlanes();
	}

	private void GenerateSpawnLocations() {
		float posx = sideLength/2*averageGap;
		float posy = spawnDistance;
		float posz = sideLength/2*averageGap;
		GameObject spawn1 = Instantiate(spawnPrefab);
		GameObject spawn2 = Instantiate(spawnPrefab);
		GameObject spawn3 = Instantiate(spawnPrefab);
		GameObject spawn4 = Instantiate(spawnPrefab);
		// spawn1.transform.SetParent(env);
		// spawn2.transform.SetParent(env);
		// spawn3.transform.SetParent(env);
		// spawn4.transform.SetParent(env);
		// NetworkServer.Spawn(spawn1);
		// NetworkServer.Spawn(spawn2);
		// NetworkServer.Spawn(spawn3);
		// NetworkServer.Spawn(spawn4);
		spawn1.transform.position = new Vector3(posx, posy, posz);
		spawn2.transform.position = new Vector3(-posx, posy, posz);
		spawn3.transform.position = new Vector3(posx, posy, -posz);
		spawn4.transform.position = new Vector3(-posx, posy, -posz);
		Transform platform2 = spawn2.transform.Find("Platform");
		Transform platform3 = spawn3.transform.Find("Platform");
		Transform platform4 = spawn4.transform.Find("Platform");
		platform2.position = new Vector3(-posx+4f,posy,posz-4f);
		platform3.position = new Vector3(posx-4f,posy,-posz+4f);
		platform4.position = new Vector3(-posx+4f,posy,-posz+4f);
		platform2.Rotate(0f,-90f,0);
		platform3.Rotate(0f, 90f, 0f);
		platform4.Rotate(0f, 180f, 0f);
	}

	private void PositionStart(GameObject obj, float x, float y, float z) {
		obj.transform.position = new Vector3(x,y,x);
	}

	private void GeneratePlatforms(int sideLength) {
		List<GameObject> platforms = new List<GameObject>((int)Mathf.Pow(sideLength,3));
		float x = -sideLength/2*averageGap+averageGap/2;
		float z = -sideLength/2*averageGap+averageGap/2;
		float y = spawnDistance + averageGap/3;
		for (int i = 0; i < sideLength; i++) {
			for (int j = 0; j < sideLength; j++) {
				for (int k = 0; k < sideLength; k++){
					GameObject platform = Instantiate(platformPrefab);
					// NetworkServer.Spawn(platform);
					platform.transform.SetParent(plats);
					platform.transform.position = new Vector3(x+i*averageGap, y+k*averageGap, z+j*averageGap);
					platform.transform.Rotate(0f, Random.Range(0,360), 0f);
					platforms.Add(platform);
				}
			}
		}
		foreach (GameObject pf in platforms) {
			Vector3 currentPosition = pf.transform.position;
			Vector3 newPosition = currentPosition + Random.onUnitSphere*Random.Range(0f,averageGap/randomGapDivisor);
			pf.transform.position = newPosition;
			// pf.transform.SetParent(env);
		}
	}

	private void DuplicateEnvironments() {
		GameObject environment = GameObject.Find("Environment");
		Vector3 clonePosition = new Vector3(-maxDistance, 0, 0);
		Instantiate(environment, clonePosition, Quaternion.identity);
		clonePosition = new Vector3(maxDistance, 0, 0);
		Instantiate(environment, clonePosition, Quaternion.identity);
		clonePosition = new Vector3(0, 0, -maxDistance);
		Instantiate(environment, clonePosition, Quaternion.identity);
		clonePosition = new Vector3(0, 0, maxDistance);
		Instantiate(environment, clonePosition, Quaternion.identity);
	}

	private void SetupTeleportationPlanes() {
		GameObject telePlane = Instantiate(teleportationPrefab);
		telePlane.transform.Rotate(90f, 0f, 0f);
		telePlane.transform.position = new Vector3(0f, spawnDistance, -maxDistance/2);
		telePlane.GetComponent<Teleporter>().teleportTo = new Vector3(0f, 0f, maxDistance);
		telePlane = Instantiate(telePlane);
		telePlane.transform.Rotate(180f, 0f, 0f);
		telePlane.transform.position = new Vector3(0f, spawnDistance, maxDistance/2);
		telePlane.GetComponent<Teleporter>().teleportTo = new Vector3(0f, 0f, -maxDistance);

		telePlane = Instantiate(teleportationPrefab);
		telePlane.transform.Rotate(0f, 0f, 90f);
		telePlane.transform.position = new Vector3(maxDistance/2, spawnDistance, 0f);
		telePlane.GetComponent<Teleporter>().teleportTo = new Vector3(-maxDistance, 0f, 0f);
		telePlane = Instantiate(telePlane);
		telePlane.transform.Rotate(0f, 0f, 180f);
		telePlane.transform.position = new Vector3(-maxDistance/2, spawnDistance, 0f);
		telePlane.GetComponent<Teleporter>().teleportTo = new Vector3(maxDistance, 0f, 0f);

	}
		


}
