using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoungeAlertBox : MonoBehaviour {
	public static string alertType;
	public static string receiver;
	public static string sender;
	int space;

	public void CloseButton () {
		gameObject.SetActive (false);
	}
	public void AcceptButton () {
		if (alertType == "Team Invite") {
			GameObject.Find ("_PlayScreen/MyTeamSheetItem").GetComponent<MyLoungeTeam> ().JoinTeam (sender + "'s Team");
			int senderID = System.Convert.ToInt32 (GameObject.Find ("_MainScreen/Chat and Player List/Player List Tab/ScrollView/Mask/LayoutGroup/" + sender));
			GetComponent<PhotonView> ().RPC ("UpdateTeamInviteList", PhotonPlayer.Find (senderID), PhotonNetwork.player.ID);
		}
		CloseButton ();
	}
	public void RejectButton () {
		if (alertType == "Team Invite") {
			int senderID = System.Convert.ToInt32 (GameObject.Find ("_MainScreen/Chat and Player List/Player List Tab/ScrollView/Mask/LayoutGroup/" + sender));
			GetComponent<PhotonView> ().RPC ("UpdateTeamInviteList", PhotonPlayer.Find (senderID), PhotonNetwork.player.ID);
		}
		CloseButton ();
	}
	[PunRPC]
	void UpdateTeamInviteList (int playerIDToRemove) {
		GameObject.Find ("_PlayScreen/MyTeamSheetItem").GetComponent<MyLoungeTeam> ().currentPlayersInvited.Remove (PhotonPlayer.Find(playerIDToRemove));
	}
}
