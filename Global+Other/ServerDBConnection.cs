using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class ServerDBConnection : MonoBehaviour {
	string wwwSite = "http://fi-domaindontsee.000webhostapp.com/";
	void Start () {
	}

	#region coins_dbstuff
	/*

	public IEnumerator GetCoins (Action<string> result) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", Account.Username);
		WWW ShowCoinsWWW = new WWW ("wwwSite + "displaycoins.php", form);
		//Wait for php to send something back to Unity
		yield return ShowCoinsWWW;
		if (ShowCoinsWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + ShowCoinsWWW.error);
			yield return 0;
		} else {
			result = ShowCoinsWWW.text;
			yield return int.Parse (result);
		}
	}

	public IEnumerator AddCoins (string name, int coins) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", name);
		form.AddField ("Coins", coins);
		WWW AddCoinsWWW = new WWW ("wwwSite + "addcoins.php", form);
		//Wait for php to send something back to Unity
		yield return AddCoinsWWW;
		if (AddCoinsWWW.error != null) {
			Debug.LogError ("Error while trying to display coins: " + AddCoinsWWW.error);
		} else {
			yield return null;
		}
	}*/
	#endregion

	#region lvl_dbstuff

	public IEnumerator DisplayLevel (Action<string> result, string u) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", u);
		WWW SomeWWW = new WWW (wwwSite + "displaylevel.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
		} else {
			string DisplayReturn;
			if (SomeWWW.text != "0 results")
				DisplayReturn = SomeWWW.text;
			else
				DisplayReturn = "0";
			result (DisplayReturn);
			yield return result;
		}
	}
				

/*	public IEnumerator AddLevel () {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", Account.Username);
		form.AddField ("Level", LobbyGUI.lvl);
		WWW SomeWWW = new WWW (wwwSite + "addlevel.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to display coins: " + SomeWWW.error);
		} else {
			GetLevel ();
		}
	} */
	#endregion

	#region xp_dbstuff
/*
	public IEnumerator DisplayXP () {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", Account.Username);
		WWW SomeWWW = new WWW ("wwwSite + "displayxp.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
		} else {
			string DisplayReturn = SomeWWW.text;
			LobbyManager.xp = int.Parse(DisplayReturn);
		}
	}

	public IEnumerator AddXP (string name, int xpAmount) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", name);
		form.AddField ("XP", xpAmount);
		WWW SomeWWW = new WWW ("wwwSite + "addxp.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to display coins: " + SomeWWW.error);
		} else {
			yield return null;
		}
	} */
	#endregion

	#region lvlupprice_dbstuff
/*	public void GetLvlUpPrice()
	{

		StartCoroutine (DisplayLvlUpPrice ());

	}

	public void PostLvlUpPrice()
	{

		StartCoroutine (AddLvlUpPrice ());

	}

	IEnumerator DisplayLvlUpPrice () {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", Account.Username);
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Accounts/displaylvlupprice.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
		} else {
			string DisplayReturn = SomeWWW.text;
			LobbyGUI.lvlUpPrice = int.Parse(DisplayReturn);
		}
	}

	IEnumerator AddLvlUpPrice () {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", Account.Username);
		form.AddField ("LevelUpPrice", LobbyGUI.lvlUpPrice);
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Accounts/addlvlupprice.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to display coins: " + SomeWWW.error);
		} else {
			GetLvlUpPrice ();
		}
	} */
	#endregion

	#region leaderboard_lvl
	public void GetLeaderboard_Level()
	{

		StartCoroutine (Leaderboard_Level ());

	}

	IEnumerator Leaderboard_Level () {
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Leaderboard/leaderboard-level.php");
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
		} else {
			string DisplayReturn = SomeWWW.text;
			//GameObject.Find ("Leaderboard").GetComponent<Text> ().text = DisplayReturn;
		}
	}
	#endregion

	#region clubs_search
/*	public void SearchClub(string searchTxt)
	{

		StartCoroutine (SearchClub_IE (searchTxt));

	}

	IEnumerator SearchClub_IE (string searchText) {
		WWWForm form = new WWWForm ();
		form.AddField ("ClubName", searchText);
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Clubs/clubs-searchname.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			LobbyGUI.clubTabTxt = DisplayReturn;
		} else {
			string DisplayReturn = SomeWWW.text;
			LobbyGUI.clubTabTxt = DisplayReturn;
		}
	} */
	#endregion
	#region clubs_create
/*	public void CreateClub (string name, string tag)
	{

		StartCoroutine (CreateClub_IE (name, tag));

	}

	IEnumerator CreateClub_IE (string name, string tag) {
		WWWForm form = new WWWForm ();
		form.AddField ("ClubName", name);
		form.AddField ("Abbreviation", tag);
		form.AddField ("Owner", Account.Username);
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Clubs/clubs-create.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			LobbyGUI.clubTabTxt = DisplayReturn;
		} else {
			string DisplayReturn = SomeWWW.text;
			LobbyGUI.clubTabTxt = DisplayReturn;
		}
	} */
	#endregion
	#region clubs_join
