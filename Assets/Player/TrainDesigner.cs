using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainDesigner : MonoBehaviour {
	public GameObject trainPrefab;
	public GameObject carriagePrefab;
	public Camera cam;
	public GameObject demoTrain;

	Minerals.Ores body = Minerals.Ores.Steel;
	List<Carriage> carriages = new List<Carriage>();
	Carriage.Mode lookAtCar = Carriage.Mode.Engine;

	float wealthCost = 0;
	float[] mineralCosts = new float[Minerals.oresNumber];

	float speed = 0;
	float armour = 0;
	float fuel = 0;
	float cargo = 0;

	void Start(){
		cam.gameObject.SetActive(false);
	}
	
	void Update(){
		if (Player.currentGameMode!=Player.GameMode.Design){
			cam.gameObject.SetActive(false);
			if (carriages.Count!=1){
				foreach(Carriage car in carriages){
					Destroy(car.gameObject);
				}
				carriages.Clear();
				
				Carriage ncar = ((GameObject)Instantiate(carriagePrefab)).GetComponent<Carriage>();
				ncar.mode = Carriage.Mode.Tunneler;
				ncar.transform.parent = demoTrain.transform;
				ncar.transform.localScale = Vector3.one*5f;
				ncar.gameObject.layer = 8;
				carriages.Add(ncar);
			}
			return;
		}

		//carriage manager
		for(int i=0; i<carriages.Count; i++){
			carriages[i].transform.localPosition = new Vector3(0,0,-i*10f);
			carriages[i].renderer.material = WorldMgr.local.minMats[(int)body];
		}

		//calc costs and stats
		wealthCost = 100;
		for(int i=0; i<mineralCosts.Length; i++)	mineralCosts[i]=0;
		wealthCost += 50 * carriages.Count;
		mineralCosts[(int)body] += 10*carriages.Count;

		speed = 0;
		for(int i=0; i<carriages.Count; i++){
			if (carriages[i].mode==Carriage.Mode.Engine)	speed += 20;
			if (carriages[i].mode==Carriage.Mode.Cargo)		cargo += 20;
			if (carriages[i].mode==Carriage.Mode.Fuel)		fuel += 5;
		}
		speed /= carriages.Count*Minerals.densities[(int)body];
		speed = Mathf.Clamp (speed, 1, 10);

		cam.gameObject.SetActive(true);
	}

	void OnGUI(){
		if (Player.currentGameMode!=Player.GameMode.Design)		return;

		GUI.skin = Interface.local.skin;
		GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
		GUILayout.BeginHorizontal();

		//body
		GUILayout.BeginVertical(GUILayout.Width(300));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Body Material");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(100));
		for(int i=0; i<Minerals.oresNumber; i++){
			if (GUILayout.Button(((Minerals.Ores)i).ToString())){
				body = (Minerals.Ores)i;
			}
		}
		GUILayout.EndVertical();
		string text = body.ToString()+"\n\n";
		switch(body){
		case Minerals.Ores.Steel:
			text+="Strong and resistent material with medium heat resistance.";
			break;
		case Minerals.Ores.Aluminium:
			text+="Being light, this allows trains to move faster than normal. However, it is weaker and has a very low heat resistance.";
			break;
		case Minerals.Ores.Gold:
			text+="Dazzle all your homies with a golden train that only an arabian prince would envy.";
			break;
		case Minerals.Ores.Lead:
			text+="By lining your trains they become much more radiation resistent and can survive easier nearer the surface. However, this makes them much heavier and thus slower.";
			break;
		case Minerals.Ores.Tungsten:
			text+="With an incredibly high melting point Tungsten helps protect against the heat lower underground. It is also very strong, but heavier and thus slower.";
			break;
		case Minerals.Ores.Uranium:
			text+="An interesting experiment... The crew may not last very long.";
			break;
		case Minerals.Ores.Copper:
			text+="Weak but cheaply available material.";
			break;
		case Minerals.Ores.Titanium:
			text+="Strong and high heat resistance while still being quite light.";
			break;
		case Minerals.Ores.Chromium:
			text+="Stronest and highest heat resistance of all materials. However, extremely rare.";
			break;
		case Minerals.Ores.Diamond:
			text+="Stronest and highest heat resistance of all materials. However, extremely rare.";
			break;
		}
		text += "\n";
		text += "\nTemperature: " + Minerals.MeltingPoints [(int)body].ToString ();
		text += "\nWeight: " + Minerals.densities [(int)body].ToString ();
		text += "\nArmour: " + Mathf.RoundToInt(Minerals.MeltingPoints [(int)body]/Minerals.densities [(int)body] ).ToString ();
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUILayout.Box(text);
		GUI.skin.box.alignment = TextAnchor.UpperCenter;
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		//add carriages
		GUILayout.BeginVertical(GUILayout.Width(300));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Add Carriage");
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(100));
		for(int i=0; i<6; i++){
			if (GUILayout.Button(((Carriage.Mode)i).ToString())){
				lookAtCar = (Carriage.Mode)i;

			}
		}
		GUILayout.EndVertical();
		GUILayout.BeginVertical();
		switch(lookAtCar){
		case Carriage.Mode.Miner:
			GUILayout.Box("Miner\n" +
			              "Station trains with mining units next to minerals to slowly harvest them.");
			break;
		case Carriage.Mode.Tunneler:
			GUILayout.Box("Tunneler\n" +
			              "Necessary to create new tunnels in to the rock.");
			break;
		case Carriage.Mode.Cargo:
			GUILayout.Box("Cargo\n" +
			              "Increases train cargo capacity, allowing it to carry more minerals.");
			break;
		case Carriage.Mode.Engine:
			GUILayout.Box("Engine\n" +
			              "Vital compoonent that drives your train. Add more to increase train speed.");
			break;
		case Carriage.Mode.Fuel:
			GUILayout.Box("Fuel\n" +
			              "Additional fuel tanks to allow trains to last longer away from home.");
			break;
		case Carriage.Mode.Torpedos:
			GUILayout.Box("Torpedos\n" +
			              "Strong weapons for defending trains and destroying others.");
			break;
		}
		if (GUILayout.Button("Add")){
			Carriage ncar = ((GameObject)Instantiate(carriagePrefab)).GetComponent<Carriage>();
			ncar.mode = lookAtCar;
			ncar.transform.parent = demoTrain.transform;
			ncar.transform.localScale = Vector3.one*5f;
			ncar.gameObject.layer = 8;
			carriages.Add(ncar);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		//carriage list
		GUILayout.BeginVertical(GUILayout.Width(150));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Train Front");
		for(int i=0; i<carriages.Count; i++){
			if (GUILayout.Button(carriages[i].name)){
				Destroy(carriages[i].gameObject);
				carriages.RemoveAt(i);
				break;
			}
		}
		GUILayout.Box("Train Rear");
		GUILayout.EndVertical();

		//cost
		GUILayout.BeginVertical(GUILayout.Width(150));
		GUILayout.FlexibleSpace();
		GUILayout.Box("Total Cost");
		GUILayout.Box("$ "+Mathf.Round(wealthCost).ToString());
		for(int i=0; i<mineralCosts.Length; i++) {
			if (mineralCosts[i]>0){
				GUILayout.Box (Mathf.RoundToInt(mineralCosts[i]).ToString()+" "+((Minerals.Ores)i).ToString());
			}
		}
		if (GUILayout.Button("BUILD")){
			Player.currentGameMode = Player.GameMode.Game;
			Train newTrain = ((GameObject)Instantiate(trainPrefab)).GetComponent<Train>();
			newTrain.health = 0.1f;
			newTrain.body = body;
			newTrain.transform.position = Selector.selected.transform.position;
			newTrain.totalMoves = (int)speed;

			newTrain.carriages = new Carriage[carriages.Count];
			for(int i=0; i<carriages.Count; i++){
				newTrain.carriages[i] = ((GameObject)Instantiate(carriagePrefab)).GetComponent<Carriage>();
				newTrain.carriages[i].transform.parent = newTrain.transform;
				newTrain.carriages[i].mode = carriages[i].mode;
			}
		}
		if (GUILayout.Button("Cancel")){
			Player.currentGameMode = Player.GameMode.Game;
		}
		GUILayout.EndVertical();

		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}
}