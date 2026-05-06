using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {
	public AudioMixer audMix;
	public Slider musicVol, generalVol;
	public Dropdown resDrop, qualityDrop;
	public Toggle fullscreenTog;
	public Button saveButton, moderationButton;
	int qualityLvl;
	Resolution[] resolutions;
	Resolution currentRes;
	void Awake () {
		resolutions = Screen.resolutions;
		GetPrefs ();
	}

	public void SetMusicVolume (float volume) {
		audMix.SetFloat ("Music Volume", volume);
		if (!saveButton.interactable)
			saveButton.interactable = true;
	}
	public void SetGeneralVolume (float volume) {
		audMix.SetFloat ("General Volume", volume);
		if (!saveButton.interactable)
			saveButton.interactable = true;
	}

	public void SetQuality (int qualityIndex) {
		qualityLvl = qualityIndex;
		if (!saveButton.interactable)
			saveButton.interactable = true;
	}

	public void SaveButton () {
		PlayerPrefs.SetInt ("Quality Level", qualityLvl);
		PlayerPrefs.SetInt ("Fullscreen", System.Convert.ToInt32 (Screen.fullScreen));
//		PlayerPrefs.SetInt ("Resolution Width", currentRes.width);
//		PlayerPrefs.SetInt ("Resolution Height", currentRes.height);
		PlayerPrefs.SetFloat ("General Volume", generalVol.value);
		PlayerPrefs.SetFloat ("Music Volume", musicVol.value);
		PlayerPrefs.Save ();
		saveButton.interactable = false;
		GetPrefs ();
	}

	public void SetResolution (int resIndex) {
	//	currentRes = resolutions [resIndex];
		Resolution res = resolutions [resIndex];
		Screen.SetResolution (res.width, res.height, Screen.fullScreen);
	//	Screen.SetResolution (currentRes.width, currentRes.height, Screen.fullScreen);
//		if (!saveButton.interactable)
//			saveButton.interactable = true;
	}

	public void FullscreenToggle (bool isFullscreen) {
		Screen.fullScreen = isFullscreen;
		if (!saveButton.interactable)
			saveButton.interactable = true;
	}

	public void CloseButton () {
		GameObject.Find ("Settings Button").GetComponent<Button> ().interactable = true;
		gameObject.SetActive (false);
	//	if (saveButton.interactable)
	//		GameObject.Find ("_GlobalSettings").GetComponent<GlobalSettings> ().SetResolution ();
		saveButton.interactable = false;
	}

	public void LogoutButton () {
		SceneManager.LoadScene ("Menu");
		PhotonNetwork.Disconnect ();
		Account.loggedin = false;
	}

	public void GetPrefs () {
	//	if (!(Screen.currentResolution.width == PlayerPrefs.GetInt ("Resolution Width") && Screen.currentResolution.height == PlayerPrefs.GetInt ("Resolution Height")))
	//		GameObject.FindObjectOfType<GlobalSettings> ().SetResolution ();
		qualityLvl = PlayerPrefs.GetInt ("Quality Level");
		qualityDrop.value = qualityLvl;
		qualityDrop.RefreshShownValue ();

		fullscreenTog.isOn =  Screen.fullScreen;

		generalVol.value = PlayerPrefs.GetFloat ("General Volume");
		musicVol.value = PlayerPrefs.GetFloat ("Music Volume");

		resDrop.ClearOptions ();
		List<string> options = new List<string> ();
		int currentResIndex = 0;
		for (int i = 0; i < resolutions.Length; i++) {
			string option = resolutions [i].width + " x " + resolutions [i].height;
			options.Add (option);
			if (resolutions [i].width == Screen.currentResolution.width && resolutions [i].height == Screen.currentResolution.height) {
				currentResIndex = i;
		//		currentRes = resolutions [i];
			}
		}
		resDrop.AddOptions (options);
		resDrop.value = currentResIndex;
		resDrop.RefreshShownValue ();

		saveButton.interactable = false;
	}
}
