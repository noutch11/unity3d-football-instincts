using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoungeManager : MonoBehaviour {
	public static int IC;
	public static int lvl;
	public static int xp;

	public static int viewer_lvl;
	public static string viewer_name;
	public static string viewer_club;

	Text plist_lvlText;
	Text plist_NameText;

	public GameObject xptext;
	public GameObject ictext;
	public GameObject lvltext_bottom;
	public GameObject lvltext_top;
	public GameObject nametext_bottom;
	public GameObject nametext_top;
	public GameObject clubnametext;
	public GameObject chatTab, playerListTab, myPlayerListObj, settingsScreen;

	public Button settingsButton;

	string currentTab;

	public static ServerDBConnection dbCon;

	// Use this for initialization
	void Start () {
		nametext_bottom.GetComponent<Text> ().text = PhotonNetwork.player.NickName;
		viewer_name = PhotonNetwork.player.NickName;
		dbCon = GameObject.FindObjectOfType<GlobalSettings>().GetComponent<ServerDBConnection> ();
		currentTab = "Player List";
		StartCoroutine (GetLevelAndName ());
	}
	
	// Update is called once per frame
	void Update () {
		if (currentTab == "Player List") {
			playerListTab.SetActive (true);
			chatTab.SetActive (false);
			GameObject.Find ("Chat and Player List/PlayerList Button").GetComponent<Button>().interactable = false;
			GameObject.Find ("Chat and Player List/Chat Button").GetComponent<Button>().interactable = true;
		} else if (currentTab == "Chat") {
			playerListTab.SetActive (false);
			chatTab.SetActive (true);
			GameObject.Find ("Chat and Player List/PlayerList Button").GetComponent<Button>().interactable = true;
			GameObject.Find ("Chat and Player List/Chat Button").GetComponent<Button>().interactable = false;
		}

	//	xptext.GetComponent<Text> ().text = xp.ToString();
	//	lvltext_bottom.GetComponent<Text> ().text = lvl.ToString();
	//	lvltext_top.GetComponent<Text> ().text = viewer_lvl.ToString();
	//	ictext.GetComponent<Text> ().text = IC.ToString();
	//	nametext_top.GetComponent<Text> ().text = viewer_name;
	//	clubnametext.GetComponent<Text> ().text = viewer_club;

	}
	public IEnumerator UpdateMyPlayerList () {
		string lvl = "";
		yield return dbCon.StartCoroutine (dbCon.DisplayLevel (result => lvl = result, PhotonNetwork.player.NickName));
		if (int.Parse (lvl) >= 75)
			myPlayerListObj.transform.Find ("StarImg").gameObject.SetActive (true);
	}
	IEnumerator GetLevelAndName () {
		myPlayerListObj.transform.Find ("Name").GetComponent<Text>().text = PhotonNetwork.playerName;
		string lvl = "";
		yield return dbCon.StartCoroutine (dbCon.DisplayLevel (result => lvl = result, PhotonNetwork.player.NickName));
		lvltext_bottom.GetComponent<Text> ().text = lvl;
		myPlayerListObj.transform.Find ("Level").GetComponent<Text> ().text = lvl;
	}
	public void ChatAndPlayerListButtonPress (string tab) {
		currentTab = tab;
		if (tab == "Chat")
			StartCoroutine (UpdateChatTab ());
	}
	IEnumerator UpdateChatTab () {
		yield return new WaitForEndOfFrame ();
		GameObject chatTxt = GameObject.Find ("Chat and Player List/ChatTab/Chat Contents/ScrollView/Mask/ChatText");
		if (chatTxt.GetComponent<RectTransform> ().rect.height > chatTxt.transform.parent.GetComponent<RectTransform> ().rect.height) {
			GameObject.Find ("Chat and Player List/ChatTab/Chat Contents/Scrollbar").GetComponent<Scrollbar> ().value = 0;
		}
	}

	public void SettingsButton () {
		settingsButton.interactable = false;
		settingsScreen.SetActive (true);
		settingsScreen.GetComponent<SettingsMenu> ().GetPrefs ();
	}
}
