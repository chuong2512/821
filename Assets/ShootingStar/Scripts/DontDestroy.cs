/*
DontDestroy.cs
Version 1.0

This script will allow an object to live on after scene transition.
*/

using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		DontDestroyOnLoad(gameObject);
	}

}
