using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {

	void OnGUI(){
		GUILayout.BeginArea (new Rect (0, 0, 200, Screen.height));
		//GUILayout.Box ("$ "+ Mathf.RoundToInt(Player.wealth).ToString());
		for(int i=0; i<Player.minerals.Length; i++) {
			GUILayout.Box (Mathf.RoundToInt(Player.minerals[i]).ToString()+" "+((Minerals.Ores)i).ToString());
		}
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect (Screen.width-200, 0, 200, Screen.height));
		GUILayout.Box ("Turn " + Player.turn.ToString ());
		if (GUILayout.Button ("Next Turn")) {
			Player.NextTurn();
		}

		GUILayout.EndArea ();
	}
}