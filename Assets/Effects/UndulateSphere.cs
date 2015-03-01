using UnityEngine;
using System.Collections;

public class UndulateSphere : MonoBehaviour {
	Quaternion[] rots = new Quaternion[5];
	Vector3[] vertices;
	
	void Start(){
		for (int j=0; j<rots.Length; j++) {
			rots [j] = Random.rotation;
		}
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
	}

	void Update(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		int i = 0;
		Vector3[] newVerts = new Vector3[vertices.Length];
		while (i < vertices.Length) {
			//vertices[i] *= Mathf.Sin(vertices[i].x*5+vertices[i].y*7+vertices[i].z*11+0.754f)/10+0.9f;
			Vector3 newPos = vertices[i];
			for (int j=0; j<rots.Length; j++) {
				newPos += rots[j]*newPos;
			}
			newVerts[i] = vertices[i] * (Mathf.Sin(newPos.x*3+Time.time)/10+0.9f);
			i++;
		}
		mesh.vertices = newVerts;
		mesh.RecalculateBounds();
	}
}