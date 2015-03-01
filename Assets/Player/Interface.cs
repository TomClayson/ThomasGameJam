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
			WorldMgr.NextTurn();
			Selector.selected = null;
			Player.NextTurn();

			for(int i=Tunnel.tunnels.Count-1; i>=0; i--){
				if (Tunnel.tunnels[i]==null)
					Tunnel.tunnels.RemoveAt(i);
				else
					Tunnel.tunnels[i].NextTurn();
			}
			Tunnel.tunnels.TrimExcess();

			for(int i=Train.trains.Count-1; i>=0; i--){
				if (Train.trains[i]==null)
					Train.trains.RemoveAt(i);
				else
					Train.trains[i].NextTurn();
			}
			Train.trains.TrimExcess();

			for(int i=Building.buildings.Count-1; i>=0; i--){
				if (Building.buildings[i]==null)
					Building.buildings.RemoveAt(i);
				else
					Building.buildings[i].NextTurn();
			}
			Building.buildings.TrimExcess();

			for(int i=Monster.monsters.Count-1; i>=0; i--){
				if (Monster.monsters[i]==null)
					Monster.monsters.RemoveAt(i);
				else
					Monster.monsters[i].NextTurn();
			}
			Monster.monsters.TrimExcess();

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