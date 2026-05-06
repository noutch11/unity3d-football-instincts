using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalSettings : MonoBehaviour {
	
	void Awake () {
		DontDestroyOnLoad (this);

		if (!PlayerPrefs.HasKey ("Quality Level"))
			PlayerPrefs.SetInt ("Quality Level", QualitySettings.GetQualityLevel ());
		
	/*	if (!PlayerPrefs.HasKey ("Resolution Width") && !PlayerPrefs.HasKey ("Resolution Height")) {
			PlayerPrefs.SetInt ("Resolution Width", Screen.currentResolution.width);
			PlayerPrefs.SetInt ("Resolution Height", Screen.currentResolution.height);
		} */

		if (!PlayerPrefs.HasKey ("General Volume"))
			PlayerPrefs.SetFloat ("General Volume", 0);
		if (!PlayerPrefs.HasKey ("Music Volume"))
			PlayerPrefs.SetFloat ("Music Volume", 0);

//		SetResolution ();
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene () != SceneManager.GetSceneByName ("Match"))
			QualitySettings.SetQualityLevel (3);
		else
			QualitySettings.SetQualityLevel (PlayerPrefs.GetInt ("Quality Level"));
	}

/*	public void SetResolution () {
		if (!PlayerPrefs.HasKey ("Fullscreen"))
			PlayerPrefs.SetInt ("Fullscreen", System.Convert.ToInt32 (Screen.fullScreen));
		else {
			if (PlayerPrefs.GetInt ("Fullscreen") == 0)
				Screen.fullScreen = false;
			else
				Screen.fullScreen = true;
		}
		int w = PlayerPrefs.GetInt ("Resolution Width");
		int h = PlayerPrefs.GetInt ("Resolution Height");

		Screen.SetResolution (w, h, Screen.fullScreen);
	} */

	void OnApplicationQuit () {
		PlayerPrefs.SetInt ("Fullscreen", System.Convert.ToInt32 (Screen.fullScreen));
	}
}
