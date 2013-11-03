using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public GameObject[] enemyPrefabs;
	public Transform top, bottom;
	
	
	public float timerMax = 4.0f;
	private float timer;
	public int maxNumEnemies = 4;
	
	void Update()
	{
		if(timer<timerMax)
		{
			timer+=Time.deltaTime*Random.Range(0.7f,0.9f);
		}
		else
		{
			timer=0;
			
			//Instantiate
			int enemyWaveType = Random.Range(0,enemyPrefabs.Length);
			int numEnemies = Random.Range(1,maxNumEnemies+1);
			
			Vector3 difference = bottom.position-top.position;
			
			for(int i =0; i<numEnemies; i++)
			{
				float amnt = ((float)i/(float)numEnemies)+Random.Range(0,0.5f);
				
				Vector3 pos = top.position+difference*amnt;
				
				Instantiate(enemyPrefabs[enemyWaveType],pos,transform.rotation);
			}
			
			
		}
	}
}
