using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour {
	public Gradient background;
	Vector3 focus = Vector3.zero;
	float ax = 45;
	float ay = 45;
	float zoom = 50f;

	const float mouseRotSens = 100;

	void Awake(){
		Player.Init ();
	}

	void Update(){
		if (Player.currentGameMode!=Player.GameMode.Design)
			camera.backgroundColor = background.Evaluate(Mathf.Clamp01(1-focus.y/500));

		if (Selector.selected != null) {
			focus = Vector3.Lerp(focus, Selector.selected.transform.position, Time.deltaTime*10);
		}
		if (Player.currentGameMode==Player.GameMode.Design)		focus = Vector3.zero;
		transform.position = focus;
		transform.rotation = Quaternion.Euler(ax, ay, 0);
		transform.Translate (0, 0, -zoom);

		if (Input.GetMouseButton (1)){
			ax -= Input.GetAxis("Mouse Y")*Time.deltaTime*mouseRotSens;
			ay += Input.GetAxis("Mouse X")*Time.deltaTime*mouseRotSens;
		}

		/*if (Player.currentGameMode==Player.GameMode.Design){
			zoom = Mathf.Lerp(zoom, 1, Time.deltaTime*10);
			return;
		}*/

		zoom -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime*2000f;
		zoom = Mathf.Clamp (zoom, 10, 300);
	}
}