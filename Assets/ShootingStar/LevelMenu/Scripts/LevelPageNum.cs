/*
LevelPageNum
Version 1.0

Displays the number according to the current page that is being viewed.
*/



using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelPageNum : MonoBehaviour {

    /// <summary>
    /// The Reference to the LevelMenu
    /// </summary>
    private LevelMenu lM;

    /// <summary>
    /// The Text Component
    /// </summary>
    private Text levelPageNum;

    /// <summary>
    /// The prefix.
    /// </summary>
    public string prefix = "" ;

	// Use this for initialization
	void Start () 
    {
        lM = GameObject.FindObjectOfType<LevelMenu>();

        //stop here if the LevelMenu doesn't exist
        if (lM == null)
        {
            Debug.LogError("No LevelMenu Exists.");
            return;
        }

        //get a reference to the Text Component
        levelPageNum = gameObject.GetComponentInChildren<Text>();

        if (levelPageNum == null)
        {
            levelPageNum = gameObject.AddComponent<Text>();
        }

//        //Add this method to the delegate to execute when the LevelManager becomes Idel.
//        lM.startIdle += UpdateTxt;
//		Invoke("UpdateTxt",0.1f);
	}

	void FixedUpdate()
	{
		UpdateTxt();
	}

    void UpdateTxt()
    {
        //set the Text..including the prefix
        levelPageNum.text = prefix + lM.selectedLevelPage.ToString();
    }




}
