using UnityEngine;
using System.Collections;

public class Marker : MonoBehaviour {
	public Train train = null;

	public void Select(){
		Selector.selected = train.gameObject;
		train.target = transform.position;
		train.moves--;
		train.Deselect();
	}
}