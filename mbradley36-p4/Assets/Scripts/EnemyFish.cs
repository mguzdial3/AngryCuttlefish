using UnityEngine;
using System.Collections;

public class EnemyFish : MonoBehaviour {
	
	private GameObject player;
	private Vector3 currVector;
	public float TIMER_MAX = 1.0f;
	private float timer;
	public float speed = 5;
	private Vector3 playerPos;
	
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		timer=TIMER_MAX*2;
	}
	
	void Update()
	{
		if(timer<TIMER_MAX)	
		{
			timer+=Time.deltaTime*Random.Range(0.5f,0.9f);
		}
		else
		{
			timer=0;
			
			playerPos= player.transform.position;
			//currVector = player.transform.position-transform.position;
			//currVector.Normalize();
			
		}
		
		
		currVector = playerPos-transform.position;
		currVector.Normalize();
		
		transform.position+=currVector*Time.deltaTime*speed;
	}
	
	/**
	public GameObject thisFish;
	public int thisDirection;
	private static int fromTop = 0;
	private static int fromLeft = 1;
	private static int fromRight = 2;

	private bool debug = false;

	// move based on direction enemy is coming from
	public bool MoveAndDestroy() {
		bool enemyDestroyed = false;
		if (debug) Debug.Log("direction: " + thisDirection);
		if (debug) Debug.Log("y pos: " + thisFish.transform.localPosition.y + "x pos: " + thisFish.transform.localPosition.y);
		if (thisDirection == fromTop) {
			if (thisFish.transform.localPosition.y < -11.3) enemyDestroyed = true;
			thisFish.transform.Translate(Vector3.down*Time.deltaTime*5);
		} else if (thisDirection == fromLeft) {
			if (thisFish.transform.localPosition.x > 7.5) enemyDestroyed = true;
			thisFish.transform.Translate(Vector3.right*Time.deltaTime*5);
		} else if (thisDirection == fromRight) {
			if (thisFish.transform.localPosition.x < -7.5) enemyDestroyed = true;
			thisFish.transform.Translate(Vector3.left*Time.deltaTime*5);
		}

		if (debug) Debug.Log("move fn finished. " + enemyDestroyed + " will be destroyed");
		return enemyDestroyed;
	}
	*/

}