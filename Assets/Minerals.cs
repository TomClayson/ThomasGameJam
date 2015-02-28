using UnityEngine;
using System.Collections;

public class Minerals : MonoBehaviour {
	public const int oresNumber = 6;
	public enum Ores {Steel, Gold, Uranium, Lead, Aluminium, Tungsten};
	//iron = basic
	//gold = money
	//uranium = power colonies nuclear bombs
	//lead = radiation resistent
	//aliuminium = speed
	//tungsten = heat restitent

	public Ores ore = Ores.Steel;
	public float amount = 0;

	void Start(){
		foreach (Transform child in transform) {
			child.transform.localPosition = Random.rotation*Vector3.up*Random.Range(1f,2f);
			child.transform.localScale = new Vector3( Random.Range(1f,2f), Random.Range(1f,2f), Random.Range(1f,2f));
			child.transform.localRotation = Random.rotation;
			child.renderer.material = WorldMgr.local.minMats[(int)ore];
		}
	}

	public void Window(){
		GUILayout.Box (ore.ToString ());
		GUILayout.Box (Mathf.RoundToInt (amount).ToString ());
	}
}