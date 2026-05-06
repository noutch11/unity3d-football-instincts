using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TickerTape : MonoBehaviour {
	public float speed;
	RectTransform position;
	Vector2 startPos;

	void Start () {
		position = GetComponent<RectTransform> ();
		startPos = position.anchoredPosition;
	}

	// Update is called once per frame
	void Update () {
		if (position.rect.width > transform.parent.GetComponent<RectTransform> ().rect.width) {
			Vector2 updatedPos = position.anchoredPosition;
			updatedPos.x -= Time.deltaTime * speed;
			position.anchoredPosition = updatedPos;

			if (position.anchoredPosition.x <= startPos.x - position.rect.width) {
				updatedPos.x = (transform.parent.GetComponent<RectTransform> ().anchoredPosition.x + 697);
				position.anchoredPosition = updatedPos;
			}
		} else
			position.anchoredPosition = startPos;
	}
}
