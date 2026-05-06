using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FINetworking : MonoBehaviour {
	public static bool connecting;
	public GameObject plistInst;
	GameObject playerListTab;
	// Use this for initialization
	void Start () {
		playerListTab = GameObject.Find ("Player List Tab");
		PhotonNetwork.playerName = Account.Username;
		if (string.IsNullOrEmpty (PhotonNetwork.playerName)) {
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
				Account.Type = "Admin";
				PhotonNetwork.playerName = "Player_" + (PhotonNetwork.countOfPlayers + 1).ToString ();
			} else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
				SceneManager.LoadScene ("Menu");
		}
		Connect ();
	}

	void Update () {
	}

	void Connect () {
		PhotonNetwork.ConnectUsingSettings ("FI - Test");
		connecting = true;
	}
	void OnGUI () {
		//Not connected to the room
		/*if (!PhotonNetwork.connected && !connecting) {
			if (GUI.Button (new Rect (400, 500, 100, 25), "Single Player")) {
				connecting = true;
				Connect ();
			}
			if (GUI.Button (new Rect (510, 500, 108, 25), "Multiplayer")) {
				connecting = true;
				Connect ();
			}
		}*/
		//We are fully connected to the room
		/* if (PhotonNetwork.connected && !lConnecting) {
			if (PhotonNetwork.player.name == "Admin" || PhotonNetwork.player.name == "Moderator") {
				GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
				GUI.color = Color.green;
				GUI.Label (new Rect (15, 30, 5000, 20), PhotonNetwork.player.name);

			} else if (PhotonNetwork.player.name == "Fro" || PhotonNetwork.player.name == "Luper") {
				GUI.skin.label.fontStyle = FontStyle.Bold;
				GUI.color = new Color32 (124, 179, 234, 255);
				GUI.Label (new Rect (15, 30, 5000, 20), PhotonNetwork.player.name);

			} else {
				GUI.skin.label.fontStyle = FontStyle.Normal;
				GUI.color = Color.white;
				GUI.Label (new Rect (15, 30, 5000, 20), PhotonNetwork.player.name);
			}
			
			int i = 0;
			GUI.color = Color.white;
			
			foreach (PhotonPlayer ply in PhotonNetwork.otherPlayers) {
				if (ply.name == "Admin" || ply.name == "Moderator") {
					GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
					GUI.color = Color.green;
					GUI.Label (new Rect (15, 50 + (i * 20), 500, 20), ply.name);

				} else if (ply.name == "Fro" || ply.name == "Luper" || ply.name == "waleed") {
					GUI.skin.label.fontStyle = FontStyle.Bold;
					GUI.color = new Color32 (124, 179, 234, 255);
					GUI.Label (new Rect (15, 50 + (i * 20), 500, 20), ply.name);

				} else {
					GUI.skin.label.fontStyle = FontStyle.Normal;
					GUI.color = Color.white;
					GUI.Label (new Rect (15, 50 + (i * 20), 500, 20), ply.name);
				}

				i++;
			}
			GUI.color = Color.white;
			GUI.skin.label.fontStyle = FontStyle.Normal;
		} else if (!PhotonNetwork.connected && lConnecting)
			GUI.Label (new Rect (15, 30, 5000, 20), "<color=green><b>Connecting...</b></color>");
		else if (!PhotonNetwork.connected && !lConnecting && !GameObject.Find ("LoadingScreen").activeInHierarchy)
			GUI.Label (new Rect (15, 30, 5000, 20), "<color=red><b>Failed to connect</b></color>");*/

	}

	void OnConnectedToMaster () {
		PhotonNetwork.JoinRoom("Lounge");
	}

	void OnPhotonJoinRoomFailed () {
		PhotonNetwork.CreateRoom ("Lounge");
	}

	void OnJoinedRoom () {
		connecting = false;
		if (PhotonNetwork.room.Name == "Lounge") {
			GameObject.Find ("_LoungeManager").GetComponent<LoungeManager> ().StartCoroutine ("UpdateMyPlayerList");

			foreach (Transform child in playerListTab.transform.Find ("ScrollView/Mask/LayoutGroup"))
				Destroy (child.gameObject);
		
			PhotonPlayer[] otherPlayers = PhotonNetwork.otherPlayers;
			for (int i = 0; i < otherPlayers.Length; i++)
				StartCoroutine (PlayerJoinedRoom_Lounge (otherPlayers [i]));
		}
	}

	void OnDisconnectedFromPhoton () {
		GameObject.Find ("_PlayScreen/MyTeamSheetItem").GetComponent<MyLoungeTeam> ().LeaveTeam ();
	}

	void OnPhotonPlayerConnected (PhotonPlayer p) {
		if (PhotonNetwork.room.Name == "Lounge") {
			StartCoroutine (PlayerJoinedRoom_Lounge (p));
		}
	}
	void OnPhotonPlayerDisconnected (PhotonPlayer p) {
		if (PhotonNetwork.room.Name == "Lounge") {
			PlayerLeftRoom_Lounge (p);
		}
	}

	IEnumerator PlayerJoinedRoom_Lounge (PhotonPlayer p) {
		PlayerLeftRoom_Lounge (p);

			ServerDBConnection dbCon = GameObject.Find ("_GlobalSettings").GetComponent<ServerDBConnection> ();

			GameObject newItem = Instantiate (plistInst, Vector3.zero, Quaternion.identity, playerListTab.transform.Find ("ScrollView/Mask/LayoutGroup"));
			newItem.name = p.NickName;
			newItem.transform.Find ("ID").GetComponent<Text> ().text = p.ID.ToString();
			newItem.transform.Find ("Name").GetComponent<Text> ().text = p.NickName;
			string lvl = "";
			yield return dbCon.StartCoroutine (dbCon.DisplayLevel (result => lvl = result, p.NickName));
			newItem.transform.Find ("Level").GetComponent<Text> ().text = lvl;
		if (int.Parse (lvl) >= 75)
			newItem.transform.Find ("StarImg").gameObject.SetActive (true);
	}
	void PlayerLeftRoom_Lounge (PhotonPlayer p) {
		Destroy (GameObject.Find (playerListTab.name + "/ScrollView/Mask/LayoutGroup/" + p.NickName));
	}

	[PunRPC]
	void LoungeAlertBox_RPC (string sender, string type, string msg) {
		GameObject alertBox = GameObject.Find ("Canvas").transform.Find ("AlertBox").gameObject;
		LoungeAlertBox.alertType = type;
		if (type == "Admin") {
			alertBox.transform.Find ("Title").GetComponent<Text> ().text = "<i>An admin has sent you an alert</i>";
			alertBox.transform.Find ("ContentArea/Content").GetComponent<Text> ().text = msg;

			alertBox.transform.Find ("Background/Accept button").gameObject.SetActive (false);
			alertBox.transform.Find ("Background/Reject button").gameObject.SetActive (false);
			alertBox.transform.Find ("Close button").gameObject.SetActive (true);
		} else if (type == "Team Invite") {
			alertBox.transform.Find ("Title").GetComponent<Text> ().text = "<i>" + sender + " has sent you a pickup match invite</i>";
			alertBox.transform.Find ("ContentArea/Content").GetComponent<Text> ().text = string.Format ("Would you like to join {0}'s team for a pickup match?", sender);

			alertBox.transform.Find ("Close button").gameObject.SetActive (false);
			alertBox.transform.Find ("Background/Accept button").gameObject.SetActive (true);
			alertBox.transform.Find ("Background/Reject button").gameObject.SetActive (true);
			LoungeAlertBox.alertType = "Team Invite";
			LoungeAlertBox.receiver = PhotonNetwork.playerName;
			LoungeAlertBox.sender = sender;
		} else if (type == "Notice") {
			alertBox.transform.Find ("Title").GetComponent<Text> ().text = "<i>Notice</i>";
			alertBox.transform.Find ("ContentArea/Content").GetComponent<Text> ().text = msg;

			alertBox.transform.Find ("Background/Accept button").gameObject.SetActive (false);
			alertBox.transform.Find ("Background/Reject button").gameObject.SetActive (false);
			alertBox.transform.Find ("Close button").gameObject.SetActive (true);
		}
		Vector2 temp = alertBox.transform.Find ("Background").GetComponent<RectTransform> ().sizeDelta;
		if (alertBox.transform.Find ("Background/Accept button").gameObject.activeInHierarchy || alertBox.transform.Find ("Background/Accept button").gameObject.activeInHierarchy)
			temp.y = alertBox.transform.Find ("ContentArea/Content").GetComponent<RectTransform> ().sizeDelta.y - 345;
		else
			temp.y = alertBox.transform.Find ("ContentArea/Content").GetComponent<RectTransform> ().sizeDelta.y - 435;
		alertBox.transform.Find ("Background").GetComponent<RectTransform> ().sizeDelta = temp;
		alertBox.SetActive (true);
	}
}
