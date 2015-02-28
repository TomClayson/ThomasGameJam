using UnityEngine;
using System.Collections;

public class Minerals : MonoBehaviour {
	public enum Ores {Iron, Gold, Uranium, Lead, Aluminium, Tungsten};
	//iron = basic
	//gold = money
	//uranium = power colonies nuclear bombs
	//lead = radiation resistent
	//aliuminium = speed
	//tungsten = heat restitent

	void Start(){
		foreach (Transform child in transform) {
			child.transform.localPosition = Random.rotation*Vector3.up*Random.Range(1f,2f);
			child.transform.localScale = new Vector3( Random.Range(1f,2f), Random.Range(1f,2f), Random.Range(1f,2f));
			child.transform.localRotation = Random.rotation;
		}
	}
}