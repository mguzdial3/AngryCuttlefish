using UnityEngine;
using System.Collections;

public class CameraMoving : MonoBehaviour {
	public float speed = 5.0f;
	
	// Update is called once per frame
	void Update () {
		transform.position+=Vector3.right*Time.deltaTime*speed;
	}
}
