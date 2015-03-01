using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainDesigner : MonoBehaviour {
	public GameObject trainPrefab;
	public Camera cam;
	public GameObject trainBody;

	Train.Mode mode = Train.Mode.Tunneler;

	Minerals.Ores body = Minerals.Ores.Steel;
	List<Train.Carriage> carriages = new List<Train.Carriage>();
	Train.Carriage lookAtCar = Train.Carriage.Tunneler;

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
		if (Player.currentGameMode!=Player.GameMode.Design){
			if (carriages.Count!=2){
				carriages.Clear();
				carriages.Add(Train.Carriage.Tunneler);
				carriages.Add(Train.Carriage.Engine);
			}
			return;
		}

		GUI.skin = Interface.local.skin;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();

		float wealthCost = 100;
		float[] mineralCosts = new float[Minerals.oresNumber];
		for(int i=0; i<mineralCosts.Length; i++)	mineralCosts[i]=0;
		mineralCosts[(int)Minerals.Ores.Steel] += 10;

		/*
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
		GUILayout.EndVertical();*/

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
//		string text = body.ToString()+
		switch(body){
		case Minerals.Ores.Diamond:
			GUILayout.Box("Daimond\n" +
			              "Stronest and highest heat resistance of all materials. However, extremely rare.");
			mineralCosts[(int)Minerals.Ores.Diamond] += 20;
			break;
		case Minerals.Ores.Titanium:
			GUILayout.Box("Titanium\n" +
			              "Strong and high heat resistance while still being quite light.");
			mineralCosts[(int)Minerals.Ores.Diamond] += 20;
			break;
		case Minerals.Ores.Chromium:
			GUILayout.Box("Chromium\n" +
			              "Stronest and highest heat resistance of all materials. However, extremely rare.");
			mineralCosts[(int)Minerals.Ores.Diamond] += 20;
			break;
		case Minerals.Ores.Steel:
			GUILayout.Box("Steel\n" +
			              "Strong and resistent material with medium heat resistance.");
			mineralCosts[(int)Minerals.Ores.Steel] += 20;
			break;
		case Minerals.Ores.Aluminium:
			GUILayout.Box("Aluminim\n" +
			              "Being light, this allows trains to move faster than normal. However, it is weaker" +
			              "and has a very low heat resistance..");
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
				              "It is also very strong, but heavier and thus slower.");
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

		//add carriages
		GUILayout.BeginVertical(GUILayout.Width(300));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Add Carriage");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(100));
		for(int i=0; i<6; i++){
			if (GUILayout.Button(((Train.Carriage)i).ToString())){
				lookAtCar = (Train.Carriage)i;

			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		switch(lookAtCar){
		case Train.Carriage.Miner:
			GUILayout.Box("Miner\n" +
			              "Station trains with mining units next to minerals to slowly harvest them.");
			break;
		case Train.Carriage.Tunneler:
			GUILayout.Box("Tunneler\n" +
			              "Necessary to create new tunnels in the rock. Must be at front or back!");
			break;
		case Train.Carriage.Cargo:
			GUILayout.Box("Cargo\n" +
			              "Increases train cargo capacity, allowing it to carry more minerals.");
			break;
		case Train.Carriage.Engine:
			GUILayout.Box("Engine\n" +
			              "Vital compoonent that drives your train. Add more to increase train speed.");
			break;
		case Train.Carriage.Fuel:
			GUILayout.Box("Fuel\n" +
			              "Additional fuel tanks to allow trains to last longer away from home.");
			break;
		case Train.Carriage.Torpedos:
			GUILayout.Box("Torpedos\n" +
			              "Strong weapons for defending trains and destroying others.");
			break;
		}
		if (GUILayout.Button("Add")){
			carriages.Add(lookAtCar);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		//carriage list
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();
		GUILayout.Box("Carriages");
		for(int i=0; i<carriages.Count; i++){
			if (GUILayout.Button(carriages[i].ToString())){
				carriages.RemoveAt(i);
				break;
			}
		}
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