using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
	public enum Modes {Movement, Attack};
	public Modes currentMode = Modes.Movement;

	public Train train = null;

	void Start(){
		switch(currentMode){
		case Modes.Movement:	renderer.material.SetColor("_Color",Color.green);		break;
		case Modes.Attack:		renderer.material.SetColor("_Color",Color.red);			break;
		}
	}

	public void Select(){
		Selector.selected = train.gameObject;
		train.target = transform.position;
		train.moves--;
		train.Deselect();
	}
}