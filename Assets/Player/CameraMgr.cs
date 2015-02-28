using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour {
	public GameObject initSelected;
	public static GameObject selected;
	Vector3 focus = Vector3.zero;
	float ax = 45;
	float ay = 45;
	float zoom = 50f;
	bool canClick = false;

	const float mouseRotSens = 100;

	void Awake(){
		selected = initSelected;
	}

	void Update(){
		if (selected != null) {
			focus = Vector3.Lerp(focus, selected.transform.position, Time.deltaTime*10);
		}
		transform.position = focus;
		transform.rotation = Quaternion.Euler(ax, ay, 0);
		transform.Translate (0, 0, -zoom);

		if (Input.GetMouseButton (1)){
			ax -= Input.GetAxis("Mouse Y")*Time.deltaTime*mouseRotSens;
			ay += Input.GetAxis("Mouse X")*Time.deltaTime*mouseRotSens;
		}
		zoom -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime*2000f;
		zoom = Mathf.Clamp (zoom, 10, 100);

		if (!Input.GetMouseButton(0))	canClick = true;

		if (Input.GetMouseButton (0) && canClick) {
			canClick = false;
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				selected = hit.collider.gameObject;
				if (selected.GetComponent<Train>()!=null){
					selected.GetComponent<Train>().Select();
				}
			}
		}
	}

	void OnGUI(){
		if (selected != null) {
			GUILayout.BeginArea(new Rect(0, 0, 500, Screen.height));
			GUILayout.FlexibleSpace();
			if (selected.GetComponent<Colony>()!=null)	selected.GetComponent<Colony>().Window();
			if (selected.GetComponent<Train>()!=null)	selected.GetComponent<Train>().Window();
			if (selected.GetComponent<Minerals>()!=null)	selected.GetComponent<Minerals>().Window();
			GUILayout.EndArea();
		}
	}
}