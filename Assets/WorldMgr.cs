using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMgr : MonoBehaviour {
	public GameObject hubPrefab = null;
	public GameObject tunnelPrefab = null;
	public GameObject mineralsPrefab = null;

	List<Transform> tunnels = new List<Transform>();
	List<Transform> hubs = new List<Transform>();
	List<Transform> minerals = new List<Transform>();

	void Start(){
		Generate();
	}

	public void Generate(){
		for(int j=0; j<5; j++){
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
					newHub.position = pos;
					hubs.Add(newHub);
				}

				//check no tunnels here
				Vector3 tunnelPos = pos-direction*5;
				bool validTunnel = true;
				foreach(Transform tunnel in hubs){
					if (tunnel.position==tunnelPos){
						validTunnel=false;
						break;
					}
				}
				if (validTunnel){
					Transform newTunnel = ((GameObject)Instantiate(tunnelPrefab)).transform;
					newTunnel.position = tunnelPos;
					newTunnel.rotation = Quaternion.LookRotation(direction);
					tunnels.Add(newTunnel);
				}
			}
		}

		//place minerals
		for (int i=0; i<5; i++){
			Vector3 pos = new Vector3(Random.Range(-2,2), Random.Range(-2,2), Random.Range(-2,2)) *10;
			bool validMin = true;
			foreach(Transform mineral in minerals){
				if (mineral.position==pos){
					validMin=false;
					break;
				}
			}
			if (validMin){
				Transform newMin = ((GameObject)Instantiate(mineralsPrefab)).transform;
				newMin.position = pos;
				minerals.Add(newMin);
			}
		}
	}

	void Update(){
		
	}
}