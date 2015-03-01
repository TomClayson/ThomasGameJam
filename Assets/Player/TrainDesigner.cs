﻿using UnityEngine;
using System.Collections;

public class TrainDesigner : MonoBehaviour {
	public GameObject trainPrefab;
	public Camera cam;
	public GameObject trainBody;

	Train.Mode mode = Train.Mode.Tunneler;
	Minerals.Ores body = Minerals.Ores.Steel;

	void Start(){
		cam.gameObject.SetActive(false);
	}
	
	void Update(){
		if (Player.currentGameMode!=Player.GameMode.Design){
			cam.gameObject.SetActive(false);
			return;
		}

		cam.gameObject.SetActive(true);
	}

	void OnGUI(){
		if (Player.currentGameMode!=Player.GameMode.Design)		return;

		GUI.skin = Interface.local.skin;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();

		float wealthCost = 100;
		float[] mineralCosts = new float[Minerals.oresNumber];
		for(int i=0; i<mineralCosts.Length; i++)	mineralCosts[i]=0;
		mineralCosts[(int)Minerals.Ores.Steel] += 10;

		//mode
		GUILayout.BeginVertical(GUILayout.Width(300));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Train Function");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(100));
		for(int i=0; i<4; i++){
			if (GUILayout.Button(((Train.Mode)i).ToString())){
				mode = (Train.Mode)i;
			}
		}
		GUILayout.EndVertical();
		switch(mode){
		case  Train.Mode.Tunneler:
			GUILayout.Box("Tunneler\n" +
						"With a giant boring face, these trains can drill through dense rock quickly and create " +
						"tunnels for other trains. They have small amounts of armour.");
			break;
		case  Train.Mode.Miner:
			GUILayout.Box("Miner\n" +
			              "Built to harvest a variety of resources with their large drills and ample of cargo storage. With " +
			              "no armour, miners are easily lost in any battles.");
			break;
		case  Train.Mode.Builder:
			GUILayout.Box("Builder\n" +
			              "Able to establish new colonies to act as forward outposts or shelter the growing population. " +
			              "Construction a colony consumes this unit.");
			break;
		case  Train.Mode.Assault:
			GUILayout.Box("Assault\n" +
			              "By mounting heavy weapons onto trains, a new deadly weapon has been created, capable " +
			              "easily destroying other trains..");
			mineralCosts[(int)Minerals.Ores.Uranium] += 2;
			break;
		case  Train.Mode.Defender:
			GUILayout.Box("Defender\n" +
			              "Super stron heavy armour protects this train and those behing it against all but " +
			              "overwhelming forces.");
			mineralCosts[(int)Minerals.Ores.Steel] += 5;
			break;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		//body
		GUILayout.BeginVertical(GUILayout.Width(300));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Body Material");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(100));
		for(int i=0; i<Minerals.oresNumber; i++){
			if (GUILayout.Button(((Minerals.Ores)i).ToString())){
				body = (Minerals.Ores)i;
				trainBody.renderer.material = WorldMgr.local.minMats[i];
			}
		}
		GUILayout.EndVertical();
		switch(body){
		case Minerals.Ores.Steel:
			GUILayout.Box("Steel\n" +
			              "Very readily available and stil quite strong and resistent.");
			mineralCosts[(int)Minerals.Ores.Steel] += 20;
			break;
		case Minerals.Ores.Aluminium:
			GUILayout.Box("Aluminim\n" +
			              "Being light, this allows trains to move faster than normal. However, it is weaker than other materials.");
			mineralCosts[(int)Minerals.Ores.Aluminium] += 20;
			break;
		case Minerals.Ores.Gold:
			GUILayout.Box("Gold\n" +
			              "Dazzle all your homies with a golden train that only an arabian prince would envy.");
			mineralCosts[(int)Minerals.Ores.Gold] += 20;
			break;
		case Minerals.Ores.Lead:
			GUILayout.Box("Lead\n" +
			              "By lining your trains they become much more radiation resistent and can survive easier nearer the " +
			              "surface. However, this makes them much heavier and thus slower.");
			mineralCosts[(int)Minerals.Ores.Lead] += 20;
			break;
		case Minerals.Ores.Tungsten:
			GUILayout.Box("Tungsten\n" +
				              "With an incredibly high melting point Tungsten helps protect against the heat lower underground. " +
				              "It is also stronger than steel, but heavier and thus slower.");
			mineralCosts[(int)Minerals.Ores.Tungsten] += 20;
			break;
		case Minerals.Ores.Uranium:
			GUILayout.Box("Uranium\n" +
			              "An interesting experiment... The crew may not last very long.");
			mineralCosts[(int)Minerals.Ores.Uranium] += 20;
			break;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		//cost
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Box("Total Cost");
		GUILayout.Box("$ "+Mathf.Round(wealthCost).ToString());
		for(int i=0; i<mineralCosts.Length; i++) {
			GUILayout.Box (Mathf.RoundToInt(mineralCosts[i]).ToString()+" "+((Minerals.Ores)i).ToString());
		}
		if (GUILayout.Button("BUILD")){
			Player.currentGameMode = Player.GameMode.Game;
			Train newTrain = ((GameObject)Instantiate(trainPrefab)).GetComponent<Train>();
			newTrain.health = 0.1f;
			newTrain.body = body;
			newTrain.mode = mode;
			newTrain.transform.position = Selector.selected.transform.position;
		}
		if (GUILayout.Button("Cancel")){
			Player.currentGameMode = Player.GameMode.Game;
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}