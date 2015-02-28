using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static int turn = 1;

	public static float wealth = 1000;

	public static float[] minerals = new float[Minerals.oresNumber];

	public static void NextTurn(){
		turn++; 
	}
}