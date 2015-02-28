using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {
	public Minerals.Ores body = Minerals.Ores.Steel;
	public float health = 1;

	public enum Mode {Tunneler, Miner, Colonist, Assault};
	public Mode mode = Mode.Tunneler;

	public void Window(){
		GUILayout.Box (name);
		GUILayout.Box ("Health "+Mathf.Floor(health*100).ToString());
		GUILayout.Box (mode.ToString ());
	}
}