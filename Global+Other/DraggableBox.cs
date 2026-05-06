using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBox : MonoBehaviour, IDragHandler {
	RectTransform draggableRect;
	RectTransform canvasRect;

	void Start () {
		if (transform.parent.name != "Canvas") {
			draggableRect = transform.parent as RectTransform;
			canvasRect = transform.parent.parent as RectTransform;
		} else {
			draggableRect = transform as RectTransform;
			canvasRect = transform.parent as RectTransform;
		}
	}
	public void OnDrag (PointerEventData eventData) {
		draggableRect.GetComponent<Transform>().position += (Vector3) eventData.delta;
	}
	void Update () {
		
		ClampPos ();
	}
	void ClampPos () {
			Vector3 pos = draggableRect.localPosition;

			Vector3 min = canvasRect.rect.min - draggableRect.rect.min;
			Vector3 max = canvasRect.rect.max - draggableRect.rect.max;

			pos.x = Mathf.Clamp (draggableRect.localPosition.x, min.x, max.x);
			pos.y = Mathf.Clamp (draggableRect.localPosition.y, min.y, max.y);
			draggableRect.localPosition = pos;
	}
}
