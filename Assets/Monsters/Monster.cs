using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Monster : MonoBehaviour {
	public static List<Monster> monsters = new List<Monster>();
	public TextMesh hoverText;
	public float health = 1;
	public float armour = 5;

	public GameObject attackPrefab;

	void Start(){
		monsters.Add (this);
		transform.localScale = Vector3.one * armour/2f;
	}

	public void NextTurn(){
		GameObject target = null;

		float dist2 = 10e10f;
		foreach (Train train in Train.trains) {
			if (train!=null){
				float newDist = (train.transform.position-transform.position).sqrMagnitude;
				if (newDist<dist2){
					dist2 = newDist;
					target = train.gameObject;
				}
			}
		}
		foreach (Building build in Building.buildings) {
			if (build!=null){
				float newDist = (build.transform.position-transform.position).sqrMagnitude;
				if (newDist<dist2+10){
					dist2 = newDist;
					target = build.gameObject;
				}
			}
		}

		//random walk
		if (target == null) {
			Vector3 oldPos = transform.position;
			transform.position += Random.Range(-1,1)*10*Vector3.up;
			transform.position += Random.Range(-1,1)*10*Vector3.right;
			transform.position += Random.Range(-1,1)*10*Vector3.back;

/*			if (Tunnel.Find(oldPos, transform.position)==null){
				WorldMgr.MakeTunnel(transform.forward, oldPos, transform.position);
			}*/

			return;
		}

		Vector3 direct = target.transform.position - transform.position;

		if (direct.sqrMagnitude < 150) {
			GameObject newAttack = (GameObject)Instantiate(attackPrefab);
			newAttack.transform.position = transform.position;
			newAttack.transform.rotation = Quaternion.LookRotation(direct);
			newAttack.GetComponent<Bullet>().target = target.transform.position;

			if (target.GetComponent<Train>()!=null)		target.GetComponent<Train>().Damage(1);
			if (target.GetComponent<Building>()!=null)	target.GetComponent<Building>().Damage(1);
			return;
		}

		Vector3 gridDirect = direct.normalized;
		for(int i=0; i<6; i++){
			Vector3 dir = Vector3.up;
			switch(i){
			case 0:	dir = Vector3.up;		break;
			case 1:	dir = Vector3.down;		break;
			case 2:	dir = Vector3.right;	break;
			case 3:	dir = Vector3.left;		break;
			case 4:	dir = Vector3.forward;	break;
			case 5:	dir = Vector3.back;		break;
			}
			if ((dir-gridDirect).sqrMagnitude<1.1f){
				gridDirect = dir;
				break;
			}
		}
		transform.position += gridDirect * 10;
	}

	void Update(){
		hoverText.text = "     " + Mathf.FloorToInt (health * 100).ToString () + "%";
	}

	public void Damage(float dam){
		health -= dam / armour;
	}

	public static Monster Find(Vector3 pos){
		foreach(Monster monster in monsters){
			if ((monster.transform.position-pos).sqrMagnitude<1)	return monster;
		}
		return null;
	}
}