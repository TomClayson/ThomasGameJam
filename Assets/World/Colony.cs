using UnityEngine;
using System.Collections;

public class Colony : MonoBehaviour {
	public void Window(){
		GUILayout.Box (name);

		GUILayout.Button("Tunneler");
		GUILayout.Button("Miner");
		GUILayout.Button("Colonist");
		GUILayout.Button("Assault");
	}
}