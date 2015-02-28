using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train : MonoBehaviour {
	public GameObject markerPrefab = null;
	List<Marker> markers = new List<Marker>();

	public Minerals.Ores body = Minerals.Ores.Steel;
	public float health = 1;
	public int moves = 1;
	public int totalMoves = 1;

	public enum Mode {Tunneler, Miner, Builder, Assault};
	public Mode mode = Mode.Tunneler;

	public Vector3 target = Vector3.zero;

	void Start(){
		target = transform.position;
	}

	void Update(){
		float speed = Time.deltaTime*10;
		Vector3 pos = transform.position;
		if (target!=pos){
			float mag = (transform.forward-target-pos).magnitude;
			float dot = Vector3.Dot(transform.forward, (target-pos).normalized);
			if (pos.x%10!=0 || pos.y%10!=0 || pos.z%10!=0){
				pos += transform.forward*Mathf.Min(mag,speed);
			}else{
				if (dot<1){
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
				                                              Quaternion.LookRotation(target-pos),
				                                              Time.deltaTime*500);
				}else{
					pos += transform.forward*Mathf.Min(mag,speed);
				}
			}
		}
		Debug.Log(transform.forward.x.ToString()+" "+transform.forward.y.ToString()+" "+transform.forward.z.ToString());
		Vector3 euler = transform.rotation.eulerAngles;
		euler.x = Mathf.Round(euler.x);
		euler.y = Mathf.Round(euler.y);
		euler.z = Mathf.Round(euler.z);
		transform.rotation = Quaternion.Euler(euler);

		pos.x = Mathf.Round(pos.x*10f)/10f;
		pos.y = Mathf.Round(pos.y*10f)/10f;
		pos.z = Mathf.Round(pos.z*10f)/10f;
		transform.position = pos;

		if (markers.Count>0 && CameraMgr.selected!=gameObject)	Deselect();
	}

	public void NextTurn(){
		moves = totalMoves;
	}

	public void Select(){
		Deselect();
		//place movement spheres
		for (int i=0; i<5; i++) {
			Vector3 pos = transform.position + transform.forward * 5;
			switch(i){
			case 0:		pos += transform.forward * 5;	break;
			case 1:		pos += transform.up * 5;		break;
			case 2:		pos += -transform.up * 5;		break;
			case 3:		pos += transform.right * 5;		break;
			case 4:		pos += -transform.right * 5;	break;
			}
			if (ValidMovePos(pos)) {
				Marker newMark = ((GameObject)Instantiate(markerPrefab)).GetComponent<Marker>();
				newMark.train = this;
				newMark.transform.position = pos;
				markers.Add(newMark);
			}
		}

	}

	public void Deselect(){
		foreach(Marker mark in markers){
			Destroy(mark.gameObject);
		}
		markers.Clear();
	}

	bool ValidMovePos(Vector3 pos){
		foreach (Tunnel tunnel in WorldMgr.tunnels) {
			if ((tunnel.transform.position-pos).sqrMagnitude<1f)		return true;
		}
		return false;
	}

	public void Window(){
		GUILayout.Box (name);
		GUILayout.Box ("Health "+Mathf.Floor(health*100).ToString());
		GUILayout.Box ("Moves " +moves.ToString()+ " / " + totalMoves.ToString());
		GUILayout.Box (mode.ToString ());
	}
}