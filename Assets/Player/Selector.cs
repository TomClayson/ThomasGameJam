using UnityEngine;
using System.Collections;

public class Selector : MonoBehaviour {
	public static GameObject selected;
	public GUISkin skin;
	bool clicked = false;
	float clickHold = 0f;
	
	void OnGUI(){
		GUI.depth = 10;
		GUI.skin = skin;
		
		if (GUI.RepeatButton(new Rect(0,0,Screen.width,Screen.height), "")){
			clicked = true;
		}
	}

	void LateUpdate(){
		if (Player.currentGameMode!=Player.GameMode.Game)	return;

		if (Input.touchCount==0 && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)){
			if (clicked && clickHold<0.3f)		SelectObject();
			clickHold = 0;
			clicked = false;
		}else{
			clickHold += Time.deltaTime;
		}
	}

	void SelectObject(){
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			selected = hit.collider.gameObject;
			if (selected.GetComponent<Train>()!=null){
				selected.GetComponent<Train>().Select();
			}
			if (selected.GetComponent<Marker>()!=null){
				selected.GetComponent<Marker>().Select();
			}
		}
	}
}