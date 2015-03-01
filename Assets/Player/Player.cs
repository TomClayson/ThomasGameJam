using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public static int turn = 1;
	public enum GameMode {Menu, Game, Design};
	public static GameMode currentGameMode = GameMode.Game;

	public static float wealth = 1000;

	public static float[] minerals = new float[Minerals.oresNumber];

	public static void Init(){
		wealth = 1000;
		minerals [(int)Minerals.Ores.Steel] = 100;
		minerals [(int)Minerals.Ores.Copper] = 100;
		minerals [(int)Minerals.Ores.Aluminium] = 50;
		minerals [(int)Minerals.Ores.Lead] = 50;
	}

	public static void NextTurn(){
		turn++;
	}
}