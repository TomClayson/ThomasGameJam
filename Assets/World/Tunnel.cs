using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tunnel : MonoBehaviour {
	public static List<Tunnel> tunnels = new List<Tunnel>();

	int life = 10;
	public Vector3 start = Vector3.zero;
	Vector3 normStart = Vector3.forward;
	public Vector3 end = Vector3.zero;
	Vector3 normEnd = Vector3.forward;

	void Start(){
		tunnels.Add(this);
	}

	public void NextTurn(){
		life--;
		if (life<=0){
			Destroy(gameObject);
		}
	}

	public static Tunnel Find(Vector3 A, Vector3 B, Vector3 norm){
		foreach(Tunnel tun in tunnels){
			if (tun!=null){
				if (tun.start==A && norm==tun.normStart && tun.end==B)		return tun;
				if (tun.start==B && tun.end==A && norm==tun.normEnd)		return tun;
			}
		}
		return null;
	}

	public static Tunnel Find(Vector3 A, Vector3 B){
		foreach(Tunnel tun in tunnels){
			if (tun!=null){
				if (tun.start==A && tun.end==B)		return tun;
				if (tun.start==B && tun.end==A)		return tun;
			}
		}
		return null;
	}

	public void SetPos(Vector3 direct, Vector3 newStart, Vector3 newEnd){
		start = newStart;
		end = newEnd;
		transform.position = start;
		LineRenderer line = GetComponent<LineRenderer>();
		direct.Normalize();

		if ((end-start).normalized==direct){
			for(int i=1; i<15; i++){
				line.SetPosition(i, direct*(float)i/14f*10);
			}
			normStart = direct;
			normEnd = -direct;
		}else{
			for(int i=1; i<15; i++){
				float angle = (float)i*3.1415f/2/14f;
				line.SetPosition(i, new Vector3(0,10-Mathf.Cos(angle)*10,Mathf.Sin(angle)*10));
			}
			transform.rotation = Quaternion.LookRotation(direct, end-(start+direct*10));
			normStart = direct;
			normEnd = transform.rotation * Vector3.down;
		}

		/*
		Vector3 dir = end-start;
		if ((dir.x==0 && dir.y==0) || (dir.x==0 && dir.z==0) || (dir.z==0 && dir.y==0)){
			for(int i=1; i<15; i++){
				line.SetPosition(i, start+dir*(float)i/15f);
			}
		}else{
			for(int i=1; i<15; i++){
				float angle = 3.1415f*(float)i/15f;
				line.SetPosition(i, new Vector3(Mathf.Sin(angle),Mathf.Cos(angle),0 ));
			}


			if ((start.x-Mathf.Round(start/10)*10)==5

			transform.rotation = Quaternion.Euler*/


			/*
			Vector3 origin = (start+end)/2f;
			origin.x = Mathf.Round(origin.x/10f)*10;
			origin.y = Mathf.Round(origin.y/10f)*10;
			origin.z = Mathf.Round(origin.z/10f)*10;
			origin -= start;
			Vector3 pos = start;
			Vector3 direct = Vector3.forward;
			if (dir.x==0)	direct = new Vector3(0,origin.z,origin.y);
			if (dir.y==0)	direct = new Vector3(origin.z,0,origin.x);
			if (dir.z==0)	direct = new Vector3(origin.y,origin.x,0);
			for(int i=1; i<15; i++){
				pos += direct * 5f*3.1415f/2f /15f;
				line.SetPosition(i, pos);
				direct = Quaternion.AngleAxis(90f/13f, 
			}
			*/
		//}
	}
}