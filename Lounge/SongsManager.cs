using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SongsManager : MonoBehaviour {
	public AudioClip[] clips;
    AudioClip lastPlayedClip;
	public AudioSource audioSource;
	public int trackNumber;
	public GameObject trackText;
	public GameObject muteImg;
	public GameObject unmutedImg;
    bool shuffle;
    public GameObject shuffleImg;

	// Use this for initialization
	void Start () {
		trackNumber = Random.Range (0, clips.Length - 1);
		audioSource.clip = clips [trackNumber];
        lastPlayedClip = audioSource.clip;
        trackText.GetComponent<Text>().text = audioSource.clip.name;
        audioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audioSource.isPlaying) {
            if (shuffle)
                GetRandomClip();
            else
            {
                audioSource.clip = GetNextClip();
                audioSource.Play();
            }
		}

		if (Input.GetKeyDown (KeyCode.RightArrow) && Input.GetAxisRaw ("Walk") != 0) {
            lastPlayedClip = audioSource.clip;
			audioSource.Stop ();
            if (shuffle)
                GetRandomClip();
            else
            {
                audioSource.clip = GetNextClip();
                trackText.GetComponent<Text>().text = audioSource.clip.name;
                audioSource.Play();
            }
		}

        if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetAxisRaw("Walk") != 0)
        {
            audioSource.Stop();
            if (shuffle)
            {
                audioSource.clip = lastPlayedClip;
                trackText.GetComponent<Text>().text = audioSource.clip.name;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = GetPreviousClip();
                trackText.GetComponent<Text>().text = audioSource.clip.name;
                audioSource.Play();
            }
        }

            if (Input.GetKeyDown (KeyCode.M) && Input.GetAxisRaw ("Walk") != 0) {
			audioSource.mute = !audioSource.mute;
			muteImg.SetActive (!muteImg.activeInHierarchy);
			unmutedImg.SetActive (!unmutedImg.activeInHierarchy);
		}

        if (Input.GetKeyDown(KeyCode.R) && Input.GetAxisRaw("Walk") != 0)
            {
                shuffle = !shuffle;
      /*      if (shuffle)
                shuffleImg.GetComponent<Image>().color = Color.green;
            else
                shuffleImg.GetComponent<Image>().color = Color.white; */
      
            }
	}

	AudioClip GetNextClip () {
		trackNumber++;
		ClipBounds ();
		return clips [trackNumber];
	}

	AudioClip GetPreviousClip () {
		trackNumber--;
		ClipBounds ();
		return clips [trackNumber];
	}

	void ClipBounds () {
		if (trackNumber < 0)
			trackNumber = clips.Length - 1;
		
		if (trackNumber == clips.Length)
			trackNumber = 0;
	}

    void GetRandomClip()
    {
        trackNumber = Random.Range(0, clips.Length - 1);
        if (audioSource.clip != clips[trackNumber])
        {
            audioSource.clip = clips[trackNumber];
            trackText.GetComponent<Text>().text = audioSource.clip.name;
            audioSource.Play();
        }
        else
            GetRandomClip();
    }
}
