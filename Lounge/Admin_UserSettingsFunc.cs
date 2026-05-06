using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Admin_UserSettingsFunc : MonoBehaviour, IPointerClickHandler {
	public AdminSettings a;
	void Awake () {
		a = GameObject.FindObjectOfType<AdminSettings> ();
	}
	public void OnPointerClick(PointerEventData eventData) {
		Transform p = transform.parent;
		string t = transform.Find ("Text").GetComponent<Text> ().text;
		if (t.Contains ("Ban") || t.Contains ("Unban")) {
			string s = "";
			if (t == "<color=#BF0000FF>Ban</color>")
				s = "B";
			else if (t == "<color=#185600FF>Unban</color>")
				s = "A";
			a.StartCoroutine (a.BanOrUnBan (p.Find ("PlayerName").GetComponent<Text> ().text, s));
		} else if (t.Contains ("Demote") || t.Contains ("Promote")) {
			string s = "";
			if (t == "<color=#BF0000FF>Demote</color>")
				s = "Regular Member";
			else if (t == "<color=#185600FF>Promote</color>")
				s = "Moderator";
			a.StartCoroutine (a.PromoteOrDemote (p.Find ("PlayerName").GetComponent<Text> ().text, s));
		}
	}
}
