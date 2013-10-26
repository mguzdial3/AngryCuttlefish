using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public EnemyFish[] fishes;
	
	
	public void Spawn()
	{
		for(int i =0; i<fishes.Length; i++)
		{
			Instantiate(fishes[i],transform.position+Vector3.up*(transform.localScale.y/2)*Random.Range(-1f,1f),transform.rotation);
		}
		
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.name =="SpawnIndicator")
		{
			Spawn();
			Destroy(gameObject);
		}
	}
}
