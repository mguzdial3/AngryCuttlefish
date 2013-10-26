// Miranda Bradley
// mbradley36
// CS 6457
// all audio from freesound.org
// bubble textures from http://www.jennifersheu.com/etc-projects/oceanus-4/edg-twitter-bubble-textures/
// background texture from http://dadrian.deviantart.com/art/Under-water-texture-169170730

using UnityEngine;
using System.Collections;

public class CuttlefishBehavior : MonoBehaviour {
	/**
	// movement property references
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
	public float gravity = 10.0F;
	public float bulletSpeed = 10.0F;

	// injured and normal cuttlefish materials
	public Material hurtMtrl;
	public Material normalMtrl;

	public GameObject mtrlBody;
	public GameObject mtrlHead;
	
	public Transform body;

	public int injuryCounter = 0;
	
	// could get these from the collider, but wanted to be able to change them
	public float cHeight = 4.5f; 
	public float cWidth = 2f; 
	private float inputRotate = 0f;
	// a small delta for making sure the avatar remains in contact with the object it collided with
	public float cDelta = 0.01f;

	// store initial positions for idle state
	private Vector3 initialBodyScale;
	private float initialEyeY;
	private Vector3 initialBubblesPosition;

	// audio
	public AudioClip beamSound;

	// bubble particles
	public ParticleSystem bubbleSystem;
	private int timeSinceTurned = 0;

	// our movement vector
	private Vector3 moveDirection = Vector3.zero;
	private int enemyTimer = 0;

	// ink beams and enemy fish
	public GameObject beam;
	public GameObject newFish;
	public ArrayList leftBeams = new ArrayList();
	public ArrayList rightBeams = new ArrayList();
	public ArrayList enemies = new ArrayList();

	public GameObject tentacleWhip;
	public GameObject cuttleBody;
	public GameObject cuttleEye;

	private bool injuredBool = false;

	// Animation references:  if you want to control the kids and do something with them
	int counter = 0;

	private int animState; // different states of animation
	const int idle = 0;
	const int shooting = 1;

	private int yRot = 0;
	private Quaternion goalRotationAngle; // for flips
	private bool facingRight = false;

	private int currChar; // 1 - light, 2 - heavy

	private bool debug = false; // for turning on/off debug log

	float delta;
	float delta2;

	// a simple start to Character controller capabilities
	bool bumpTop;
	bool bumpLeft;
	bool bumpRight;
	bool bumpBottom;
	bool isGrounded = false;
	// "depth" of the collider into the object collided with.  Assume it's only in the 4 principle directions 
	float leftD, rightD, topD, bottomD;
	// need to move the clearing of variables out of fixedUpdate since fixedUpdate appears
	// to be called more often then collisions and collisions seem NOT to happen every
	// frame!??
	bool initController = false;
	

	void Start (){

		counter = 0;
		bumpTop = bumpLeft = bumpRight = bumpBottom = false;
		leftD = rightD = topD = bottomD = 0f;
		isGrounded = false;
		initController = false;

		tentacleWhip.active = false;

		animState = idle;

		goalRotationAngle = Quaternion.Euler(0, yRot, 0);

		// set initial positions to return to in idle state
		initialBodyScale = cuttleBody.transform.localScale;
		initialEyeY = cuttleEye.transform.localScale.y;
		initialBubblesPosition = bubbleSystem.transform.localPosition;

		timeSinceTurned = 35;

	}
	
	// do you application logic, managing states and so on, in here.  These examples have no explicit
	// states, but you should consider adding some to keep the code better organized
	void Update() {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        // bullets
        if (Input.GetKey("space")) {
        	float x = transform.localPosition.x;
        	float y = transform.localPosition.y+0.15f;
        	//float y = transform.localPosition.y-inputRotate;
        	float z = transform.localPosition.z;
        	// add to appropriate array list
        	if (facingRight) {
        		rightBeams.Add(Instantiate(beam, new Vector3(x, y, z), Quaternion.identity));
        	} else {
        		leftBeams.Add(Instantiate(beam, new Vector3(x, y, z), Quaternion.identity));
        	}

        	animState = shooting;
        }

        // turn around
        if(Input.GetKeyDown("v")) {
        	yRot += 180;
        	goalRotationAngle = Quaternion.Euler(0, yRot, 0);
			facingRight = !facingRight;
			timeSinceTurned = 0;
        }

        // adjust particles location
        if (facingRight) {
        	bubbleSystem.transform.localPosition = new Vector3(bubbleSystem.transform.localPosition.x, bubbleSystem.transform.localPosition.y, 0.6f);
        } else {
        	bubbleSystem.transform.localPosition = initialBubblesPosition;
        }

        // perform the actual rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, goalRotationAngle, (speed*4* Time.deltaTime));
        
        // release bubbles over time when we turn
        if (timeSinceTurned < 13) bubbleSystem.Emit(1);
        timeSinceTurned ++;

        // tentacle whip
        if (Input.GetKeyDown("b")) {
        	tentacleWhip.active = true;
        }

        if (Input.GetKeyUp("b")) {
        	tentacleWhip.active = false;
        }

		
        HandleBeams();
        HandleEnemies();

		//*******************************************************************************
		// alter these to just stop the character when reached screen edge
		// if we've bumped the top, and are moving upwards, stop the upward movement
		// also, move down "almost" out of whatever we colided with
		if (bumpTop && moveDirection.y > 0)
		{
			moveDirection.y = 0f;			
		}
		// if we've bumped the left or right, and are moving in that direction, stop the movement
		// also, move "almost" out of whatever we colided with
		if (bumpLeft && moveDirection.x < 0)
		{
			HandleColliding(); // directly called for speed					
		} else if (bumpRight && moveDirection.x > 0)
		{
			HandleColliding();							
		}
		// if we're on the ground, and are "inside" whatever we are on, move "almost" out.  If we are 
		if (isGrounded) 
		{
			// if below ground
			if (bottomD > 0)
			{
				
			}
		}

		if ( moveDirection.x == 0 && moveDirection.y == 0 ) { // set to idle if not moving
			if(animState != shooting) animState = idle;
		}

		Debug.Log("animation state: " +animState);
		switch ( animState ) { // determine which method to call
			case idle:		 HandleIdle();			break;
			case shooting:	 HandleShooting();		break;
		}

		// flash character back to normal after injury
		if (injuryCounter == 10) {
			injuredBool = false;
			injuryCounter = 0;
			Injured();
		}

		// after all the movement is computed ... move!
		transform.Translate(moveDirection * Time.deltaTime);
		counter++;
		enemyTimer++;
		injuryCounter++;
    }
	
    void FixedUpdate() {
		initController = true;
	}
	
	void InitController() {
		// reinitialize for checking for collisions.  FixedUpdate is called BEFORE any collisions.
		bumpTop = bumpLeft = bumpRight = bumpBottom = false;
		leftD = rightD = topD = bottomD = 0f;
		isGrounded = false;
		initController = false;
		if( debug ) Debug.Log ("Init Controller");
	}

	// a simple function that sets the left/right/top/bottom based on a single collision contact point.
	// the function also returns a boolean, indicating if we are "grounded", so that we can call the 
	// function from collisionStay as well as collisionEnter
	bool checkContactPoint (ContactPoint c)
	{			
		float dotUp = Vector3.Dot (c.normal, Vector3.up);
		float dotLeft = Vector3.Dot (c.normal, Vector3.left);
		Vector3 pt = transform.InverseTransformPoint(c.point);
		float ydiff = cHeight - Mathf.Abs (pt.y); 
		float xdiff = cWidth - Mathf.Abs (pt.x);
				
		//if( debug ) Debug.Log ("dots: " + dotUp + " " + dotLeft);
		if (dotUp < -0.5) {
			if (ydiff > topD) 
				topD = ydiff;
			bumpTop = true;
			if( debug ) Debug.Log ("Bumped with Top");
		}
		else if (dotUp > 0.5)
		{
			if (ydiff > bottomD)
				bottomD = ydiff;
			bumpBottom = true;
			if( debug ) Debug.Log ("Bumped with Bottom");
		}
		
		if (dotLeft > 0.5) 
		{
			if (xdiff > rightD)
				rightD = xdiff;
			bumpRight = true;
			if( debug ) Debug.Log ("Bumped with Right");
		}
		else if (dotLeft < -0.5)
		{
			if (xdiff > leftD)
				leftD = xdiff;
			bumpLeft = true;
			if( debug ) Debug.Log ("Bumped with Left");
		}
		
		// return if it's hit the bottom so we can check for grounded below
		return (dotUp > 0.5);
	}

	// Collision handling.  Update global variables for use in state machines.
	// DO NOT do any of the application logic associated with states here.  Just compute the 
	// various results of collisions, so that they can be used in Update once all the collisions 
	// are processed
	
	void OnCollisionEnter (Collision collision) {

		// see if this is the first time this is called for this loop through the 
		// collision routines
		if (initController) InitController();

		foreach (ContactPoint c in collision.contacts) {
            if (debug) Debug.Log(c.thisCollider.name + " COLLIDES WITH " + c.otherCollider.name);
            if (debug) Debug.Log("Collision: " + transform.InverseTransformPoint(c.point) + ", Normal: " + c.normal);
    		if (checkContactPoint(c))
			{
				isGrounded = true;			
				if (debug) Debug.Log ("Collision Enter GROUNDED");
			}

			// if we've collided in front and are attacking, destroy enemy
			Debug.Log("bump left");
	 		if (bumpLeft && tentacleWhip.active) {
	 			if (c.otherCollider.name.Equals("Sphere")) {
		 			Debug.Log("killed an enemy");
		 			GameObject killedEnemy = c.otherCollider.transform.parent.gameObject;
		 			enemies.Remove(killedEnemy);
		 			GameObject.Destroy(killedEnemy);
		 		}
	 		} else {
	 			injuredBool = true;
	 			Injured();
	 		}

	 		if (bumpRight || bumpTop || bumpBottom) {
	 			if (c.otherCollider.name.Equals("Sphere")) {
	 				injuredBool = true;
	 				Injured();
	 			}
	 		}

		}
	}

	void OnCollisionStay (Collision collision) {
		// see if this is the first time this is called for this loop through the 
		// collision routines
		if (initController) InitController();

		foreach (ContactPoint c in collision.contacts) {
            if (debug) Debug.Log(c.thisCollider.name + " STAY ON " + c.otherCollider.name);
            if (debug) Debug.Log("Collision: " + transform.InverseTransformPoint(c.point) + ", Normal: " + c.normal);
    		if (checkContactPoint(c))
			{
				isGrounded = true;			
				if (debug) Debug.Log ("Collision Stay GROUNDED");
			}
        }
	}
	
	void OnCollisionExit () {
		// see if this is the first time this is called for this loop through the 
		// collision routines
		if (initController) InitController();

		if( debug ) Debug.Log ("Collision Exit");
		
		// technically, there could be multiple simultaneous collisions (e.g., in a corner), so we should 
		// keep track of which ones are ending here
		isGrounded = false;
	}

	//------------------------------------------------------------------------------------------------//
	//  character injured
	//------------------------------------------------------------------------------------------------//
	void Injured() {
		// flash red when character is hit
		if (injuredBool) {
			mtrlBody.renderer.material = hurtMtrl;
			mtrlHead.renderer.material = hurtMtrl;
		} else {
			mtrlBody.renderer.material = normalMtrl;
			mtrlHead.renderer.material = normalMtrl;
		}

	}


	//------------------------------------------------------------------------------------------------//
	// beam logic
	//------------------------------------------------------------------------------------------------//
	void HandleBeams() {
		//*******************************************************************************
		// come back to this and make it so bullets know their velocity and move by it
        for (int i = 0; i < leftBeams.Count; i++) {
        	//keep moving bullets
        	GameObject thisBeam = (GameObject)leftBeams[i];
            thisBeam.transform.Translate(Vector3.left*Time.deltaTime*bulletSpeed);
        	//check if bullets are outside screen and destroy if they are
        	bool outsideScreen = false;
        	if (thisBeam.transform.localPosition.x < -8.6F || thisBeam.transform.localPosition.x > 8.6F) outsideScreen = true;
        	if (thisBeam.transform.localPosition.y < -6.4F || thisBeam.transform.localPosition.y > 6.4F) outsideScreen = true;
        	if (outsideScreen) {
        		leftBeams.Remove(thisBeam);
        		GameObject.Destroy(thisBeam);
        	}
        }

        for (int i = 0; i < rightBeams.Count; i++) {
        	//keep moving bullets
        	GameObject thisBeam = (GameObject)rightBeams[i];
            thisBeam.transform.Translate(Vector3.right*Time.deltaTime*bulletSpeed);
        	//check if bullets are outside screen and destroy if they are
        	bool outsideScreen = false;
        	if (thisBeam.transform.localPosition.x < -8.6F || thisBeam.transform.localPosition.x > 8.6F) outsideScreen = true;
        	if (thisBeam.transform.localPosition.y < -6.4F || thisBeam.transform.localPosition.y > 6.4F) outsideScreen = true;
        	if (outsideScreen) {
        		rightBeams.Remove(thisBeam);
        		GameObject.Destroy(thisBeam);
        	}
        }
	}

	//------------------------------------------------------------------------------------------------//
	// enemy logic
	//------------------------------------------------------------------------------------------------//
	void HandleEnemies () {
		if (enemyTimer == 60) {
        	enemyTimer = 0;

        	// determine a random position for the new enemy
	        float x = Random.Range(-7.2f, 6.5f);
			float y = Random.Range(-10f, -1.6f);
			int dir = (int)Random.Range(0, 3);
			// place the enemy based on the direction they're coming from
			if (dir == 0) {
				newFish = (GameObject)Instantiate(Resources.Load("EnemyCuttlefish"), new Vector3(x, 0, 0), Quaternion.identity);
			} else if (dir == 1) {
				newFish = (GameObject)Instantiate(Resources.Load("EnemyCuttlefish"), new Vector3(-7.6f, y, 0), Quaternion.identity);
			} else if (dir == 2) {
				newFish = (GameObject)Instantiate(Resources.Load("EnemyCuttlefish"), new Vector3(7, y, 0), Quaternion.identity);
			}

			// assign values in EnemyFish script
			EnemyFish thisEnemy = newFish.GetComponent<EnemyFish>();
        	thisEnemy.thisDirection = dir;
        	thisEnemy.thisFish = newFish;
        	enemies.Add(newFish);
        }


		for (int i=0; i<enemies.Count; i++) {
			if (!enemies[i].Equals("null")) {
				GameObject currentFish = (GameObject)enemies[i];
				EnemyFish thisEnemy = currentFish.GetComponent<EnemyFish>();
				if (thisEnemy.MoveAndDestroy()) {
					if (debug) Debug.Log("destroying below enemy");
					if (debug) Debug.Log("-----------------" + enemies[i] + "  " + i);
					enemies.Remove(enemies[i]);
					GameObject.Destroy(currentFish);
				}
			} else {
				enemies.Remove(enemies[i]);
			}
		}
	}

	//------------------------------------------------------------------------------------------------//
	// state handling
	//------------------------------------------------------------------------------------------------//
	void HandleIdle () {
		// set back to normal scale and position
		cuttleBody.transform.localScale = initialBodyScale;
		float squishedEyeY = cuttleEye.transform.localScale.y+0.05f;
		if (squishedEyeY > initialEyeY) squishedEyeY = initialEyeY;

		cuttleEye.transform.localScale = new Vector3(cuttleEye.transform.localScale.x, squishedEyeY, cuttleEye.transform.localScale.z);

		// randomly release bubbles
		int bubbleRelease = (int)Random.Range(0, 50);
		if (bubbleRelease == 15) {
			bubbleSystem.Emit(1);
		}

	}

	void HandleShooting () {
		// Shooting
		Debug.Log("scaling down");
		float squishedY = cuttleBody.transform.localScale.y-0.005f;
		float squishedEyeY = cuttleEye.transform.localScale.y-0.05f;
		if (squishedY < 0.85f) squishedY = 0.85f;
		if (squishedEyeY < 0.8f) squishedEyeY = 0.8f;
		cuttleBody.transform.localScale = new Vector3(cuttleBody.transform.localScale.x, squishedY, cuttleBody.transform.localScale.z);
		cuttleEye.transform.localScale = new Vector3(cuttleEye.transform.localScale.x, squishedEyeY, cuttleEye.transform.localScale.z);
		// play shooting sound
		audio.clip = beamSound;
 		if( !audio.isPlaying) {
 			if( debug ) Debug.Log("play collision");
 			audio.Play();
		}
		animState = idle;
	}

	void HandleColliding () {
		// for side collisions
		if ( debug ) Debug.Log("colliding");
		if ( bumpRight ) { 
			
	 	}
	 	else if ( bumpLeft ) {

	 	}
	}
	*/

}
