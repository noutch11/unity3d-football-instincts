using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour {
	public Image img;
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, -15);
		img.color = new Color (img.color.r, img.color.g, img.color.b, Mathf.PingPong (Time.time, 1));
	}
}
