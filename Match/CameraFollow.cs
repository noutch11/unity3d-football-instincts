using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float distance;
	public float sensitvity;

    public float yMinLimit;
    public float yMaxLimit;

	public float distanceMin;
	public float distanceMax;

	float x = 0.0f;
	float y = 0.0f;

	// Use this for initialization
	void Start () 
	{
		if (PlayerPrefs.HasKey ("Camera Zoom"))
			distance = PlayerPrefs.GetFloat ("Camera Zoom");
		
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
	}

	void LateUpdate () 
	{
		if (target) 
		{
			x += Input.GetAxis("Mouse X") * (sensitvity / 2) * 0.1f;
			y -= Input.GetAxis("Mouse Y") * (sensitvity / 2) * 0.1f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);

			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) 
			{
		//		distance -=  hit.distance;
			}
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + new Vector3 (target.position.x, target.position.y, target.position.z);

			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}

	void OnApplicationQuit () {
		PlayerPrefs.SetFloat ("Camera Zoom", distance);
	}
}