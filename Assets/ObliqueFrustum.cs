using UnityEngine;
using System.Collections;

public class ObliqueFrustum : MonoBehaviour {

	[Range(-1, 1)]
	public float horizontalOblique = 0f;

	[Range(-1, 1)]
	public float verticalOblique = 0f;



	void SetObliqueness(float horizObl, float vertObl) {
		Matrix4x4 mat  = Camera.main.projectionMatrix;
		mat[0, 2] = horizObl;
		mat[1, 2] = vertObl;
		Camera.main.projectionMatrix = mat;
	}



	public void Update(){
		SetObliqueness(horizontalOblique, verticalOblique);
	}
}
