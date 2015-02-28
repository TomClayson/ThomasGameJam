using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tunnel : MonoBehaviour {
	public static List<Tunnel> tunnels = new List<Tunnel>();

	int life = 10;

	void Start(){
		tunnels.Add(this);
	}

	public void NextTurn(){
		life--;
		if (life<=0){
			Destroy(gameObject);
		}
	}

	public static Tunnel FindTunnel(Vector3 tunnelPos){
		foreach(Tunnel tunnel in tunnels)	if (tunnel!=null){
			if ((tunnel.transform.position-tunnelPos).sqrMagnitude<1)	return tunnel;
		}
		return null;
	}
}