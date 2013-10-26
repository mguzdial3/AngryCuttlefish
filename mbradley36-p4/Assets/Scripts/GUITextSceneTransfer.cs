using UnityEngine;
using System.Collections;

public class GUITextSceneTransfer : MonoBehaviour {
	bool mouseOn;
	public string levelName;
	// Update is called once per frame
	void Update () {
		if(mouseOn && Input.GetMouseButtonDown(0))
		{
			Application.LoadLevel(levelName);
		}
	}
	
	void OnMouseEnter()
	{
		guiText.fontStyle = FontStyle.Bold;
		mouseOn = true;
	}
	void OnMouseExit()
	{
		guiText.fontStyle = FontStyle.Normal;
		mouseOn = false;
	}
}
