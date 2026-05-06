using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class MyLoungeTeam : Photon.MonoBehaviour {
	public Hashtable myTeamData = new Hashtable ();
	List<PhotonPlayer> myTeamPlayers = new List<PhotonPlayer>();
	public List<PhotonPlayer> currentPlayersInvited = new List<PhotonPlayer>();

	public GameObject teamSheetItem;
	// Use this for initialization
	void Start () {
		myTeamPlayers.Add (PhotonNetwork.player);
		transform.Find ("Name Text").GetComponent<Text> ().text = PhotonNetwork.playerName;
	}
	
	// Update is called once per frame
	void Update () {
		PhotonNetwork.player.CustomProperties ["Team Size"] = myTeamPlayers.Count;
	}

	public void JoinTeam (string teamName) {
		if (PhotonNetwork.player.customProperties.ContainsKey ("Team Captain"))
			GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Captain");
		if (PhotonNetwork.player.customProperties.ContainsKey ("Team Name"))
			GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Name");
		if (PhotonNetwork.player.customProperties.ContainsKey ("Team Position"))
			GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Position");
		if (PhotonNetwork.player.customProperties.ContainsKey ("Team IAmReady"))
			GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team IAmReady");
		myTeamData.Add ("Team IAmReady", "False");
		UpdateReadyStatus ();
		myTeamData.Add ("Team Name", teamName);
		List<string> tempAvailPos = new List<string> (GetComponent<LoungeTeamSheet> ().availablePositions);
		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if (p.CustomProperties.ContainsKey ("Team Name") && (string)p.CustomProperties ["Team Name"] == teamName) {
				PhotonNetwork.player.CustomProperties["Team Captain"] = (string)p.CustomProperties ["Team Captain"];
				tempAvailPos.Remove ((string)p.customProperties ["Team Position"]);
			}
		}
		int i = Random.Range (0, tempAvailPos.Count - 1);
		myTeamData.Add ("Team Position", tempAvailPos [i]);
		PhotonNetwork.player.SetCustomProperties (myTeamData);
		transform.position = PhotonView.Find (System.Convert.ToInt32 ((string)PhotonNetwork.player.CustomProperties ["Team Position"])).transform.position;
		GetComponent<LoungeTeamSheet> ().currentPos = (string)PhotonNetwork.player.CustomProperties ["Team Position"];

		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			if (p.CustomProperties.ContainsKey ("Team Name") && (string)p.customProperties ["Team Name"] == teamName)
				photonView.RPC ("JoinLoungeTeam_RPC", p, teamName);
		}
	}
	public void LeaveTeam () {
		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if (p.CustomProperties.ContainsKey ("Team Name") && (string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.CustomProperties ["Team Name"])
				Destroy (GameObject.Find ("Team Sheet Item: " + p.NickName));
		}
		myTeamData.Remove ("Team IAmReady");
		myTeamData.Add ("Team IAmReady", "False");
		foreach (PhotonPlayer ply in myTeamPlayers)
			photonView.RPC ("UpdateReadyStatus", ply);

		if ((string)PhotonNetwork.player.customProperties ["Team Captain"] == PhotonNetwork.playerName) {
			myTeamPlayers.Remove (PhotonNetwork.player);
			int i = Random.Range (0, myTeamPlayers.Count - 1);
			PhotonPlayer newCaptain = myTeamPlayers[i];
			foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
				if ((string)p.CustomProperties ["Team Name"] == (string)PhotonNetwork.player.CustomProperties ["Team Name"]) {
					p.CustomProperties ["Team Captain"] = newCaptain.NickName;
				}
			}
		} 
		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if ((string)p.CustomProperties ["Team Name"] == (string)PhotonNetwork.player.CustomProperties ["Team Name"]) {
				photonView.RPC ("LeaveLoungeTeam_RPC", p, PhotonNetwork.player);
			}
		}
		myTeamData.Remove ("Team Name");
		myTeamData.Add ("Team Name", PhotonNetwork.playerName + "'s Team");
		myTeamData.Remove ("Team Captain");
		myTeamData.Add ("Team Captain", PhotonNetwork.playerName);
		myTeamData.Remove ("Team Position");
		myTeamData.Add ("Team Position", GetComponent<LoungeTeamSheet>().currentPos);
		PhotonNetwork.player.SetCustomProperties (myTeamData);
		myTeamPlayers.Clear ();
		myTeamPlayers.Add (PhotonNetwork.player);
	}

	public void InvitePlayerToTeam (PhotonPlayer p) {
		if (myTeamPlayers.Count + currentPlayersInvited.Count < 4) {
			if (!currentPlayersInvited.Contains (p) && !myTeamPlayers.Contains (p)) {
			GameObject.FindWithTag ("Network Manager").GetPhotonView ().RPC ("LoungeAlertBox_RPC", p, PhotonNetwork.player.NickName, "Team Invite", null);
				currentPlayersInvited.Add (p);
			}
		} else {
			GameObject.FindWithTag ("Network Manager").GetPhotonView ().RPC ("LoungeAlertBox_RPC", PhotonNetwork.player, PhotonNetwork.player.NickName, "Notice", "You don't have enough space to invite this player!");
		}
	}

	[PunRPC]
	public void JoinLoungeTeam_RPC (string teamName) {
		if (myTeamPlayers.Count < 2)
			LoungeTeamManager.AddTeam (teamName);

		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if ((string)p.CustomProperties ["Team Name"] == teamName) {
				GameObject newItem = Instantiate (teamSheetItem, Vector3.zero, Quaternion.identity, transform.parent);
				newItem.name = "Team Sheet Item: " + p.NickName;
				newItem.transform.Find ("Name Text").GetComponent<Text> ().text = p.NickName;
				newItem.GetComponent<LoungeTeamSheet> ().currentPos = (string)p.customProperties["Team Position"];
				newItem.transform.position = PhotonView.Find (System.Convert.ToInt32 ((string)p.customProperties["Team Position"])).transform.position;
				if ((string)PhotonNetwork.player.customProperties ["Team Captain"] == PhotonNetwork.player.NickName)
					newItem.transform.Find ("Kick Button").gameObject.SetActive (true);
				photonView.RPC ("UpdateReadyStatus", p);
				myTeamPlayers.Add (p);
			}
		}
	}
	[PunRPC]
	public void LeaveLoungeTeam_RPC (PhotonPlayer p) {
		Destroy (GameObject.Find ("Team Sheet Item: " + p.NickName));
		myTeamPlayers.Remove (p);
		if ((string)PhotonNetwork.player.CustomProperties ["Team Captain"] == PhotonNetwork.playerName) {
			foreach (PhotonPlayer ply in PhotonNetwork.otherPlayers) {
				if ((string)ply.CustomProperties ["Team"] == (string)PhotonNetwork.player.CustomProperties ["Team"]) {
					GameObject.Find ("Team Sheet Item: " + ply.NickName).transform.Find ("Kick Button").gameObject.SetActive (true);
				}
			}
		}
		if (myTeamPlayers.Count < 2) {
			myTeamData.Remove ("Team Name");
			myTeamData.Add ("Team Name", PhotonNetwork.playerName + "'s Team");
			myTeamData.Remove ("Team Captain");
			myTeamData.Add ("Team Captain", PhotonNetwork.playerName);
			myTeamData.Remove ("Team Position");
			myTeamData.Add ("Team Position", GetComponent<LoungeTeamSheet>().currentPos);
			PhotonNetwork.player.SetCustomProperties (myTeamData);
		}
	}
	[PunRPC]
	public void UpdateReadyStatus () {
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
                GameObject pos = PhotonView.Find(System.Convert.ToInt32((string)p.customProperties["Team Position"])).gameObject;

                if (p.CustomProperties ["Team IAmReady"] == "True")
                    pos.GetComponent<Image>().color = new Color32(0, 255, 86, 255);
                //	GameObject.Find ((string)p.customProperties ["Team Position"]).GetComponent<Image> ().color = new Color32 (0, 255, 86, 255);
                else if (p.CustomProperties ["Team IAmReady"] == "False")
                    pos.GetComponent<Image>().color = Color.white;
                //GameObject.Find ((string)p.customProperties ["Team Position"]).GetComponent<Image> ().color = Color.white;
            }
		}
	}
}
