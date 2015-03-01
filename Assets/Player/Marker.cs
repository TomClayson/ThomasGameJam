using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
	public enum Modes {Movement, Attack};
	public Modes currentMode = Modes.Movement;

	public Train train = null;

	void Start(){
		switch(currentMode){
		case Modes.Movement:	renderer.material.SetColor("_Color",new Color(0,0.8f,0,0.6f));		break;
		case Modes.Attack:		renderer.material.SetColor("_Color",new Color(1,0,0,0.6f));			break;
		}
	}

	public void Select(){
		Selector.selected = train.gameObject;

		switch(currentMode){
		case Modes.Movement:
			train.target = transform.position;
			train.moves--;
			break;
		case Modes.Attack:
			train.FireTorpedo(transform.position);
			train.moves--;
			break;
		}


		train.Deselect();
	}
}