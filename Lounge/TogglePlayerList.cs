using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TogglePlayerList : MonoBehaviour {
	Animation anim;
	bool open = false;
	bool closed = true;

	void Start () {
		anim = GetComponent<Animation>();
	}

	public void ToggleButton () {
		transform.Find ("Toggle Button").GetComponent<Button> ().interactable = false;
		if (closed) {
			anim.Play ("playerlist_open");
			open = true;
			closed = false;
		} else if (open) {
			anim.Play ("playerlist_close");
			open = false;
			closed = true;
		}
			
	}

	public void Interactable () {
		transform.Find ("Toggle Button").GetComponent<Button> ().interactable = true;
	}		
}
