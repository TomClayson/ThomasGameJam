﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Train : MonoBehaviour {
	public static List<Train> trains = new List<Train>();

	Vector3 position = Vector3.zero;
	Vector3 normal = Vector3.zero;
	public Vector3 target = Vector3.zero;

	public GameObject markerPrefab = null;
	List<Marker> markers = new List<Marker>();
	public GameObject torpedoPrefab = null;

	public GameObject buildingPrefab = null;

	public Minerals.Ores body = Minerals.Ores.Steel;
	public float health = 1;
	public float fuel = 0;
	public float maxfuel = 10;
	public float armour = 10;
	public int moves = 1;
	int miners = 0;
	int tunnelers = 0;
	int builders = 0;
	int torpedos = 0;
	public int totalMoves = 3;

	public Carriage[] carriages;

	void Start(){
		moves = 0;
		trains.Add(this);
		position = transform.position;
		normal = transform.forward;
		target = position;

		for (int i=0; i<carriages.Length; i++) {
			if (carriages[i]!=null){
				carriages[i].renderer.material = WorldMgr.local.minMats[(int)body];
				if (carriages[i].mode==Carriage.Mode.Miner)		miners++;
				if (carriages[i].mode==Carriage.Mode.Tunneler)	tunnelers++;
				if (carriages[i].mode==Carriage.Mode.Builder)	builders++;
				if (carriages[i].mode==Carriage.Mode.Torpedos)	torpedos++;
			}
		}

	}

	void Update(){
		fuel = Mathf.Clamp (fuel, 0f, 10000f);

		//carriage manager
		for (int i=0; i<carriages.Length; i++) {
			if (carriages[i]!=null)		carriages[i].transform.localPosition = new Vector3(0,0,-i);
		}

		if (target!=position){
			//make new tunnel
			if (Tunnel.Find(position,target,normal)==null){
				if (Vector3.Dot(target-position, normal)>0.1f){
					WorldMgr.MakeTunnel(normal,position, target);
				}else if (Vector3.Dot(target-position, normal)<-0.1f){
					WorldMgr.MakeTunnel(-normal,position, target);
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
						if ((dir-(target-position).normalized).sqrMagnitude<1.1f){
							normal = dir;
							Debug.Log(normal);
							break;
						}
					}
					WorldMgr.MakeTunnel(normal,position, target);
				}
			}

			//set new position
			if (((target-position).normalized-normal).sqrMagnitude>0.1f && ((target-position).normalized+normal).sqrMagnitude>0.1f){
				normal = NewNormal(position, target, normal);
			}
			position = target;
		}

		float dist = (position-transform.position).magnitude;
		//if (dist<1f){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(normal), Time.deltaTime*2f);
			transform.position = Vector3.Slerp(transform.position,position, Time.deltaTime*2f);
		/*}else{
			//get the nice movement working later!!!
			float speed = Time.deltaTime*10;


			if ((position-transform.position).normalized!=normal){
				float maxR = speed/10 /3.1415f*180f;
				transform.Rotate( Vector3.Cross(normal, position-transform.position).normalized, -maxR);
			}

				//transform.rotation = Quaternion.RotateTowards(transform.rotation,
				 //                                             Quaternion.LookRotation(target-transform.position),
			      //                                            maxR);
			//transform.Rotate( Vector3.Cross(transform.forward, normal), maxR);
			transform.Translate(0,0,speed);
		}*/

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
				
		if (markers.Count==0 && Selector.selected==gameObject)	Select();
		if (Selector.selected!=gameObject)	Deselect();
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

	public void Damage(float dam){
		health -= dam / armour;
	}
	
	public void NextTurn(){
		moves = totalMoves;
		fuel--;

		//mining
		if (miners>0){
			foreach(Minerals min in WorldMgr.minerals){
				if (min!=null){
					if ((min.transform.position-position).sqrMagnitude<1){
						float delta = Mathf.Min(5*miners, min.amount);
						min.amount -= delta;
						Player.minerals[(int)min.ore] += delta;
					}
				}
			}
		}

		//heal
		fuel = maxfuel;
		if (InBuilding())	health += 0.2f;
		health = Mathf.Clamp01(health);
	}

	public void Select(){
		Deselect();
		if (moves<=0)	return;

		//check if in buildings firstly
		if (InBuilding()!=null){
			for (int i=0; i<18; i++) {
				Vector3 pos = position;
				switch(i){
				case 0:		pos += Vector3.up*10;						break;
				case 1:		pos += Vector3.up*10 + Vector3.right*10;	break;
				case 2:		pos += Vector3.up*10 - Vector3.right*10;	break;
				case 3:		pos += Vector3.up*10 + Vector3.forward*10;	break;
				case 4:		pos += Vector3.up*10 - Vector3.forward*10;	break;
				case 5:		pos += Vector3.down*10;						break;
				case 6:		pos += Vector3.down*10 + Vector3.right*10;	break;
				case 7:		pos += Vector3.down*10 - Vector3.right*10;	break;
				case 8:		pos += Vector3.down*10 + Vector3.forward*10;break;
				case 9:		pos += Vector3.down*10 - Vector3.forward*10;break;
				case 10:	pos += Vector3.right*10;					break;
				case 11:	pos += Vector3.left*10;						break;
				case 12:	pos += Vector3.forward*10;					break;
				case 13:	pos += Vector3.back*10;						break;
				case 14:	pos += Vector3.right*10 + Vector3.back*10;	break;
				case 15:	pos += Vector3.right*10 - Vector3.back*10;	break;
				case 16:	pos += Vector3.left*10 + Vector3.back*10;	break;
				case 17:	pos += Vector3.left*10 - Vector3.back*10;	break;
				}
				if (ValidMoveBuilding(pos)){
					Marker newMark = ((GameObject)Instantiate(markerPrefab)).GetComponent<Marker>();
					newMark.train = this;
					newMark.transform.position = pos;
					newMark.currentMode = Marker.Modes.Movement;
					markers.Add(newMark);
				}
			}
		}else{

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
					newMark.currentMode = Marker.Modes.Movement;
					markers.Add(newMark);

				}
			}
		}

		//place attack spheres
		if (torpedos>0){
			Debug.Log ("tops");
			for (int i=0; i<18; i++) {
				Vector3 pos = position;
				switch(i){
				case 0:		pos += Vector3.up*10;						break;
				case 1:		pos += Vector3.up*10 + Vector3.right*10;	break;
				case 2:		pos += Vector3.up*10 - Vector3.right*10;	break;
				case 3:		pos += Vector3.up*10 + Vector3.forward*10;	break;
				case 4:		pos += Vector3.up*10 - Vector3.forward*10;	break;
				case 5:		pos += Vector3.down*10;						break;
				case 6:		pos += Vector3.down*10 + Vector3.right*10;	break;
				case 7:		pos += Vector3.down*10 - Vector3.right*10;	break;
				case 8:		pos += Vector3.down*10 + Vector3.forward*10;break;
				case 9:		pos += Vector3.down*10 - Vector3.forward*10;break;
				case 10:	pos += Vector3.right*10;					break;
				case 11:	pos += Vector3.left*10;						break;
				case 12:	pos += Vector3.forward*10;					break;
				case 13:	pos += Vector3.back*10;						break;
				case 14:	pos += Vector3.right*10 + Vector3.back*10;	break;
				case 15:	pos += Vector3.right*10 - Vector3.back*10;	break;
				case 16:	pos += Vector3.left*10 + Vector3.back*10;	break;
				case 17:	pos += Vector3.left*10 - Vector3.back*10;	break;
				}
				if (Monster.Find(pos)){
					Marker newMark = ((GameObject)Instantiate(markerPrefab)).GetComponent<Marker>();
					newMark.train = this;
					newMark.transform.position = pos;
					newMark.currentMode = Marker.Modes.Attack;
					markers.Add(newMark);
				}
			}
		}

		return;
	}

	public void FireTorpedo(Vector3 pos){
		GameObject torp = (GameObject)Instantiate (torpedoPrefab);
		torp.transform.position = transform.position;
		torp.transform.rotation = Quaternion.LookRotation (pos - transform.position);
		torp.GetComponent<Bullet>().target = pos;

		Monster mon = Monster.Find (pos);
		if (mon != null) {
			mon.Damage (torpedos);
		}
	}
	
	public void Deselect(){
		if (markers.Count==0)	return;
		foreach(Marker mark in markers){
			Destroy(mark.gameObject);
		}
		markers.Clear();
	}

	Building InBuilding(){
		foreach(Building build in Building.buildings){
			if ((position-build.transform.position).sqrMagnitude<1)		return build;
		}
		return null;
	}

	bool ValidMovePos(Vector3 pos){
		if (fuel <= 0)	return false;
		if (Find(pos)!=null)	return false;
		if (Monster.Find(pos)!=null)	return false;
		if (tunnelers>0)	return true;
		if (Tunnel.Find(position, pos, normal)!=null)	return true;
		if (Tunnel.Find(position, pos, -normal)!=null)	return true;
		return false;
	}

	bool ValidMoveBuilding(Vector3 pos){
		if (fuel <= 0)	return false;
		if (Find(pos)!=null)	return false;
		if (Monster.Find(pos)!=null)	return false;
		if (tunnelers>0)	return true;
		if (Tunnel.Find(position, pos)!=null)	return true;
		if (Tunnel.Find(position, pos)!=null)	return true;
		return false;
	}

	Train Find(Vector3 pos){
		foreach(Train train in trains){
			if ((train.position-pos).sqrMagnitude<1)	return train;
		}
		return null;
	}

	public void Window(){
		GUILayout.BeginHorizontal ();

		GUILayout.BeginVertical(GUILayout.Width(200));
		GUILayout.FlexibleSpace();
		GUILayout.Box (name);
		GUILayout.Box ("Health: "+Mathf.Floor(health*100).ToString()+"%");
		GUILayout.Box ("Fuel: "+Mathf.Floor(fuel/maxfuel*100).ToString()+"%");
		GUILayout.Box ("Moves: " +moves.ToString()+ " / " + totalMoves.ToString());
		if (builders>0){
			if (GUILayout.Button("Build Colony")){
				Building build = ((GameObject)Instantiate(buildingPrefab)).GetComponent<Building>();
				build.transform.position = position;
				Selector.selected = build.gameObject;
				builders--;
				for (int i=0; i<carriages.Length; i++) {
					if(carriages[i].mode==Carriage.Mode.Builder){
						carriages[i].mode = Carriage.Mode.Spent;
						break;
					}
				}
			}
		}
		GUILayout.EndVertical();

		GUILayout.BeginVertical(GUILayout.Width(150));
		GUILayout.FlexibleSpace();
		for (int i=0; i<carriages.Length; i++) {
			if (carriages[i]!=null){
				GUILayout.Box(carriages[i].mode.ToString());
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal ();
	}
}