using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMgr : MonoBehaviour {
	public GameObject tunnelPrefab = null;
	public GameObject mineralsPrefab = null;
	
	public static List<Minerals> minerals = new List<Minerals>();

	public Material[] minMats;
	public static WorldMgr local;

	void Awake(){
		local = this;
	}

	void Start(){
		Generate();
	}

	public void Generate(){
		for(int j=0; j<10; j++){
			Vector3 direction = Quaternion.Euler(Random.Range(0,4)*90, Random.Range(0,4)*90, Random.Range(0,4)*90) *transform.up;

			Vector3 pos = Vector3.zero;
			for(int i=0; i<5; i++){
				direction = Quaternion.Euler(Random.Range(-1,1)*90, Random.Range(-1,1)*90, Random.Range(-1,1)*90) *direction;
				pos += direction*10;
				MakeTunnel(pos-direction*5);
			}
		}

		//place minerals
		for (int i=0; i<5; i++){
			Vector3 pos = new Vector3(Random.Range(-2,2), Random.Range(-2,2), Random.Range(-2,2))*10;
			switch(Random.Range(0,3)){
			case 0:	pos.x += Random.Range(0,1)*10-5;	break;
			case 1:	pos.y += Random.Range(0,1)*10-5;	break;
			case 2:	pos.z += Random.Range(0,1)*10-5;	break;
			}
			bool validMin = true;
			foreach(Minerals mineral in minerals){
				if (mineral.transform.position==pos){
					validMin=false;
					break;
				}
			}
			if (validMin){
				Minerals newMin = ((GameObject)Instantiate(mineralsPrefab)).GetComponent<Minerals>();;
				newMin.transform.parent = transform;
				newMin.transform.position = pos;
				newMin.ore = (Minerals.Ores)Random.Range(0,Minerals.oresNumber);
				newMin.amount = Random.Range(100,200);
				minerals.Add(newMin);
			}
		}
	}

	public static Tunnel MakeTunnel(Vector3 tunnelPos){
		if (Tunnel.FindTunnel(tunnelPos))	return null;
		Tunnel newTunnel = ((GameObject)Instantiate(local.tunnelPrefab)).GetComponent<Tunnel>();
		newTunnel.transform.parent = local.transform;
		newTunnel.transform.position = tunnelPos;

		if (Mathf.Abs(tunnelPos.z - Mathf.Round(tunnelPos.z/10)*10)>1f) newTunnel.transform.rotation = Quaternion.Euler(0,0,0);
		if (Mathf.Abs(tunnelPos.y - Mathf.Round(tunnelPos.y/10)*10)>1f) newTunnel.transform.rotation = Quaternion.Euler(90,0,0);
		if (Mathf.Abs(tunnelPos.x - Mathf.Round(tunnelPos.x/10)*10)>1f) newTunnel.transform.rotation = Quaternion.Euler(0,90,0);
		return newTunnel;
	}

	void Update(){
	}
}