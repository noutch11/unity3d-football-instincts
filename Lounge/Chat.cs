using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Chat : MonoBehaviour {
	public GameObject chatField, chatText, sendButton, scrollBar;
	public float waitTime;
	bool canChat = true;
	List<string> chats = new List<string> ();
	//<summary>
	//array of words that will be censored in the chat
	//</summary>
	string[] censoredWords = new string[]{"fuck", "cunt", "cock", "dick", "nigger", "nigga", "faggot", "motherfucker", "fucker", "shit", "fag", "pussy", "boob", "tit", "ass", "porn", "bitch"};

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (FINetworking.connecting) {
			chatField.GetComponent<InputField> ().interactable = false;
			chatText.GetComponent<Text> ().text = "<i>Connecting...</i>";
		}
		if (string.IsNullOrEmpty (chatField.GetComponent<InputField> ().text))
			sendButton.GetComponent<Button> ().interactable = false;
		else
			sendButton.GetComponent<Button> ().interactable = true;
		
		if (Input.GetKeyDown ("return")) {
			SendChat ();
		}
			
	}

	public void SendChat () {
		if (!string.IsNullOrEmpty (chatField.GetComponent<InputField> ().text) && canChat) {
			if (Account.Type == "Admin" || Account.Type == "Moderator") {
				if (chatField.GetComponent<InputField> ().text == "!clear") {
					AddChat ("*Admin cleared chat*", "Clear");
					chatField.GetComponent<InputField> ().text = "";
				} else {
					AddChat ("<b>" + PhotonNetwork.player.NickName + "</b>: " + chatField.GetComponent<InputField> ().text, null);
					chatField.GetComponent<InputField> ().text = "";
					canChat = false;
					StartCoroutine (ChatWaitTime ());
				}
					
			} else {
				AddChat ("<b>" + PhotonNetwork.player.NickName + "</b>: " + chatField.GetComponent<InputField> ().text, null);
				chatField.GetComponent<InputField> ().text = "";
				canChat = false;
				StartCoroutine (ChatWaitTime ());
			}
		} else if (!string.IsNullOrEmpty (chatField.GetComponent<InputField> ().text) && !canChat)
				AddChat ("<color=#FFFF9BFF><size=27><i><b>Admin: </b>Please wait " + waitTime + " seconds between chat messages.</i></size></color>", "WaitTime");
	}


	public void AddChat (string msg, string cmd) {
		foreach (string s in censoredWords) {
			if (msg.ToLowerInvariant ().Contains (s)) {
				string followingChars = msg.ToLowerInvariant().Substring (msg.ToLowerInvariant().LastIndexOf (s [0]) + s.Length);
				string precedingChars = msg.ToLowerInvariant().Substring (PhotonNetwork.player.NickName.Length + 8, msg.ToLowerInvariant().LastIndexOf (s [0]) - (PhotonNetwork.player.NickName.Length + 8));
				if (followingChars.Length > 0) {
						if (precedingChars [precedingChars.Length - 1] == ' ' || followingChars [0] == ' ' || followingChars == s || System.Char.IsDigit(followingChars[0]) || System.Char.IsNumber(followingChars[0]) || System.Char.IsPunctuation(followingChars[0]) || System.Char.IsDigit(precedingChars[precedingChars.Length - 1]) || System.Char.IsNumber(precedingChars[precedingChars.Length - 1]) || System.Char.IsPunctuation(precedingChars[precedingChars.Length - 1])) {
							string charReplacement = new string ('*', s.Length);
							string censoredMsg = Regex.Replace (msg, s, charReplacement, RegexOptions.IgnoreCase);
							msg = censoredMsg;
						}
				} else if (followingChars.Length == 0) {
						string charReplacement = new string ('*', s.Length);
						string censoredMsg = Regex.Replace (msg, s, charReplacement, RegexOptions.IgnoreCase);
						msg = censoredMsg;
				}
			}
		}
		if (cmd == "WaitTime") {
			chats.Add (msg);
			if (chats.Count > 1)
				chatText.GetComponent<Text> ().text += "\n\n" + msg;
			else
				chatText.GetComponent<Text> ().text += msg;

			Canvas.ForceUpdateCanvases ();
			StartCoroutine (UpdateScrollbar (false));
		} else
			GetComponent<PhotonView> ().RPC ("AddLobbyChat_RPC", PhotonTargets.AllViaServer, msg, cmd);
	}
	[PunRPC]
	void AddLobbyChat_RPC (string msg, string adminCmd) {
		if (adminCmd == "Clear") {
			chats.Clear ();
			chats.Add (msg);
			chatText.GetComponent<Text> ().text = "<b><i><color=green>The chat has been cleared by a moderator</color></i></b>";
			Canvas.ForceUpdateCanvases ();
			StartCoroutine (UpdateScrollbar (true));
		} else if (string.IsNullOrEmpty (adminCmd)) {
			chats.Add (msg);
			if (chats.Count > 1)
				chatText.GetComponent<Text> ().text += "\n\n" + msg;
			else
				chatText.GetComponent<Text> ().text += msg;
					
			Canvas.ForceUpdateCanvases ();
			StartCoroutine (UpdateScrollbar (false));
		}
	}
	IEnumerator UpdateScrollbar(bool chatIsEmpty) {
		yield return new WaitForEndOfFrame ();
		if (!chatIsEmpty)
			scrollBar.GetComponent<Scrollbar> ().value = 0f;
		else {
			scrollBar.GetComponent<Scrollbar> ().size = 1f;
			scrollBar.GetComponent<Scrollbar> ().value = 1f;
		}
	}
	IEnumerator ChatWaitTime () {
		yield return new WaitForSeconds (waitTime);
		canChat = true;
	}

	void OnJoinedRoom () {
		chatField.GetComponent<InputField> ().interactable = true;
		string msg = "<color=#FFFF9BFF><size=27>Welcome, " + PhotonNetwork.player.NickName + ". You have joined the chat.</size></color>";
		chatText.GetComponent<Text> ().text = msg;
		chats.Add (msg);
	}
}
