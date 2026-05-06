using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Text.RegularExpressions;

public class Account : MonoBehaviour {
	#region vars
	/// <summary>
	/// Regular expression, which is used to validate an E-Mail address.
	/// </summary>
	public const string MatchEmailPattern =
		@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
              + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
	//Static vars
	public static string Username = "";
	public static string Type = "";
	public static string resultMsg = "Welcome to Football Instincts! Go ahead and login or create an account.\nTip: You can drag the white box to adjust the position";
	public static bool loggedin = false;
	//Private vars
	private string CreateAccountURL = "http://fi-domaindontsee.000webhostapp.com/CreateAccount.php";
	private string LoginURL = "http://fi-domaindontsee.000webhostapp.com/LogintoAccount.php";
	private string CreatePass = "";
	private string ConfirmPass = "";
	private string CreateUsername = "";
	private string CreateEmail = "";
	private string LoginEmail = "";
	private string LoginPass = "";
	private string ForgotPass_EMAIL = "";
	string CurrentMenu = "";
	//Public vars
	public GameObject loadingScreen;
	public GameObject loginTab, registerTab, alertBox;
	#endregion
	// Use this for initialization
	void Start () {
	//	DontDestroyOnLoad (this);
		CurrentMenu = "Login";
		if (PlayerPrefs.HasKey ("Login Email"))
		loginTab.transform.Find ("EmailField").GetComponent<InputField>().text = PlayerPrefs.GetString ("Login Email");
	}

	public static bool IsEmail(string email) 
	{
		return Regex.IsMatch(email, MatchEmailPattern);
	}

	void OnDestroy () {
		PlayerPrefs.SetString ("Login Email", LoginEmail);
	}

	void Update () {
		if (SceneManager.GetSceneByName ("Menu").isLoaded) {
			if (CurrentMenu == "Login") {
				LoginUI ();
				alertBox = loginTab.transform.Find ("Alert Box").gameObject;
				registerTab.transform.position = loginTab.transform.position;
			} else if (CurrentMenu == "Register") {
				CreateAccountUI ();
				alertBox = registerTab.transform.Find ("Alert Box").gameObject;
				loginTab.transform.position = registerTab.transform.position;
			}
			alertBox.GetComponentInChildren<Text> ().text = resultMsg;
		}
	}

	#region ui events

	#region login
	//input field - email
	public void InputEmail (string inputTxt) {
		LoginEmail = inputTxt;
	}
	//input field - password
	public void InputPassword (string inputTxt) {
		LoginPass = inputTxt;
	}

	//button - login
	public void LoginButton () {
		if (LoginEmail != "" && LoginPass != "") {
		//	alertBox.SetActive (true);
			resultMsg = "Logging in...";
			StartCoroutine ("Login");
		} else {
		//	alertBox.SetActive (true);
			resultMsg = "Please complete each field.";
		}
	}
	//button - go to create account tab
	public void CreateAccountButton () {
		CurrentMenu = "Register";
		resultMsg = "Welcome to Football Instincts! Go ahead and login or create an account.\nTip: You can drag the white box to adjust the position";
	//	alertBox.SetActive (false);
	}

	#endregion

	#region register
	//input field - create email
	public void CreateEmailInput (string inputTxt) {
		CreateEmail = inputTxt;
	}
	//input field - create pass
	public void CreatePassInput (string inputTxt) {
		CreatePass = inputTxt;
	}
	//input field - confirm pass
	public void ConfirmPassInput (string inputTxt) {
		ConfirmPass = inputTxt;
	}
	//input field - create username
	public void CreateUsernameInput (string inputTxt) {
		CreateUsername = inputTxt;
	}

	//button - back
	public void BackButton () {
		CurrentMenu = "Login";
		resultMsg = "Welcome to Football Instincts! Go ahead and login or create an account.\nTip: You can drag the white box to adjust the position";
	//	alertBox.SetActive (false);
	}
	//button - finally check if registration will can be done
	public void FinalizeRegister () {
	//	alertBox.SetActive (true);
		resultMsg = "Checking...";
		if (CreateUsername != "" && CreatePass != "" && CreateEmail != "" && CreateUsername.Length >= 3 && CreatePass.Length >= 6 && IsEmail (CreateEmail) && ConfirmPass == CreatePass) {
			StartCoroutine ("Register");
		} else if (CreateUsername == "" || CreatePass == "" || CreateEmail == "" || ConfirmPass == "") {
			resultMsg = "Please complete each field.";
		} else if ((CreateUsername.Length < 3 && CreateUsername != "") || (CreatePass.Length < 6 && CreatePass != "")) {
			resultMsg = "Minimum lengths - Username: 3, Password: 6";
		} else if (CreatePass != ConfirmPass) {
			resultMsg = "Passwords do not match.";
		} else if (IsEmail (CreateEmail) == false) {
			resultMsg = "Please enter a valid email!";
		}
	}
	#endregion

