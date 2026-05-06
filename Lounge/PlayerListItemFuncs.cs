using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerListItemFuncs : MonoBehaviour, IPointerClickHandler {

	public void OnPointerClick(PointerEventData eventData) {
		PhotonPlayer thisPerson = PhotonPlayer.Find(System.Convert.ToInt32 (transform.parent.Find ("ID").GetComponent<Text>().text));
		if ((string)PhotonNetwork.player.CustomProperties ["Team Captain"] != PhotonNetwork.playerName || (int)thisPerson.CustomProperties["Team Size"] > 1)
			return;
		PhotonPlayer ply = null;
		foreach (PhotonPlayer p in PhotonNetwork.otherPlayers) {
			if (p.name == transform.parent.Find ("Name").GetComponent<Text> ().text)
				ply = p;
		}
		GameObject.Find ("_PlayScreen/MyTeamSheetItem").GetComponent<MyLoungeTeam> ().InvitePlayerToTeam (ply);
	}
}
