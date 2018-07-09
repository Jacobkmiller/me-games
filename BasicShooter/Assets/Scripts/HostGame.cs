using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {
	[SerializeField]
	private uint roomSize = 4;

	private string roomName;
	private NetworkManager networkManager;
	
	public void SetRoomName(string _name) {
		roomName = _name;
	}

	void Start(){
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			Debug.Log("Match Maker Started.");
			networkManager.StartMatchMaker();
		}
	}
	public void CreateRoom(){
		Debug.Log("Trying to create room " + roomName + "...");
		if (roomName != "" && roomName != null) {
			Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "","", 0, 0, networkManager.OnMatchCreate);
		}
	}
}
