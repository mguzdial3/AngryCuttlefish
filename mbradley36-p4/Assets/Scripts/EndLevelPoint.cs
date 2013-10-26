using UnityEngine;
using System.Collections;

public class EndLevelPoint : MonoBehaviour {

	void OnTriggerEnter(Collider other)
	{
		if(other.name =="SpawnIndicator")
		{
			Application.LoadLevel("LevelSelect");
		}
	}
}
