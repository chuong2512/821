/*
DisableNextLevel.cs
Version 1.0

This will disable the NextLevel button if it's the last level.
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisableNextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		int LN = LevelData.getCurrentLevelNum();

		if (LevelData.getSceneName(LN+1) == "LevelMenu")
		{
			GetComponent<Button>().interactable = false;

			Text text = GetComponentInChildren<Text>();
			text.color = new Color(0f,0f,0f);
		}
	}
	

}
