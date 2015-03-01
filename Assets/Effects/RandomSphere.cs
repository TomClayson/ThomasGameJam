using UnityEngine;
using System.Collections;

public class RandomSphere : MonoBehaviour {
	void Start(){
		Quaternion[] rots = new Quaternion[5];
		for (int j=0; j<rots.Length; j++) {
			rots[j] = Random.rotation;
		}

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		int i = 0;
		while (i < vertices.Length) {
			//vertices[i] *= Mathf.Sin(vertices[i].x*5+vertices[i].y*7+vertices[i].z*11+0.754f)/10+0.9f;
			Vector3 newPos = vertices[i];
			for (int j=0; j<rots.Length; j++) {
				newPos += rots[j]*newPos;
			}
			vertices[i] *= Mathf.Sin(newPos.x*1000)/10+0.9f;
			i++;
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
	}
}