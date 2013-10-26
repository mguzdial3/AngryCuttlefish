using UnityEngine;
using System.Collections;

public class MovingTextureCheck : MonoBehaviour {
	public float scrollSpeed = 1.0f;
	public RageHandler rageHandler;
	public float maxExtraSpeed = 0.2f;
	
	// Update is called once per frame
	void Update () {
		float offset  = Time.time * (scrollSpeed+(maxExtraSpeed*rageHandler.getRatio()));
		renderer.material.SetTextureOffset ("_MainTex", new Vector2(offset,0));
	}
}
