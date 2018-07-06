using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGen : MonoBehaviour {

	// Use this for initialization
	// public int platformGapMax;
	// public int platformGapMin;
	// public int spawnGapMin;
	// public int spawnGapMax;
	public int averageGap;
	// public int numberOfSpawns;
	// public int numberOfPlatforms;
	public int sideLength;
	public float randomGapDivisor;
	public GameObject spawnPrefab;
	public GameObject platformPrefab;
	private float baseDistance= 110f;
	void Start () {
		GenerateSpawnLocations();
		GeneratePlatforms(sideLength);
	}

	private void GenerateSpawnLocations() {
		// for (int i = 0; i < numberOfSpawns; i++) {
			// GameObject spawn = Instantiate(spawnPrefab);
			// Quaternion rotQ = Random.rotation;
			// Vector3 rot = new Vector3(rotQ.eulerAngles.x, 0f, rotQ.eulerAngles.z);
			// Vector3 rot = new Vector3(0f, rotQ.eulerAngles.y, 0f);
			// float distance = Random.Range(spawnGapMin, spawnGapMax);
			// spawn.transform.position = rot*distance;
			// Vector3 p = spawn.transform.position;
			// p.y = -110f;
			// spawn.transform.position = p;
			
		// }
		float posx = sideLength/2*averageGap;
		float posy = baseDistance;
		float posz = sideLength/2*averageGap;
		GameObject spawn1 = Instantiate(spawnPrefab);
		GameObject spawn2 = Instantiate(spawnPrefab);
		GameObject spawn3 = Instantiate(spawnPrefab);
		GameObject spawn4 = Instantiate(spawnPrefab);
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
		float y = baseDistance + averageGap/3;
		for (int i = 0; i < sideLength; i++) {
			for (int j = 0; j < sideLength; j++) {
				for (int k = 0; k < sideLength; k++){
					GameObject platform = Instantiate(platformPrefab);
					platform.transform.position = new Vector3(x+i*averageGap, y+k*averageGap, z+j*averageGap);
					platform.transform.Rotate(0f, Random.Range(0,360), 0f);
					platforms.Add(platform);
				}
			}
		// 	GameObject platform = Instantiate(platformPrefab);
		// 	float rotx = Random.Range(0,90);
		// 	float roty = Random.Range(0,360);
		// 	// Vector3 rot = new Vector3(rotQ.eulerAngles.x, 0f, rotQ.eulerAngles.z);
		// 	Quaternion rot = Quaternion.Euler(0f, rotx, roty);
		// 	// Vector3 distance = Vector3.forward*Random.Range(platformGapMin, platformGapMax);
		// 	platform.transform.position = rot*distance;
		// 	Quaternion rotQ = Random.rotation;
			// Quaternion rotyPlatform = Quaternion.Euler(0f, 0f, rotQ.eulerAngles.y);
			// platform.transform.rotation = rotyPlatform;
			

		}
		foreach (GameObject pf in platforms) {
			Vector3 currentPosition = pf.transform.position;
			Vector3 newPosition = currentPosition + Random.onUnitSphere*averageGap/randomGapDivisor;
			pf.transform.position = newPosition;
		}
	}


}
