﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {
	public static List<Building> buildings = new List<Building>();
	float health = 1;
	float population = 100;
	float armour = 25;

	void Start(){
		buildings.Add(this);
		population = Random.Range(300,500);
	}

	public void Window(){
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical(GUILayout.Width(200));
		GUILayout.FlexibleSpace();
		GUILayout.Box (name);
		GUILayout.Box ("Health "+Mathf.RoundToInt(health*100).ToString()+"%");
		GUILayout.Box ("Pop "+Mathf.Round(population).ToString());
		if (GUILayout.Button("Build Train")){
			Player.currentGameMode = Player.GameMode.Design;
		}
		GUILayout.EndVertical();
		
		GUILayout.BeginVertical(GUILayout.Width(200));
		GUILayout.FlexibleSpace();
		foreach(Train train in Train.trains){
			if (train!=null){
				if ((train.transform.position-transform.position).sqrMagnitude<1){
					if (GUILayout.Button(train.name+" "+Mathf.RoundToInt(train.health*100)+"% "+train.moves+"/"+train.totalMoves)){
						Selector.selected = train.gameObject;
					}
				}
			}
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
	}

	public void NextTurn(){
		population += Random.Range(100,200);
		//health = Mathf.Clamp01(health+0.1f);
		Player.wealth += population/10f;
	}

	public void Damage(float dam){
		health -= dam / armour;
	}
}