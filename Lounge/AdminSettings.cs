using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminSettings : MonoBehaviour {
	Color activeColor;
	Color inactiveColor;
	string currentTab;
	string subTab;

	public static string userListSelection = "";

	string tickerTapeTxt;

	ServerDBConnection dbCon;
	public GameObject userSettingsListItem, alertSettingsListItem, moderatorTabButtons, adminTabButtons;
	public InputField alertSettInput;
	[Header("Main Buttons")]
	public Button mainSettBut; public Button userSettBut; public Button chatSettBut; public Button tickertapeBut; public Button alertsBut;
	[Header("Main Tabs")]
	public GameObject mainSettings; public GameObject userSettings; public GameObject chatSettings; public GameObject tickertapeSettings; public GameObject alertsSettings;
	[Header("Sub-Buttons")]
	public Button banUsersBut; public Button moderatorsManageBut; public Button sendAlertButton;
	[Header("Sub-Tabs")] public GameObject banUsers; public GameObject moderatorsManage;
	// Use this for initialization
	void Awake () {
		MainSettingsButton ();
		dbCon = GameObject.FindObjectOfType<ServerDBConnection> ();
		activeColor = new Color32 (50, 50, 50, 255);
		inactiveColor = new Color32 (50, 50, 50, 150);
	}
	
	// Update is called once per frame
	void Update () {
		#region handling tabs
		if (Account.Type == "Admin") {
			chatSettBut = adminTabButtons.transform.Find ("ChatSettings Button").GetComponent<Button>();
			mainSettBut = adminTabButtons.transform.Find ("MainSettings Button").GetComponent<Button>();
			if (!adminTabButtons.activeInHierarchy) {
				adminTabButtons.SetActive (true);
				moderatorTabButtons.SetActive(false);
			}
			if (currentTab == "Main") {
				mainSettings.SetActive (true);
				userSettings.SetActive(false);
				chatSettings.SetActive (false);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(false);
			} else if (currentTab == "User") {
				mainSettings.SetActive (false);
				userSettings.SetActive(true);
				chatSettings.SetActive (false);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(false);
				if (subTab == "Ban") {
				banUsers.SetActive (true);
				moderatorsManage.SetActive (false);
				} else if (subTab == "Moderators") {
				banUsers.SetActive (false);
				moderatorsManage.SetActive (true);
				}
			} else if (currentTab == "Chat") {
				mainSettings.SetActive (false);
				userSettings.SetActive(false);
				chatSettings.SetActive (true);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(false);
			} else if (currentTab == "Tickertape") {
				mainSettings.SetActive (false);
				userSettings.SetActive(false);
				chatSettings.SetActive (false);
				tickertapeSettings.SetActive (true);
				alertsSettings.SetActive(false);
			} else if (currentTab == "Alerts") {
				mainSettings.SetActive (false);
				userSettings.SetActive(false);
				chatSettings.SetActive (false);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(true);
			}
		} else if (Account.Type == "Moderator") {
			chatSettBut = moderatorTabButtons.transform.Find ("ChatSettings Button").GetComponent<Button>();
			mainSettBut = moderatorTabButtons.transform.Find ("MainSettings Button").GetComponent<Button>();
			if (!moderatorTabButtons.activeInHierarchy) {
				adminTabButtons.SetActive (false);
				moderatorTabButtons.SetActive(true);
			}
			if (currentTab == "Main") {
				mainSettings.SetActive (true);
				userSettings.SetActive(false);
				chatSettings.SetActive (false);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(false);
			}else if (currentTab == "Chat") {
				mainSettings.SetActive (false);
				userSettings.SetActive(false);
				chatSettings.SetActive (true);
				tickertapeSettings.SetActive (false);
				alertsSettings.SetActive(false);
			}
		} else {
			if (adminTabButtons.activeInHierarchy || moderatorTabButtons.activeInHierarchy) {
				adminTabButtons.SetActive (false);
				moderatorTabButtons.SetActive(false);
			}
			currentTab = "Main";
			mainSettings.SetActive (true);
			userSettings.SetActive(false);
			chatSettings.SetActive (false);
			tickertapeSettings.SetActive (false);
			alertsSettings.SetActive(false);
		}
		#endregion
		GetCurrentText ();
	}

	#region main buttons
	public void MainSettingsButton () {
		mainSettBut.interactable = false;
		userSettBut.interactable = true;
		chatSettBut.interactable = true;
		tickertapeBut.interactable = true;
		alertsBut.interactable = true;

		mainSettBut.GetComponentInChildren<Text> ().color = activeColor;
		userSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		chatSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		tickertapeBut.GetComponentInChildren<Text> ().color = inactiveColor;
		alertsBut.GetComponentInChildren<Text> ().color = inactiveColor;

		currentTab = "Main";
	}
	public void UserSettingsButton () {
		mainSettBut.interactable = true;
		userSettBut.interactable = false;
		chatSettBut.interactable = true;
		tickertapeBut.interactable = true;
		alertsBut.interactable = true;

		mainSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		userSettBut.GetComponentInChildren<Text> ().color = activeColor;
		chatSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		tickertapeBut.GetComponentInChildren<Text> ().color = inactiveColor;
		alertsBut.GetComponentInChildren<Text> ().color = inactiveColor;

		currentTab = "User";
		BanUsersButton ();
	}
	public void ChatSettingsButton () {
		mainSettBut.interactable = true;
		userSettBut.interactable = true;
		chatSettBut.interactable = false;
		tickertapeBut.interactable = true;
		alertsBut.interactable = true;

		mainSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		userSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		chatSettBut.GetComponentInChildren<Text> ().color = activeColor;
		tickertapeBut.GetComponentInChildren<Text> ().color = inactiveColor;
		alertsBut.GetComponentInChildren<Text> ().color = inactiveColor;

		currentTab = "Chat";
	}
	public void TickertapeButton () {
		mainSettBut.interactable = true;
		userSettBut.interactable = true;
		chatSettBut.interactable = true;
		tickertapeBut.interactable = false;
		alertsBut.interactable = true;

		mainSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		userSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		chatSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		tickertapeBut.GetComponentInChildren<Text> ().color = activeColor;
		alertsBut.GetComponentInChildren<Text> ().color = inactiveColor;

		currentTab = "Tickertape";
	}
	public void AlertsButton () {
		mainSettBut.interactable = true;
		userSettBut.interactable = true;
		chatSettBut.interactable = true;
		tickertapeBut.interactable = true;
		alertsBut.interactable = false;

		mainSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		userSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		chatSettBut.GetComponentInChildren<Text> ().color = inactiveColor;
		tickertapeBut.GetComponentInChildren<Text> ().color = inactiveColor;
		alertsBut.GetComponentInChildren<Text> ().color = activeColor;

		currentTab = "Alerts";
		GetUserListForAlerts();
	}
	#endregion
	#region sub buttons
	public void BanUsersButton () {
		banUsersBut.interactable = false;
		moderatorsManageBut.interactable = true;
		banUsersBut.GetComponentInChildren<Text> ().color = activeColor;
		moderatorsManageBut.GetComponentInChildren<Text> ().color = inactiveColor;

		subTab = "Ban";
		StartCoroutine (GetBanList ());
	}
	public void ModeratorsButton () {
		banUsersBut.interactable = true;
		moderatorsManageBut.interactable = false;
		banUsersBut.GetComponentInChildren<Text> ().color = inactiveColor;
		moderatorsManageBut.GetComponentInChildren<Text> ().color = activeColor;

		subTab = "Moderators";
		StartCoroutine (GetModeratorList ());
	}
	#endregion

	#region tickertape settings
	public void UpdateTickerTape () {
		GetComponent<PhotonView> ().RPC ("UpdateTickerTape_RPC", PhotonTargets.AllBuffered, tickerTapeTxt.ToUpperInvariant ());
		tickertapeSettings.GetComponentInChildren<InputField> ().text = "";
	}
	public void SetNewTickerTapeText (string text) {
		tickerTapeTxt = text;
	}
	void GetCurrentText () {
		if (string.IsNullOrEmpty (tickertapeSettings.GetComponentInChildren<InputField> ().text))
			tickertapeSettings.GetComponentInChildren<Button> ().interactable = false;
		else
			tickertapeSettings.GetComponentInChildren<Button> ().interactable = true;
		tickertapeSettings.transform.Find ("CurrentNews").GetComponent<Text> ().text = "<b>Current News: </b>" + GameObject.Find ("Ticker Tape/Mask/Text").GetComponent<Text> ().text;
	}
	[PunRPC]
	void UpdateTickerTape_RPC (string txt) {
		GameObject.Find ("_MainScreen/Ticker Tape/Mask/Text").GetComponent<Text> ().text = txt;
	} 
	#endregion
	#region user settings
	IEnumerator GetBanList () {
		Transform p = transform.Find ("UserSettings/BanUsers Screen/Mask/LayoutGroup");
		List<GameObject> children = new List<GameObject> ();
		foreach (Transform c in p)
			children.Add (c.gameObject);
		children.ForEach (c => Destroy (c));
		p.parent.Find ("Loading...").gameObject.SetActive (true);
		string res = "";
		yield return dbCon.BanList (result => res = result);
		string[] splits = res.Split ('|');
		foreach (string s in splits) {
			if (!string.IsNullOrEmpty (s)) {
				p.parent.Find ("Loading...").gameObject.SetActive (false);
				string[] secondSplit = s.Split (' ');
				string name = secondSplit [0];
				string status = secondSplit [1];
				if (name != PhotonNetwork.player.NickName) {
					GameObject newItem = Instantiate (userSettingsListItem, Vector3.zero, Quaternion.identity, p);

						newItem.transform.Find ("PlayerName").GetComponent<Text> ().text = name;
						if (status == "A")
							newItem.transform.Find ("Button/Text").GetComponent<Text> ().text = "<color=#BF0000FF>Ban</color>";
						else if (status == "B")
							newItem.transform.Find ("Button/Text").GetComponent<Text> ().text = "<color=#185600FF>Unban</color>";
				}
			}
		}
	}
	public IEnumerator BanOrUnBan(string name, string status) {
		yield return dbCon.UpdateBanStatus (name, status);
		StartCoroutine (GetBanList ());
	}
	IEnumerator GetModeratorList () {
		Transform p = transform.Find ("UserSettings/ModeratorsManage Screen/Mask/LayoutGroup");
		List<GameObject> children = new List<GameObject> ();
		foreach (Transform c in p)
			children.Add (c.gameObject);
		children.ForEach (c => Destroy (c));
		p.parent.Find ("Loading...").gameObject.SetActive (true);
		string res = "";
		yield return dbCon.ModeratorList (result => res = result);
		string[] splits = res.Split ('|');
		foreach (string s in splits) {
			if (!string.IsNullOrEmpty (s)) {
				p.parent.Find ("Loading...").gameObject.SetActive (false);
				GameObject newItem = Instantiate (userSettingsListItem, Vector3.zero, Quaternion.identity, p);
				string[] secondSplit = s.Split (' ');
				string name = secondSplit [0];
				string type = secondSplit [1];
				newItem.transform.Find ("PlayerName").GetComponent<Text> ().text = name;
				if (type == "Moderator")
						newItem.transform.Find ("Button/Text").GetComponent<Text> ().text = "<color=#BF0000FF>Demote</color>";
				else if (type == "Regular")
						newItem.transform.Find ("Button/Text").GetComponent<Text> ().text = "<color=#185600FF>Promote</color>";
			}
		}
	}
	public IEnumerator PromoteOrDemote(string name, string status) {
		yield return dbCon.UpdateModerators (name, status);
		StartCoroutine (GetModeratorList ());
	}
	#endregion
	#region alert settings
	void GetUserListForAlerts () {
		Transform p = transform.Find ("AlertSettings/ScrollView/Mask/LayoutGroup");
		List<GameObject> children = new List<GameObject> ();
		foreach (Transform c in p) {
			if (c.name != "DefaultOption")
				children.Add (c.gameObject);
		}
		children.ForEach (c => Destroy (c));
		foreach (PhotonPlayer ply in PhotonNetwork.otherPlayers) {
			GameObject newItem = Instantiate (alertSettingsListItem, Vector3.zero, Quaternion.identity, p);
			newItem.transform.Find ("Text").GetComponent<Text> ().text = ply.NickName;
		}
	}
	public void SendAlertButton () {
		if (userListSelection == "Send an alert to all players") {
			GameObject.Find ("_NetworkManager").GetComponent<PhotonView> ().RPC ("LoungeAlertBox_RPC", PhotonTargets.Others, PhotonNetwork.player.NickName, Account.Type, alertSettInput.text);
			alertSettInput.text = "";
			StartCoroutine (UpdatePlaceholder ("Alert sent to all players"));
			ToggleSendAlertButton ();
		} else if (!string.IsNullOrEmpty (userListSelection)) {
			Transform t = GameObject.Find ("_MainScreen").transform.Find ("Chat and Player List/Player List Tab/ScrollView/Mask/LayoutGroup");
			GameObject.Find ("_NetworkManager").GetComponent<PhotonView> ().RPC ("LoungeAlertBox_RPC", PhotonPlayer.Find (int.Parse (t.Find (userListSelection).Find ("ID").GetComponent<Text> ().text)), PhotonNetwork.player.NickName, Account.Type, alertSettInput.text);
			alertSettInput.text = "";
			StartCoroutine (UpdatePlaceholder ("Alert sent to: " + userListSelection));
			ToggleSendAlertButton ();
		} else {
			alertSettInput.text = "";
			StartCoroutine (UpdatePlaceholder ("Please select someone to receive the alert"));
			sendAlertButton.interactable = false;
		}
	}
	public void ToggleSendAlertButton () {
		if (!string.IsNullOrEmpty (alertSettInput.text))
			sendAlertButton.interactable = true;
		else
			sendAlertButton.interactable = false;
	}
	IEnumerator UpdatePlaceholder (string t) {
		alertSettInput.placeholder.GetComponent<Text> ().text = t;
		yield return new WaitForSeconds (2.5f);
		alertSettInput.placeholder.GetComponent<Text> ().text = "Enter text...";
	}
	#endregion
}
