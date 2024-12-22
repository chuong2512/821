/*
LevelTitle.cs
Version 1.0

Sets the Title of the Level
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTitle : MonoBehaviour {

	void Awake()
	{
		int LevelNum = LevelData.getCurrentLevelNum();
		GetComponent<Text>().text = "Level " + LevelNum.ToString();
	}

}
