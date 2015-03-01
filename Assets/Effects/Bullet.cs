using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public Vector3 target = Vector3.zero;
	public float speed = 20;
	public GameObject hitEffect;

	void Update(){
		transform.rotation = Quaternion.LookRotation (target - transform.position);

		float vel = speed * Time.deltaTime;
		transform.Translate (0, 0, vel);
		if ((target - transform.position).sqrMagnitude < vel * vel * 2) {
			if (hitEffect!=null){
				GameObject hit = (GameObject)Instantiate(hitEffect);
				hit.transform.position = target;
			}
			Destroy(gameObject);
		}
	}
}