using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public bool debug = false;
	public GameObject Cuttlefish;
	public AudioClip enemyDeath;
	
	public Vector3 origPosition;
	public Vector3 mvmntVector = Vector3.right;
	public float speed =3.0f;
	public float maxDist=20;
	public RageHandler rageHandler;
	
	public float damage = 1.0f;
	
	private Color origColor; 
	
	void Start()
	{
		origColor = renderer.material.color;
		
		//Cheat for now to link damage and size/rage
		//damage = transform.localScale.x;
		if(rageHandler != null){
			damage = 1+rageHandler.getRatio();
		}
	}
	
	
	// Update is called once per frame
	void Update () {
		transform.position+=mvmntVector*Time.deltaTime*speed;
		
		float distTravelled = Mathf.Abs(origPosition.x-transform.position.x);
		
		if(distTravelled>maxDist)
		{
			Destroy(gameObject);
		}
		
		if(distTravelled>=maxDist*0.9f)
		{
			renderer.material.color = Color.Lerp(origColor,new Color(0,0,0,0),(distTravelled-maxDist*0.9f)/(maxDist*0.1f));
		}
		
		
		
	}

	// Collision handling.  Update global variables for use in state machines.
	// DO NOT do any of the application logic associated with states here.  Just compute the 
	// various results of collisions, so that they can be used in Update once all the collisions 
	// are processed
	void OnCollisionEnter (Collision collision) {

		foreach (ContactPoint c in collision.contacts) {
            //Debug.Log(c.thisCollider.name + " COLLIDES WITH " + c.otherCollider.name);
            //if( debug ) Debug.Log("Collision: " + transform.InverseTransformPoint(c.point) + ", Normal: " + c.normal);

	 		if (c.otherCollider.tag=="Enemy") {
	 			GameObject killedEnemy = c.otherCollider.gameObject;
				
				BasicEnemy be = killedEnemy.GetComponent<BasicEnemy>();
				
				if(be!=null)
				{
					be.doDamage(damage);
				}
				else
				{
					Destroy(gameObject);
				}

	 			audio.clip = enemyDeath;
		 		if( !audio.isPlaying) {
		 			//if( debug ) Debug.Log("play collision");
		 			//audio.Play();
				}
				
				Destroy(gameObject);
	 		}
		}
	}
	
	void setRageHandler(RageHandler rageH){
		rageHandler = rageH;
	}
}
