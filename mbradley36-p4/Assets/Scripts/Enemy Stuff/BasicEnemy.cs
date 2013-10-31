using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {
	//Link to cuttlefish
	protected CuttlefishMovement cuttlefish; //Reference to the player
	public float hitPoints=10f; //Max hitpoints of this fish
	public float pointsWorth = 1.0f; //The amount of rage points killing this fish gives you/takes away if you miss it
	public float speed, speedSeenPlayer; //Two different speeds
	
	//High level gameState stuff
	protected int gameState;
	protected const int NORMAL_STATE=0; //What the fish does normally
	protected const int ATTACK_STATE = 3; //If the fish has a different behavior when attacking
	protected const int PAUSED_STATE = 2; //If the fish should be paused
	
	// Use this for initialization
	public virtual void Start () {
		cuttlefish = GameObject.FindGameObjectWithTag("Player").GetComponent<CuttlefishMovement>();
	}
	
	// Update is called once per frame
	void Update () 
	{
			
		if(gameState==NORMAL_STATE)
		{
			NormalGameplay();
			
			if(checkIfOff())
			{
				Escape();
			}
			
			gameState = transferStateFromNormal();
			
		}
		else if(gameState==ATTACK_STATE)
		{
			AttackGameplay();
		}
		
		
		
	}
	
	//Determine if you should move into a different state from the normal
	protected virtual int transferStateFromNormal()
	{
		return 0;	
	}
	
	//Called if fish has different attack behavior
	protected virtual void AttackGameplay()
	{}
	
	//Normal fish behavior
	protected virtual void NormalGameplay()
	{
		float maxSpeed = 5.0f;
		
		transform.position+=Vector3.left*Time.deltaTime*(speed+maxSpeed*cuttlefish.rageHandler.getRatio());
	}
	
	
	
	//Checks if we're off the screen
	protected virtual bool checkIfOff()
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		
		//If we've made it off the screen
		if(screenPos.x<0)
		{
			return true;
		}
		
		return false;
		
	}
	
	
	//Called when the enemy goes off the screen to the left
	protected virtual void Escape()
	{
		cuttlefish.rageHandler.alterRage(-1*pointsWorth*Time.deltaTime);
		
		Destroy(gameObject);
	}
	
	//Called to do damage to this fish, calls OnDeath() if hitpoints too low
	public virtual void doDamage(float damage)
	{
		hitPoints-=damage;
		
		if(hitPoints<=0)
		{
			OnDeath();
		}
	}
	
	//What to do when this fish dies
	protected virtual void OnDeath()
	{
		cuttlefish.rageHandler.alterRage(pointsWorth*Time.deltaTime);
		
		Destroy(gameObject);
	}
	
	
}
