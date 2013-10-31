using UnityEngine;
using System.Collections;

public class DefaultPowerup : Powerup {

	public override void activatePowerup (GameObject other)
	{
		Destroy(other.GetComponent<CuttlefishShooter>());
		CuttlefishShooter r = other.AddComponent<CuttlefishShooter>();
		
		other.GetComponent<CuttlefishMovement>().shooter = r;
		
		Destroy(gameObject);
	}
}
