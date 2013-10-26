using UnityEngine;
using System.Collections;

public class RageHandler : MonoBehaviour {
	public float currRage = 0;
	public float maxRange = 100;
	
	public GUITexture rageMeter, rageMeterBack;
	public float divisorForRageMeter = 6;
	public float rageMeterWidthPercent = 0.8f;
	
	void Start()
	{
		float height = Screen.height/divisorForRageMeter;
		rageMeterBack.pixelInset = new Rect(0,Screen.height-height,Screen.width,height);
		rageMeterBack.color = Color.black;
		rageMeter.pixelInset = new Rect(Screen.width*(1f-rageMeterWidthPercent)/2f,Screen.height-height+(height*(1f-rageMeterWidthPercent)/2f),getCurrentRageMeterWidth(),height*rageMeterWidthPercent);
		rageMeter.color = Color.white;
		
	}
	
	public float getRatio()
	{
		return currRage/maxRange;
	}
	
	//Method to call to determine (returns current rage)
	public float alterRage(float rageAlteration)
	{
		currRage+=rageAlteration;
		
		
		if(currRage>0 && currRage<maxRange)
		{
			resetRageMeterVisual();
		}
		
		return currRage;
	}
	
	//Called to reset rage meter's appearance after a change 
	private void resetRageMeterVisual()
	{
		float height = Screen.height/divisorForRageMeter;
		rageMeter.pixelInset = new Rect(Screen.width*(1f-rageMeterWidthPercent)/2f
			,Screen.height-height+(height*(1f-rageMeterWidthPercent))
			,getCurrentRageMeterWidth(),
			height*rageMeterWidthPercent);
		rageMeter.color = getCurrRageMeterColor();
	
	}
	
	
	
	private float getCurrentRageMeterWidth()
	{
		return ((1f-((maxRange-currRage)/maxRange))*rageMeterWidthPercent*Screen.width)+5f;
	}
	
	
	private Color getCurrRageMeterColor()
	{
		float val = (1f-((maxRange-currRage)/maxRange));
		return Color.Lerp(Color.white,Color.red,val);
	}
	
	
}
