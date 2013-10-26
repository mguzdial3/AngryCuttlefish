using UnityEngine;
using System.Collections;

public class AttackEnemy : BasicEnemy {
	public float damageAmount;
	private float timer;
	private float timerMax = 1.0f;
	private Vector3 currDirection;
	
	public override void Start ()
	{
		base.Start ();
		currDirection = cuttlefish.transform.position -transform.position;
	}
	
	protected override void NormalGameplay ()
	{
		
		if(timer<timerMax)
		{
			timer+=Time.deltaTime;
		}
		else
		{
			timer=0;
			currDirection = cuttlefish.transform.position -transform.position;
		}
		transform.position+=currDirection.normalized*speedSeenPlayer*Time.deltaTime;
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("every getting here?");
		
		if(other.collider.tag == "Player")
		{
			cuttlefish.rageHandler.alterRage(-1*damageAmount*Time.deltaTime);
			//Push Cuttlefish
			cuttlefish.transform.position+=rigidbody.velocity;
		}
	}
}
