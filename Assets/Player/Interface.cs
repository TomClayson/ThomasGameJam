using UnityEngine;
using System.Collections;

public class Interface : MonoBehaviour {
	public GUISkin skin;
	public static Interface local;

	void Awake(){
		local = this;
	}

	void OnGUI(){
		GUI.skin = skin;
		GUILayout.BeginArea (new Rect (0, 0, 150, Screen.height));
		GUILayout.Box ("$ "+ Mathf.RoundToInt(Player.wealth).ToString());
		for(int i=0; i<Player.minerals.Length; i++) {
			GUILayout.Box (Mathf.RoundToInt(Player.minerals[i]).ToString()+" "+((Minerals.Ores)i).ToString());
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect (Screen.width-200, 0, 200, Screen.height));
		GUILayout.Box ("Turn " + Player.turn.ToString ());
		if (GUILayout.Button ("Next Turn")) {
			Player.NextTurn();

			foreach(Tunnel tunnel in Tunnel.tunnels)	tunnel.NextTurn();
			for(int i=Tunnel.tunnels.Count-1; i>=0; i--)
				if (Tunnel.tunnels[i]==null)	Tunnel.tunnels.RemoveAt(i);
			Tunnel.tunnels.TrimExcess();

			foreach(Train train in Train.trains)		train.NextTurn();
			for(int i=Train.trains.Count-1; i>=0; i--)
				if (Train.trains[i]==null)	Train.trains.RemoveAt(i);
			Train.trains.TrimExcess();

			foreach(Building build in Building.buildings)		build.NextTurn();
			for(int i=Building.buildings.Count-1; i>=0; i--)
				if (Building.buildings[i]==null)	Building.buildings.RemoveAt(i);
			Building.buildings.TrimExcess();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndArea ();

		if (Player.currentGameMode==Player.GameMode.Design)	return;

		if (Selector.selected != null) {
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.FlexibleSpace();
			if (Selector.selected.GetComponent<Building>()!=null)
				Selector.selected.GetComponent<Building>().Window();
			if (Selector.selected.GetComponent<Train>()!=null)
				Selector.selected.GetComponent<Train>().Window();
			if (Selector.selected.GetComponent<Minerals>()!=null)
				Selector.selected.GetComponent<Minerals>().Window();
			GUILayout.EndArea();
		}
	}
}