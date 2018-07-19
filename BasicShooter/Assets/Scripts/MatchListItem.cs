using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class MatchListItem : MonoBehaviour {

	public delegate void JoinMatchDelegate(MatchInfoSnapshot _match);
	public JoinMatchDelegate joinMatchCallback;
	private MatchInfoSnapshot match;
	[SerializeField]
	private Text matchNameText;
	public void Setup(MatchInfoSnapshot _match, JoinMatchDelegate _joinMatchCallback) {
		match = _match;
		joinMatchCallback = _joinMatchCallback;
		matchNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

	public void JoinMatch(){
		joinMatchCallback.Invoke(match);
	}
}
