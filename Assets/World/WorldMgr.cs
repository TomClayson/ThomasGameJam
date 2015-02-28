using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMgr : MonoBehaviour {
	public GameObject hubPrefab = null;
	public GameObject tunnelPrefab = null;
	public GameObject mineralsPrefab = null;

	public static List<Tunnel> tunnels = new List<Tunnel>();
	public static List<Transform> hubs = new List<Transform>();
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

				//check no hubs there
				bool validHub = true;
				foreach(Transform hub in hubs){
					if (hub.position==pos){
						validHub=false;
						break;
					}
				}
				if (validHub){
					Transform newHub = ((GameObject)Instantiate(hubPrefab)).transform;
					newHub.parent = transform;
					newHub.position = pos;
					hubs.Add(newHub);
				}

				//check no tunnels here
				Vector3 tunnelPos = pos-direction*5;
				bool validTunnel = true;
				foreach(Tunnel tunnel in tunnels){
					if (tunnel.transform.position==tunnelPos){
						validTunnel=false;
						break;
					}
				}
				if (validTunnel){
					Tunnel newTunnel = ((GameObject)Instantiate(tunnelPrefab)).GetComponent<Tunnel>();
					newTunnel.transform.parent = transform;
					newTunnel.transform.position = tunnelPos;
					newTunnel.transform.rotation = Quaternion.LookRotation(direction);
					tunnels.Add(newTunnel);
				}
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

	void Update(){
	}
}