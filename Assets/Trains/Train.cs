using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train : MonoBehaviour {
	public static List<Train> trains = new List<Train>();

	public GameObject markerPrefab = null;
	List<Marker> markers = new List<Marker>();

	public Minerals.Ores body = Minerals.Ores.Steel;
	public float health = 1;
	public int moves = 1;
	public int totalMoves = 1;

	public enum Mode {Tunneler, Builder, Assault};
	public Mode mode = Mode.Tunneler;

	public Vector3 target = Vector3.zero;
	
	void Start(){
		trains.Add(this);
		target = transform.position;
	}

	void Update(){
		if (target!=transform.position){
			float dist = (target-transform.position).magnitude;
			if (dist<0.1f){
				transform.position = target;
			}else{
				float speed = Time.deltaTime*10;
				float maxR = speed/10 /3.1415f*180f;
				transform.rotation = Quaternion.RotateTowards(transform.rotation,
				                                              Quaternion.LookRotation(target-transform.position),
			                                                  maxR);
				transform.Translate(0,0,speed);
			}
		}else{
			for(int i=0; i<6; i++){
				Vector3 dir = Vector3.up;
				switch(i){
				case 0:	dir = Vector3.up;		break;
				case 1:	dir = Vector3.down;		break;
				case 2:	dir = Vector3.right;	break;
				case 3:	dir = Vector3.left;		break;
				case 4:	dir = Vector3.forward;	break;
				case 5:	dir = Vector3.back;		break;
				}
				if ((dir-transform.forward).sqrMagnitude<0.1f){
					transform.rotation = Quaternion.LookRotation(dir);
				}
				Vector3 pos = transform.position;
				pos.x = Mathf.Round(pos.x);
				pos.y = Mathf.Round(pos.y);
				pos.z = Mathf.Round(pos.z);
				transform.position = pos;
			}
		}

/*		float speed = Time.deltaTime*10;
		Vector3 pos = transform.position;
		if ((target-pos).sqrMagnitude<0.1f){
			pos = target;
		}else{
			float mag = (transform.forward-target-pos).magnitude;
			float dot = Vector3.Dot(transform.forward, (target-pos).normalized);
			float dotmag = Vector3.Dot(transform.forward, target-pos);
			float dx = pos.x - Mathf.Round(pos.x/10)*10;
			float dy = pos.y - Mathf.Round(pos.y/10)*10;
			float dz = pos.z - Mathf.Round(pos.z/10)*10;
			if ( Mathf.Abs(dx)>0.01f || Mathf.Abs(dy)>0.01f || Mathf.Abs(dz)>0.01f){
				pos += transform.forward*Mathf.Min(dotmag,speed);
			}else{
				//rotate
				if (dot<1){
					transform.rotation = Quaternion.RotateTowards(transform.rotation,
				                                              Quaternion.LookRotation(target-pos),
				                                              Time.deltaTime*500);
				}else{
					pos += transform.forward*Mathf.Min(mag,speed);
				}

				//new tunnel
				if (Tunnel.FindTunnel(target)==null)	WorldMgr.MakeTunnel(target);
			}
		}
		Vector3 euler = transform.rotation.eulerAngles;
		euler.x = Mathf.Round(euler.x);
		euler.y = Mathf.Round(euler.y);
		euler.z = Mathf.Round(euler.z);
		transform.rotation = Quaternion.Euler(euler);

		pos.x = Mathf.Round(pos.x*10f)/10f;
		pos.y = Mathf.Round(pos.y*10f)/10f;
		pos.z = Mathf.Round(pos.z*10f)/10f;
		transform.position = pos;*/

		if (markers.Count==0 && CameraMgr.selected==gameObject)	Select();
		if (CameraMgr.selected!=gameObject)	Deselect();
	}

	public void NextTurn(){
		moves = totalMoves;
	}

	public void Select(){
		Deselect();
		if (moves<=0)	return;
		//place movement spheres
		for (int i=0; i<5; i++) {
			Vector3 pos = transform.position + transform.forward * 10;
			switch(i){
			case 1:		pos += transform.up * 10;		break;
			case 2:		pos += -transform.up * 10;		break;
			case 3:		pos += transform.right * 10;	break;
			case 4:		pos += -transform.right * 10;	break;
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
		if (markers.Count==0)	return;
		foreach(Marker mark in markers){
			Destroy(mark.gameObject);
		}
		markers.Clear();
	}

	bool ValidMovePos(Vector3 pos){
		//if (mode==Mode.Tunneler)	return true;
		if (Tunnel.Find(transform.position, pos, transform.forward)!=null)	return true;
		return false;
	}

	public void Window(){
		GUILayout.Box (name);
		GUILayout.Box ("Health "+Mathf.Floor(health*100).ToString());
		GUILayout.Box ("Moves " +moves.ToString()+ " / " + totalMoves.ToString());
		GUILayout.Box (mode.ToString ());
	}

	/*
	 * 
	 * mining
	 * mines resources
	 * 
	 * factory
	 * builds new trains
	 * 
	 * colony
	 * houses people - gives income? maybe
	 * 
	 */
}