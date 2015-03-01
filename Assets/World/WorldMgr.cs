using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldMgr : MonoBehaviour {
	public GameObject tunnelPrefab = null;
	public GameObject mineralsPrefab = null;
	public GameObject monsterPrefab = null;
	
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
		for(int j=0; j<3; j++){
			Vector3 direction = Quaternion.Euler(Random.Range(0,4)*90, Random.Range(0,4)*90, Random.Range(0,4)*90) *transform.up;

			Vector3 pos = Vector3.zero;
			for(int i=0; i<3; i++){
				Vector3 oldPos = pos;
				Vector3 olddirect = direction;
				direction = Quaternion.Euler(Random.Range(-1,1)*90, Random.Range(-1,1)*90, Random.Range(-1,1)*90) *direction;
				pos += olddirect*10;
				if (direction!=olddirect)	pos += direction*10;
				MakeTunnel(olddirect, oldPos, pos);
			}
		}
	}

	public static Tunnel MakeTunnel(Vector3 direct, Vector3 start, Vector3 end){
		if (Tunnel.Find(start, end))	return null;
		Tunnel newTunnel = ((GameObject)Instantiate(local.tunnelPrefab)).GetComponent<Tunnel>();
		newTunnel.transform.parent = local.transform;
		newTunnel.SetPos(direct,start,end);
		return newTunnel;
	}

	public static void NextTurn(){
		//spawn minerals
		if (minerals.Count < Building.buildings.Count*5) {
			Vector3 pos = Building.buildings[Random.Range(0,Building.buildings.Count)].transform.position;
			pos += new Vector3(Random.Range(-5,5), Random.Range(-5,5), Random.Range(-5,5))*10;

			bool validMin = true;
			foreach(Minerals mineral in minerals){
				if ((mineral.transform.position-pos).sqrMagnitude<1){
					validMin=false;
					break;
				}
			}
			if (validMin){
				Minerals newMin = ((GameObject)Instantiate(local.mineralsPrefab)).GetComponent<Minerals>();
				newMin.transform.position = pos;
				newMin.ore = SelectOre(pos.y);
				newMin.amount = Random.Range(50,100);
				minerals.Add(newMin);
			}
		}

		//spawn monsters
		if (Monster.monsters.Count < Building.buildings.Count*2) {
			Vector3 pos = Building.buildings[Random.Range(0,Building.buildings.Count)].transform.position;
			pos += new Vector3(Random.Range(0,2)-0.5f, Random.Range(0,2)-0.5f, Random.Range(0,2)-0.5f)*500;
			
			bool validMin = true;
			foreach(Monster monster in Monster.monsters){
				if ((monster.transform.position-pos).sqrMagnitude<1){
					validMin=false;
					break;
				}
			}
			if (validMin){
				Monster newMin = ((GameObject)Instantiate(local.monsterPrefab)).GetComponent<Monster>();
				newMin.transform.position = pos;
				newMin.armour = Random.Range(1,Building.buildings.Count*2);
			}
		}
	}

	static Minerals.Ores SelectOre(float height){
		if (Random.value<0.05f)		return Minerals.Ores.Uranium;
		if (Random.value<0.05f)		return Minerals.Ores.Gold;
		if (Random.value<0.1f)		return Minerals.Ores.Lead;
		
		if (-height>Random.value*400+400){
			return Minerals.Ores.Diamond;
		}
		if (-height>Random.value*300+300){
			return Minerals.Ores.Tungsten;
		}
		if (-height>Random.value*200+200){
			return Minerals.Ores.Chromium;
		}
		if (-height>Random.value*100+100){
			return Minerals.Ores.Titanium;
		}
		
		switch(Random.Range(0,3)){
		case 0:		return Minerals.Ores.Steel;		break;
		case 1:		return Minerals.Ores.Copper;	break;
		case 2:		return Minerals.Ores.Aluminium;	break;
		}
		return Minerals.Ores.Copper;
	}
}