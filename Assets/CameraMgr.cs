using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour {
	public static GameObject selected;
	Vector3 focus = Vector3.zero;
	float ax = 45;
	float ay = 45;
	float zoom = 50f;

	const float mouseRotSens = 100;

	void Update(){
		if (selected != null) {
			focus += Vector3.Lerp(focus, selected.transform.position, Time.deltaTime);
		}
		transform.position = focus;
		transform.rotation = Quaternion.Euler(ax, ay, 0);
		transform.Translate (0, 0, -zoom);

		if (Input.GetMouseButton (0) || Input.GetMouseButton (1)){
			ax -= Input.GetAxis("Mouse Y")*Time.deltaTime*mouseRotSens;
			ay += Input.GetAxis("Mouse X")*Time.deltaTime*mouseRotSens;
		}
	}
}