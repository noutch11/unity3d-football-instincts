using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LoungeTeamSheet : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public Button readyButton;
	public Button leaveTeamButton;
	[HideInInspector]
	public bool ready = false;
	[HideInInspector]
	public string currentPos;
	string previousPos;
	public List<string> availablePositions = new List<string>();

	RectTransform draggableRect;
	RectTransform backgroundRect;
	// Use this for initialization
	void Start () {
		draggableRect = transform as RectTransform;
		backgroundRect = transform.parent as RectTransform;

		availablePositions.Add ("50");//ST
		availablePositions.Add ("51");//CB
		availablePositions.Add ("52");//LM
		availablePositions.Add ("53");//RM

		if (name == "MyTeamSheetItem") {
			transform.Find ("Name Text").GetComponent<Text> ().text = PhotonNetwork.playerName;

			foreach (string s in availablePositions) {
				//if (GameObject.Find (s).transform.position == this.transform.position) {
                if (PhotonView.Find (System.Convert.ToInt32 (s)).gameObject.transform.position == this.transform.position) {
					currentPos = s;
				}
			}

			if (PhotonNetwork.player.customProperties.ContainsKey ("Team Captain"))
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Captain");
			GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Captain", PhotonNetwork.playerName);
			if (PhotonNetwork.player.customProperties.ContainsKey ("Team Name"))
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Name");
			GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Name", PhotonNetwork.playerName + "'s Team");
			if (PhotonNetwork.player.customProperties.ContainsKey ("Team Position"))
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Position");
			GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Position", currentPos);
			if (PhotonNetwork.player.customProperties.ContainsKey ("Team Size"))
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Size");
			GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Size", 1);
			PhotonNetwork.player.SetCustomProperties (GetComponent<MyLoungeTeam> ().myTeamData);
		}
	}
	
	// Update is called once per frame
	void Update () {
		ClampPos ();
		if (name == "MyTeamSheetItem") {
			if ((int)PhotonNetwork.player.CustomProperties ["Team Size"] < 2)
				leaveTeamButton.gameObject.SetActive (false);
			else
				leaveTeamButton.gameObject.SetActive (true);
		}
	}

	public void ReadyOrUnready () {
		if (name != "MyTeamSheetItem")
			return;
		ready = !ready;
		if (PhotonNetwork.player.customProperties.ContainsKey ("Team IAmReady"))
			GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team IAmReady");
		if (ready) {
			readyButton.GetComponent<Image> ().color = Color.green;
			readyButton.GetComponentInChildren<Text> ().text = "Unready";
	//		GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team IAmReady", true);
	//		GameObject.Find (currentPos).GetComponent<Image> ().color = new Color32 (0, 255, 86, 255);
		} else {
			readyButton.GetComponent<Image> ().color = Color.red;
			readyButton.GetComponentInChildren<Text> ().text = "Ready";
	//		GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team IAmReady", false);
	//		GameObject.Find (currentPos).GetComponent<Image> ().color = Color.white;
		}
		GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team IAmReady", ready.ToString());
		PhotonNetwork.player.SetCustomProperties (GetComponent<MyLoungeTeam> ().myTeamData);
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			if (p.CustomProperties.ContainsKey ("Team Name") && (string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"])
				GetComponent<PhotonView>().RPC ("UpdateReadyStatus", p);
		}
	}
	public void KickFromTeamButton () {
		GameObject[] playerList = GameObject.FindGameObjectsWithTag ("PlayerListItem");
		string actualPhotonName = name.Substring (17);
		GameObject actualPhotonPlayer = null;
		foreach (GameObject g in playerList) {
			if (g.name == actualPhotonName)
				actualPhotonPlayer = g;
		}
		int actualPhotonID = System.Convert.ToInt32 (actualPhotonPlayer.transform.Find ("ID").GetComponent<Text> ().text);
		GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("GetKickedFromTeam", PhotonPlayer.Find (actualPhotonID));
	}
	public void LeaveTeamButton () {
		GetComponent<MyLoungeTeam> ().LeaveTeam ();
	}

	public void OnBeginDrag (PointerEventData eventData) {
		if ((string)PhotonNetwork.player.customProperties ["Team Captain"] != PhotonNetwork.playerName)
			return;

		previousPos = currentPos;
		GetComponent<RawImage> ().raycastTarget = false;
		transform.SetAsLastSibling ();
	}
	public void OnDrag (PointerEventData eventData) {
		if ((string)PhotonNetwork.player.customProperties ["Team Captain"] != PhotonNetwork.playerName)
			return;
		
		transform.position += (Vector3)eventData.delta;
	}
	public void OnEndDrag (PointerEventData eventData) {
		if ((string)PhotonNetwork.player.customProperties ["Team Captain"] != PhotonNetwork.playerName)
			return;

		if (eventData.pointerEnter == null)
			transform.position = PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position;
		else if (eventData.pointerEnter.GetComponent<PhotonView>() != null && availablePositions.Contains (eventData.pointerEnter.GetComponent<PhotonView>().viewID.ToString())) {
			if (name == "MyTeamSheetItem") {
				ready = true;
				ReadyOrUnready ();
				transform.position = eventData.pointerEnter.transform.position;
				currentPos = eventData.pointerEnter.GetComponent<PhotonView>().viewID.ToString();
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Position");
				GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Position", currentPos);
				PhotonNetwork.player.SetCustomProperties (GetComponent<MyLoungeTeam> ().myTeamData);
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
							GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "Team Sheet Item: " + PhotonNetwork.playerName, eventData.pointerEnter.transform.position);
					}
				}
			} else {
				transform.position = eventData.pointerEnter.transform.position;
				currentPos = eventData.pointerEnter.GetComponent<PhotonView>().viewID.ToString();

				GameObject[] playerList = GameObject.FindGameObjectsWithTag ("PlayerListItem");
				string actualPhotonName = name.Substring (17);
				GameObject actualPhotonPlayer = null;
				foreach (GameObject g in playerList) {
					if (g.name == actualPhotonName)
						actualPhotonPlayer = g;
				}
				int actualPhotonID = System.Convert.ToInt32 (actualPhotonPlayer.transform.Find ("ID").GetComponent<Text> ().text);
				PhotonPlayer ply = PhotonPlayer.Find (actualPhotonID);
				ply.CustomProperties ["Team Position"] = currentPos;
				GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("PositionChangeUnready", ply);
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
						if (p.NickName == actualPhotonName)
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "MyTeamSheetItem", eventData.pointerEnter.transform.position);
						else
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "Team Sheet Item: " + actualPhotonName, eventData.pointerEnter.transform.position);
					}
				}
			}
		} else if (eventData.pointerEnter.tag == "TeamSheetItem") {
			GameObject[] playerList = GameObject.FindGameObjectsWithTag ("PlayerListItem");
			string actualPhotonName = eventData.pointerEnter.name.Substring (17);
			GameObject actualPhotonPlayer = null;
			foreach (GameObject g in playerList) {
				if (g.name == actualPhotonName)
					actualPhotonPlayer = g;
			}
			int actualPhotonID = System.Convert.ToInt32 (actualPhotonPlayer.transform.Find ("ID").GetComponent<Text> ().text);
			PhotonPlayer ply = PhotonPlayer.Find (actualPhotonID);
			if (name == "MyTeamSheetItem") {
				ready = true;
				ReadyOrUnready ();
				GetComponent<PhotonView> ().RPC ("PositionChangeUnready", ply);

				transform.position = eventData.pointerEnter.transform.position;
				currentPos = eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos;
				GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Position");
				GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Postion", currentPos);
				PhotonNetwork.player.SetCustomProperties (GetComponent<MyLoungeTeam> ().myTeamData);
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
						GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "Team Sheet Item: " + PhotonNetwork.playerName, eventData.pointerEnter.transform.position);
					}
				}
				eventData.pointerEnter.transform.position = PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position;
				eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos = previousPos;
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
						if (p.NickName == actualPhotonName)
							GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "MyTeamSheetItem", PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
						else
							GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, eventData.pointerEnter.name, PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
					}
				}
			} else {
				string myPhotonName = name.Substring (17);
				GameObject myGameObject = null;
				foreach (GameObject g in playerList) {
					if (g.name == myPhotonName)
						myGameObject = g;
				}
				int myPhotonID = System.Convert.ToInt32 (myGameObject.transform.Find ("ID").GetComponent<Text> ().text);
				PhotonPlayer myPhotonPlayer = PhotonPlayer.Find (myPhotonID);
				GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("PositionChangeUnready", myPhotonPlayer);

				GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("PositionChangeUnready", ply);

				transform.position = eventData.pointerEnter.transform.position;
				currentPos = eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos;
				myPhotonPlayer.CustomProperties ["Team Position"] = currentPos;
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
						if (p.NickName == myPhotonName)
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "MyTeamSheetItem", PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
						else
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "Team Sheet Item: " + myPhotonPlayer.NickName, eventData.pointerEnter.transform.position);
					}
				}
				eventData.pointerEnter.transform.position = PhotonView.Find (System.Convert.ToInt32 (previousPos)).transform.position;
				eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos = previousPos;
				foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
					if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
						if (p.NickName == actualPhotonName)
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "MyTeamSheetItem", PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
						else
							GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, eventData.pointerEnter.name, PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
					}
				}
			}
			ply.CustomProperties ["Team Position"] = previousPos;
		} else if (eventData.pointerEnter.name == "MyTeamSheetItem") {
			GameObject[] plist = GameObject.FindGameObjectsWithTag ("PlayerListItem");
			string myPhotonName = name.Substring (17);
			GameObject myGameObject = null;
			foreach (GameObject g in plist) {
				if (g.name == myPhotonName)
					myGameObject = g;
			}
			int myPhotonID = System.Convert.ToInt32 (myGameObject.transform.Find ("ID").GetComponent<Text> ().text);
			PhotonPlayer myPhotonPlayer = PhotonPlayer.Find (myPhotonID);

			eventData.pointerEnter.GetComponent<LoungeTeamSheet>().ready = true;
			eventData.pointerEnter.GetComponent<LoungeTeamSheet>().ReadyOrUnready ();
			GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("PositionChangeUnready", myPhotonPlayer);

			transform.position = eventData.pointerEnter.transform.position;
			currentPos = eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos;
			myPhotonPlayer.CustomProperties ["Team Position"] = currentPos;
			foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
				if ((string)p.customProperties ["Team Name"] == (string)PhotonNetwork.player.customProperties ["Team Name"]) {
					GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("EditTeamSheet", p, "Team Sheet Item: " + myPhotonPlayer.NickName, eventData.pointerEnter.transform.position);
				}
			}
			eventData.pointerEnter.transform.position = PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position;
			eventData.pointerEnter.GetComponent<LoungeTeamSheet> ().currentPos = previousPos;
			eventData.pointerEnter.GetComponent<MyLoungeTeam> ().myTeamData.Remove ("Team Position");
			eventData.pointerEnter.GetComponent<MyLoungeTeam> ().myTeamData.Add ("Team Postion", previousPos);
			PhotonNetwork.player.SetCustomProperties (GetComponent<MyLoungeTeam> ().myTeamData);
			foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
                if ((string)p.customProperties["Team Name"] == (string)PhotonNetwork.player.customProperties["Team Name"]) {
                    GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView>().RPC("EditTeamSheet", p, "Team Sheet Item: " + PhotonNetwork.playerName, PhotonView.Find(System.Convert.ToInt32(previousPos)).transform.position);
				}
			}
			PhotonNetwork.player.CustomProperties ["Team Position"] = previousPos;
		} else
			transform.position = PhotonView.Find (System.Convert.ToInt32 (previousPos)).transform.position;

		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if ((string)p.CustomProperties ["Team Name"] == (string)PhotonNetwork.player.CustomProperties ["Team Name"])
				GameObject.Find("MyTeamSheetItem").GetComponent<PhotonView> ().RPC ("UpdateCurrentPosVarForOthers", p);
		}

		GetComponent<RawImage> ().raycastTarget = true;
	}

	[PunRPC]
	void EditTeamSheet (string player, Vector3 newPosition) {
		GameObject.Find (player).transform.position = newPosition;
	}
	[PunRPC]
	void UpdateCurrentPosVarForOthers() {
		LoungeTeamSheet[] l = GameObject.FindObjectsOfType<LoungeTeamSheet> ();
		foreach (LoungeTeamSheet lts in l) {
			if (lts.name == "MyTeamSheetItem") {
				if (lts.currentPos != (string)PhotonNetwork.player.CustomProperties ["Team Position"])
					lts.currentPos = (string)PhotonNetwork.player.CustomProperties ["Team Position"];
			} else {
				GameObject[] playerList = GameObject.FindGameObjectsWithTag ("PlayerListItem");
				string actualPhotonName = lts.name.Substring(17);
				GameObject actualGameObject = null;
				foreach (GameObject plisitem in playerList) {
					if (plisitem.name == actualPhotonName)
						actualGameObject = plisitem;
				}
				int actualPhotonID = System.Convert.ToInt32 (actualGameObject.transform.Find ("ID").GetComponent<Text> ().text);
				PhotonPlayer actualPhotonPlayer = PhotonPlayer.Find (actualPhotonID);
				if (lts.currentPos != (string)actualPhotonPlayer.CustomProperties ["Team Position"])
					lts.currentPos = (string)actualPhotonPlayer.CustomProperties ["Team Position"];
			}
		}
	}
	[PunRPC]
	void PositionChangeUnready () {
		if (name == "MyTeamSheetItem") {
			ready = true;
			ReadyOrUnready ();
		} else {
			GameObject.Find ("MyTeamSheetItem").GetComponent<LoungeTeamSheet> ().ready = true;
			GameObject.Find ("MyTeamSheetItem").GetComponent<LoungeTeamSheet> ().ReadyOrUnready ();
		}
	}
	[PunRPC]
	void GetKickedFromTeam () {
		GameObject.FindWithTag ("Network Manager").GetComponent<PhotonView> ().RPC ("LoungeAlertBox_RPC", PhotonNetwork.player, PhotonNetwork.player.NickName, "Notice", (string)PhotonNetwork.player.CustomProperties["Team Captain"] + " has kicked you from their team.");
		GameObject.Find ("MyTeamSheetItem").GetComponent<MyLoungeTeam> ().LeaveTeam ();
	}
	void ClampPos () {
		Vector3 pos = transform.localPosition;

		Vector3 min = backgroundRect.rect.min - draggableRect.rect.min;
		Vector3 max = backgroundRect.rect.max - draggableRect.rect.max;

		pos.x = Mathf.Clamp (draggableRect.localPosition.x, min.x, max.x);
		pos.y = Mathf.Clamp (draggableRect.localPosition.y, min.y, max.y);
		draggableRect.localPosition = pos;
	}
}
