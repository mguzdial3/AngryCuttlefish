using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	//What it should do when it hits something
	void OnTriggerEnter(Collider other)
	{
		//When it hits the player
		if(other.collider.tag == "Player")
		{
			activatePowerup(other.gameObject);
		}
	}
	
	
	//Set up cuttlefish to use new power
	public virtual void activatePowerup(GameObject other)
	{
		Destroy(other.GetComponent<CuttlefishShooter>());
		RandomShooter r = other.AddComponent<RandomShooter>();
		
		other.GetComponent<CuttlefishMovement>().shooter = r;
		
		Destroy(gameObject);
		
	}
	
}
