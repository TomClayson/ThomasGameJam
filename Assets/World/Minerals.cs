using UnityEngine;
using System.Collections;

public class Minerals : MonoBehaviour {
	public TextMesh hoverText;
	public const int oresNumber = 10;
	public enum Ores {
		Steel,
		Gold,
		Uranium,
		Lead,
		Aluminium,
		Tungsten,
		Copper,
		Titanium,
		Chromium,
		Diamond
	};
	public static float[] MeltingPoints = new float[]{
		1370,
		1060,
		1400,
		330,
		660,
		3420,
		1090,
		1670,
		1910,
		4440
	};
	public static float[] densities = new float[]{
		8,
		20,
		19,
		10,
		3,
		19,
		9,
		4,
		7,
		3.5f
	};

	//iron = basic
	//gold = money
	//uranium = power colonies nuclear bombs
	//lead = radiation resistent
	//aliuminium = speed
	//tungsten = heat restitent

	public Ores ore = Ores.Steel;
	public float amount = 0;

	void Start(){
		for(int i=0; i<transform.childCount; i++){
			Transform child = transform.GetChild(i);
			if (child.name!="HoverText"){
				child.transform.localPosition = Random.rotation*Vector3.up*Random.Range(1f,2f);
				child.transform.localScale = new Vector3( Random.Range(1f,2f), Random.Range(1f,2f), Random.Range(1f,2f));
				child.transform.localRotation = Random.rotation;
				child.renderer.material = WorldMgr.local.minMats[(int)ore];
			}
		}
	}

	void Update(){
		hoverText.text = "          " + Mathf.FloorToInt (amount).ToString () + " " + ore.ToString ();
	}

	public void Window(){
		GUILayout.Box (ore.ToString ());
		GUILayout.Box (Mathf.RoundToInt (amount).ToString ());
	}
}