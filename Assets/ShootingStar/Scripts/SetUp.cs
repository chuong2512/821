/*
SetUp.cs
Version 1.0

Creates GameObjects on Awake...but don't create them if they exists.
*/

		

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SetUp : MonoBehaviour {

    [Header("Global Objects")]
    //list of Objects
    public List<GameObject> GlobalObjects = new List<GameObject>();

 
    void Awake()
    {
        foreach(GameObject O in GlobalObjects)
        {
            if (GameObject.Find (O.name) == null) //make sure it doesn't already exists
            {
                GameObject ThisO = Instantiate(O) as GameObject; //create object
                ThisO.name = ThisO.name.Replace("(Clone)",""); //removed the "(Clone)" from the name
            }
        }
    }




}
