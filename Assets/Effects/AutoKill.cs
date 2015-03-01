using UnityEngine;
using System.Collections;

public class AutoKill : MonoBehaviour {
	public float life = 0;

	void Start(){
		Destroy (gameObject, life);
	}
}