using UnityEngine;
using UnityEngine.UI;

public class AlertSettingsListItem_Func : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if (AdminSettings.userListSelection == GetComponentInChildren<Text> ().text)
			GetComponent<Button> ().interactable = false;
		else
			GetComponent<Button> ().interactable = true;
	}
	public void UpdateAlertSettingsListSelection () {
		AdminSettings.userListSelection = GetComponentInChildren<Text> ().text;
	}
}
