using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongTrackNameTicker : MonoBehaviour {
	RectTransform r;
	RectTransform p;
	float startX;
	public float tickWaitTime;
	float nextTick;
	float tickSpeed;
	string tickDir;
	// Use this for initialization
	void Start () {
		r = GetComponent<RectTransform> ();
		p = r.parent.GetComponent<RectTransform> ();
		startX = r.anchoredPosition.x;
		tickSpeed = r.rect.width / 5;
	}
	
	// Update is called once per frame
	void Update () {
		if (r.rect.width > p.rect.width) {
			Vector2 updatedPos = r.anchoredPosition;
			if (r.anchoredPosition.x >= startX) {
				tickDir = "Left";
				if (nextTick <= 0)
				nextTick = tickWaitTime;
			}
			if (r.anchoredPosition.x <= (p.rect.width - r.rect.width)) {
				tickDir = "Right";
				if (nextTick <= 0)
				nextTick = tickWaitTime;
			}

			if (nextTick > 0)
				nextTick -= Time.deltaTime;
			if (nextTick <= 0) {
				Tick ();
			}
		} else {
			Vector2 updatedPos = r.anchoredPosition;
			updatedPos.x = startX;
			r.anchoredPosition = updatedPos;
		}
	}

	void Tick () {
			Vector2 updatedPos = r.anchoredPosition;
			if (tickDir == "Left") {
				updatedPos.x -= tickSpeed * Time.deltaTime;
			}
			if (tickDir == "Right") {
				updatedPos.x += tickSpeed * Time.deltaTime;
			}
			r.anchoredPosition = updatedPos;
	}
}
