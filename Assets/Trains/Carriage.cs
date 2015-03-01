using UnityEngine;
using System.Collections;

public class Carriage : MonoBehaviour {
	public enum Mode {Tunneler, Miner, Fuel, Torpedos, Cargo, Engine, Builder, Spent};
	public Mode mode = Mode.Engine;

	void Start(){
		name = mode.ToString();
	}
}