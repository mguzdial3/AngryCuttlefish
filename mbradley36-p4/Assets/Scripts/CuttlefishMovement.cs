using UnityEngine;
using System.Collections;

public class CuttlefishMovement : MonoBehaviour {
	
	// movement property references
    public float speed = 6.0F;
	private float origSpeedVal;
	public float turnSpeed = 6.0f;
	public float moveBackAmnt = 0.6f;
	
	//Beam to Instantiate
	public GameObject beam;
	//Place to instantiate beam from
	public Transform beamPosition;
	
	//Facing Left To Begin With
	private bool facingRight = false;
	
	private Quaternion goalRotation;
	private float minShrinkVal = 0.7f;
	
	public float angerBarHeight; 
	
	public RageHandler rageHandler;
	

	
	// Use this for initialization
	void Start () {
		goalRotation = Quaternion.Euler(new Vector3(0,180,0));
		facingRight = true;
		origSpeedVal = speed;
		
		angerBarHeight = Screen.height/rageHandler.divisorForRageMeter;
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = new Vector3(-1*Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
		float maxSpeedBoost = 5.0f;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= (speed+maxSpeedBoost*rageHandler.getRatio());
		
		
		
		
		if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)
			|| Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
		{
			if(speed<origSpeedVal*1.5f)
			{
				speed+=Time.deltaTime*4;
			}
		}
		else
		{
			speed=origSpeedVal;
		}
		
		
		
		
		if(transform.rotation!=goalRotation)
		{
			 transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, (turnSpeed*4* Time.deltaTime));
		}
		
		if(Input.GetKey(KeyCode.Space))
		{
			Vector3 difference = Vector3.right*Random.Range(-0.15f,0.15f)+Vector3.up*Random.Range(-0.15f,0.15f);
			
			float bulletSizeMax = 5.0f;
			Bullets b = (Instantiate(beam,beamPosition.transform.position+difference,transform.rotation) as GameObject).GetComponent<Bullets>();
			b.transform.localScale*=(1+rageHandler.getRatio()*bulletSizeMax);
			
			b.transform.parent = transform.parent;
			
			b.origPosition = beamPosition.transform.position;
			if(facingRight)
			{
				b.mvmntVector = Vector3.right;
				
				moveDirection-=(Vector3.left+-1*difference)*moveBackAmnt;
			}
			else
			{
				b.mvmntVector = Vector3.left;
				
				moveDirection+=(Vector3.right+-1*difference)*moveBackAmnt;
			}
			
			
			if(transform.localScale.y>minShrinkVal)
			{
				transform.localScale-=Vector3.up*Time.deltaTime*5;
			}
		}
		else
		{
			if(transform.localScale.y<1)
			{
				transform.localScale+=Vector3.up*Time.deltaTime*2;
			}
		}
		
		Vector3 newPos = transform.position+moveDirection * Time.deltaTime;
		
		Rect screenRect = new Rect(0,0,Screen.width,Screen.height-angerBarHeight);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(newPos);
		
		if(-1*Input.GetAxis("Horizontal")>0)
		{
			screenPos.x-=20;
		}
		else
		{
			screenPos.x+=20;
		}
		

		
		if(screenRect.Contains(screenPos))
		{
			transform.position = newPos;
		}
	}
}
