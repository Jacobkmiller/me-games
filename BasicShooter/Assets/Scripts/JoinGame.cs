using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	[SerializeField]
	private Text status;
	[SerializeField]
	private GameObject matchListItemPrefab;
	[SerializeField]
	private Transform matchListParent;
	private NetworkManager networkManager;
	List<GameObject> matchList = new List<GameObject>();

	void Start() {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			Debug.Log("Match Maker Started.");
			networkManager.StartMatchMaker();
		}

		RefreshMatchList();
	}

	public void RefreshMatchList() {
		ClearRoomList();
		if (networkManager.matchMaker == null) {
			Debug.Log("Match Maker Started.");
			networkManager.StartMatchMaker();
		}
		networkManager.matchMaker.ListMatches(0,20, "", true, 0, 0, OnMatchList);
		status.text = "Loading...";
	}

	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
		status.text = "";
		if (!success) {
			status.text = "Couldn't get match list.";
			return;
		}

		foreach (MatchInfoSnapshot match in matches) {
			GameObject matchListItemGO = Instantiate(matchListItemPrefab);
			matchListItemGO.transform.SetParent(matchListParent);
			MatchListItem matchListItem = matchListItemGO.GetComponent<MatchListItem>();
			if (matchListItem != null) {
				matchListItem.Setup(match, JoinMatch);
			}
			matchList.Add(matchListItemGO);
		}

		if (matchList.Count == 0) {
			status.text = "No rooms available";
		}
	}

	void ClearRoomList() {
		for (int i = 0; i < matchList.Count; i++){
			Destroy(matchList[i]);
		}

		matchList.Clear();
	}

	public void JoinMatch(MatchInfoSnapshot _match) {
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
		StartCoroutine(WaitForJoin());
	}

	IEnumerator WaitForJoin() {
		ClearRoomList();
		int countdown = 10;
		while (countdown > 0) {
			status.text = "Joining... (" + countdown + ")";
			yield return new WaitForSeconds(1);
			countdown -= 1;
		}

		//Failed to connect
		status.text = "Failed to connect :(";
		yield return new WaitForSeconds(1);
		MatchInfo matchInfo = networkManager.matchInfo;
		if (matchInfo != null) {
			networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
			networkManager.StopHost();
		}

		RefreshMatchList();
	}

}