	#endregion

	#region custom voids
	//Login to an account (UI)
	void LoginUI () {
		loginTab.SetActive (true);
		registerTab.SetActive (false);
	}

	//Create an account (UI)
	void CreateAccountUI () {
		loginTab.SetActive (false);
		registerTab.SetActive (true);
	}
	#endregion

	#region coroutines
	//Actually now creating an account
	IEnumerator Register () {
		WWWForm form = new WWWForm ();
		form.AddField ("Email", CreateEmail);
		form.AddField ("Username", CreateUsername);
		form.AddField ("Password", CreatePass);
		WWW CreateAccountWWW = new WWW (CreateAccountURL, form);
		//Wait for php to send something back to Unity
		yield return CreateAccountWWW;
		if (CreateAccountWWW.error != null) {
			Debug.LogError ("Error while trying to create an account: " + CreateAccountWWW.error);
			resultMsg = "An error occured. Please try again.";
		} else {
			string CreateAccountReturn = CreateAccountWWW.text;
			alertBox.SetActive (true);
			if (CreateAccountReturn == "Success") {
				Debug.Log ("Success: Account created successfully! The register URL printed '" + CreateAccountReturn + "'");
				resultMsg = "Account created successfully! You will now be logged in.";
				yield return new WaitForSeconds (2.1f);
				StartCoroutine (LoadingScreen ());
				loggedin = true;
				resultMsg = "";
				ServerDBConnection globalSettings = GameObject.Find ("_GlobalSettings").GetComponent<ServerDBConnection> ();
				globalSettings.StartCoroutine(globalSettings.GetUsername (CreateEmail));
			} else if (CreateAccountReturn == "This username or email already exists.")
				resultMsg = "This username or email already exists. Please try again.";
			else
				resultMsg = "Unknown error.";
		}

	}
	//Actually now logging into an account
	IEnumerator Login () {
		WWWForm form = new WWWForm ();
		form.AddField ("Email", LoginEmail);
		form.AddField ("Password", LoginPass);
		WWW LogintoAccountWWW = new WWW (LoginURL, form);
		//Wait for php to send something back to Unity
		yield return LogintoAccountWWW;
		if (LogintoAccountWWW.error != null) {
			Debug.LogError ("Error while trying to log in: " + LogintoAccountWWW.error);
			resultMsg = "An error occured. Please try again.";
		} else {
			string LoginReturn = LogintoAccountWWW.text;
			if (LoginReturn == "Success") {
				Debug.Log ("Success: Logged in successfully! The login URL printed '" + LoginReturn + "'");
				resultMsg = "Logged in successfully. Welcome back!";
				yield return new WaitForSeconds (1.1f);
				StartCoroutine (LoadingScreen ());
				loggedin = true;
				resultMsg = "";
				ServerDBConnection globalSettings = GameObject.Find ("_GlobalSettings").GetComponent<ServerDBConnection> ();
				globalSettings.StartCoroutine(globalSettings.GetUsername (LoginEmail));

			} else if (LoginReturn == "Incorrect email") {
				resultMsg = "This user does not exist.";
			} else if (LoginReturn == "Incorrect password") {
				resultMsg = "Invalid username and password combination.";
			}
			else if (resultMsg.Contains ("banned")) {
				resultMsg = "<color=#960000FF>" + LoginReturn + "</color>";
			} else
				resultMsg = "Unknown error.";
		}
	}

	IEnumerator LoadingScreen () {
		int loadProgress = 0;
		loadingScreen.SetActive (true);
		loadingScreen.GetComponentInChildren<Text> ().text = loadProgress + "%";
		yield return new WaitForSeconds (3);

		AsyncOperation async = SceneManager.LoadSceneAsync ("Lounge");
		while (!async.isDone) {
			loadProgress = (int)(async.progress * 100);
			loadingScreen.GetComponentInChildren<Text> ().text = loadProgress + "%";
			yield return null;
		}
	}
	#endregion
}