/*	public void JoinClub(string name, string tag)
	{

		StartCoroutine (JoinClub_IE (name, tag));

	}

	IEnumerator JoinClub_IE (string name, string tag) {
		WWWForm form = new WWWForm ();
		form.AddField ("ClubName", name);
		form.AddField ("Member", Account.Username);
		WWW SomeWWW = new WWW ("http://solarwatergames.cf/Clubs/clubs-join.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			LobbyGUI.clubTabTxt = DisplayReturn;
		} else {
			string DisplayReturn = SomeWWW.text;
			LobbyGUI.clubTabTxt = DisplayReturn;
		}
	} */
	#endregion
	#region clubs_get
	public IEnumerator GetClub(string name, Action<string> result) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", name);
		WWW SomeWWW = new WWW (wwwSite + "displayclub.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			result ("*Free agent*");
			yield return result;
		} else {
			if (SomeWWW.text == "None" || SomeWWW.text == "0 results")
				result ("*Free Agent*");
			else
				result (SomeWWW.text);
			
				yield return result;
		}
	}
	#endregion

	#region get_username

	public IEnumerator GetUsername (string email) {
		WWWForm form = new WWWForm ();
		form.AddField ("Email", email);
		WWW LogintoAccountWWW = new WWW (wwwSite + "getusername.php", form);
		//Wait for php to send something back to Unity
		yield return LogintoAccountWWW;
		if (LogintoAccountWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + LogintoAccountWWW.error);
			SceneManager.LoadScene ("Menu");
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			GameObject.Find ("_AccountManager").GetComponent<Account>().alertBox.GetComponentInChildren<Text> ().text = DisplayReturn;
		} else {
			string LoginReturn = LogintoAccountWWW.text;
			if (LoginReturn != "0 results") {
				Account.Username = LoginReturn;
				string type = "";
				yield return StartCoroutine (GetMemberType (email, result=>type=result));
				Account.Type = type;

			} else
				SceneManager.LoadScene ("Menu");
		}
	}
	#endregion

	#region get_type
	public IEnumerator GetMemberType (string email, Action<string> result) {
		WWWForm form = new WWWForm ();
		form.AddField ("Email", email);
		WWW GetTypeWWW = new WWW (wwwSite + "gettype.php", form);
		//Wait for php to send something back to Unity
		yield return GetTypeWWW;
		if (GetTypeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + GetTypeWWW.error);
			SceneManager.LoadScene ("Menu");
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			GameObject.Find ("_AccountManager").GetComponent<Account> ().alertBox.SetActive (true);
			GameObject.Find ("_AccountManager").GetComponent<Account>().alertBox.GetComponentInChildren<Text> ().text = DisplayReturn;
		} else {
			if (GetTypeWWW.text != "0 results") {
				result (GetTypeWWW.text);
				yield return result;
			} else
				SceneManager.LoadScene ("Menu");
		}
	}
	#endregion

	#region password_change
	public void ChangePass(string email)
	{

		StartCoroutine (ChangePass_IE (email));

	}

	IEnumerator ChangePass_IE (string email) {
		WWWForm form = new WWWForm ();
		form.AddField ("Email", email);
		WWW SomeWWW = new WWW (wwwSite + "password_recovery.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
			string DisplayReturn = "<color=red>Unkown server error. Please try again later</color>";
			//GameObject.Find ("AlertBox").GetComponentInChildren<Text> ().text =DisplayReturn;
		} else {
			string DisplayReturn = SomeWWW.text;
			//LobbyGUI.clubTabTxt = DisplayReturn;
		}
	}
	#endregion

	#region admin panel
	public IEnumerator BanList (Action<String> result) {
		WWW SomeWWW = new WWW (wwwSite + "banlist.php");
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
		} else {
			string DisplayReturn = SomeWWW.text;
			result (DisplayReturn);
			yield return result;
		}
	}
	public IEnumerator UpdateBanStatus (string name, string status) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", name);
		form.AddField ("Status", status);
		WWW SomeWWW = new WWW (wwwSite + "updatebanstatus.php", form);
			//Wait for php to send something back to Unity
			yield return SomeWWW;
			if (SomeWWW.error != null) {
				Debug.LogError ("Error while trying to update ban status: " + SomeWWW.error);
			} else {
			yield return null;
			}
	}
	public IEnumerator ModeratorList (Action<string> result) {
		WWW SomeWWW = new WWW (wwwSite + "moderatorlist.php");
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to connect to server: " + SomeWWW.error);
		} else {
			string DisplayReturn = SomeWWW.text;
			result (DisplayReturn);
			yield return result;
		}
	}
	public IEnumerator UpdateModerators (string name, string type) {
		WWWForm form = new WWWForm ();
		form.AddField ("Username", name);
		form.AddField ("Type", type);
		WWW SomeWWW = new WWW (wwwSite + "updatemoderators.php", form);
		//Wait for php to send something back to Unity
		yield return SomeWWW;
		if (SomeWWW.error != null) {
			Debug.LogError ("Error while trying to update ban status: " + SomeWWW.error);
		} else {
			yield return null;
		}
	}
	#endregion
}