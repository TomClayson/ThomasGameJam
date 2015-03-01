using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train : MonoBehaviour {
	public static List<Train> trains = new List<Train>();

	Vector3 position = Vector3.zero;
	Vector3 normal = Vector3.zero;
	public Vector3 target = Vector3.zero;

	public GameObject markerPrefab = null;
	List<Marker> markers = new List<Marker>();

	public Minerals.Ores body = Minerals.Ores.Steel;
	public float health = 1;
	public int moves = 1;
	public int totalMoves = 1;

	public enum Mode {Tunneler, Builder, Assault};
	public Mode mode = Mode.Tunneler;
	

	void Start(){
		trains.Add(this);
		position = transform.position;
		normal = transform.forward;
		target = position;
	}

	void Update(){

		float dist = (target-transform.position).magnitude;
		if (dist<0.1f){
			if ((target-position).normalized!=normal){
				normal = NewNormal(position, target, normal);
			}
			position = target;
			transform.rotation = Quaternion.LookRotation(normal);
			transform.position = position;
		}else{
			float speed = Time.deltaTime*10;


			if ((target-position).normalized!=normal){

				float maxR = speed/10 /3.1415f*180f;
				Vector3 newNorm = NewNormal(position, target, normal);

				transform.Rotate( Vector3.Cross(normal, newNorm), maxR);

			}

				//transform.rotation = Quaternion.RotateTowards(transform.rotation,
				 //                                             Quaternion.LookRotation(target-transform.position),
			      //                                            maxR);
			//transform.Rotate( Vector3.Cross(transform.forward, normal), maxR);
			transform.Translate(0,0,speed);
		}

		//snap position and normals
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
			if ((dir-normal).sqrMagnitude<0.1f)	normal = dir;

		}
		position.x = Mathf.Round(position.x);
		position.y = Mathf.Round(position.y);
		position.z = Mathf.Round(position.z);
				
		if (markers.Count==0 && CameraMgr.selected==gameObject)	Select();
		if (CameraMgr.selected!=gameObject)	Deselect();
	}

	Vector3 NewNormal(Vector3 start, Vector3 end, Vector3 norm){
		if (norm.x==0){
			if (end.x>start.x)	return Vector3.right;
			if (end.x<start.x)	return Vector3.left;
		}
		if (norm.y==0){
			if (end.y>start.y)	return Vector3.up;
			if (end.y<start.y)	return Vector3.down;
		}
		if (norm.z==0){
			if (end.z>start.z)	return Vector3.forward;
			if (end.z<start.z)	return Vector3.back;
		}
		return Vector3.up;
	}
	
	public void NextTurn(){
		moves = totalMoves;
	}

	public void Select(){
		Deselect();
		if (moves<=0)	return;
		//place movement spheres
		for (int i=0; i<10; i++) {
			Quaternion oldrot = transform.rotation;
			transform.rotation = Quaternion.LookRotation(normal);
			Vector3 pos = position;
			switch(i){
			case 0:		pos += normal*10;						break;
			case 1:		pos += normal*10 + transform.up*10;		break;
			case 2:		pos += normal*10 - transform.up*10;		break;
			case 3:		pos += normal*10 + transform.right*10;	break;
			case 4:		pos += normal*10 - transform.right*10;	break;
			case 5:		pos -= normal*10;						break;
			case 6:		pos -= normal*10 + transform.up*10;		break;
			case 7:		pos -= normal*10 - transform.up*10;		break;
			case 8:		pos -= normal*10 + transform.right*10;	break;
			case 9:		pos -= normal*10 - transform.right*10;	break;
			}
			transform.rotation = oldrot;
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
		if (Tunnel.Find(position, pos, normal)!=null)	return true;
		if (Tunnel.Find(position, pos, -normal)!=null)	return true;
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