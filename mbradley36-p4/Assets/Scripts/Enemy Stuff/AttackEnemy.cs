using UnityEngine;
using System.Collections;

//Basic type of attacking enemy
public class AttackEnemy : BasicEnemy {
	public float damageAmount; //The amount of damage this enemy does to the player
	private float timer; //Timer for movement
	private float timerMax = 1.0f; //Max amount of time for movement
	private Vector3 currDirection; //The current direction we're moving in
	
	public override void Start ()
	{
		base.Start (); //We need to have this to set up the basic info from Basic Enemy 
		currDirection = cuttlefish.transform.position -transform.position;
	}
	
	protected override void NormalGameplay ()
	{
		
		//Determine if we should change the currDirection yet
		if(timer<timerMax)
		{
			timer+=Time.deltaTime;
		}
		else
		{
			timer=0;
			currDirection = cuttlefish.transform.position -transform.position;
		}
		
		//Move this enemy
		transform.position+=currDirection.normalized*speedSeenPlayer*Time.deltaTime;
		
	}
	
	//What it should do when it hits something
	void OnTriggerEnter(Collider other)
	{
		//When it hits the player
		if(other.collider.tag == "Player")
		{
			//Do damage to cuttlefish
			cuttlefish.rageHandler.alterRage(-1*damageAmount*Time.deltaTime);
			//Push Cuttlefish away
			cuttlefish.transform.position+=rigidbody.velocity;
		}
	}
	
	//Change the BasicEnemy Escape, as attack enemies don't want to escape
	protected override void Escape ()
	{}
}
