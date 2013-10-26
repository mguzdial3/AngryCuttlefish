using UnityEngine;
using System.Collections;

public class BasicEnemy : MonoBehaviour {
	//Link to cuttlefish
	protected CuttlefishMovement cuttlefish;
	public float hitPoints=10f;
	public float pointsWorth = 1.0f;
	public float speed, speedSeenPlayer;
	
	protected int gameState;
	protected const int NORMAL_STATE=0;
	protected const int ATTACK_STATE = 3;
	protected const int PAUSED_STATE = 2;
	
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
	
	protected virtual int transferStateFromNormal()
	{
		return 0;	
	}
	
	protected virtual void AttackGameplay()
	{}
	
	protected virtual void NormalGameplay()
	{
		float maxSpeed = 5.0f;
		
		transform.position+=Vector3.left*Time.deltaTime*(speed+maxSpeed*cuttlefish.rageHandler.getRatio());
	}
	
	
	
	
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
	
	
	protected virtual void Escape()
	{
		cuttlefish.rageHandler.alterRage(-1*pointsWorth*Time.deltaTime);
		
		Destroy(gameObject);
	}
	
	public virtual void doDamage(float damage)
	{
		hitPoints-=damage;
		
		if(hitPoints<=0)
		{
			OnDeath();
		}
	}
	
	protected virtual void OnDeath()
	{
		cuttlefish.rageHandler.alterRage(pointsWorth*Time.deltaTime);
		
		Destroy(gameObject);
	}
	
	
}
