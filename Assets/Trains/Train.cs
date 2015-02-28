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

	public enum Mode {Tunneler, Miner, Colonist, Assault};
	public Mode mode = Mode.Tunneler;

	public void NextTurn(){
		moves = totalMoves;
	}

	public void Select(){
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
				markers.Add(newMark);
			}
		}

	}

	bool ValidMovePos(Vector3 pos){
		foreach (Tunnel tunnel in WorldMgr.tunnels) {
			if (tunnel.transform.position==pos)		return true;
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