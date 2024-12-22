/*
ChangePageButton

Changes the Page in the LevelMenu.
*/

using UnityEngine;
using System.Collections;

public class ChangePageButton : MonoBehaviour 
{

    /// <summary>
    /// The LevelMenu
    /// </summary>
    private LevelMenu lM;

    /// <summary>
    /// determines how much we will change the LevelMenu Page
    /// </summary>
    public int pageDelta = 1;

	// Use this for initialization
	void Start () 
    {
	    lM = GameObject.FindObjectOfType<LevelMenu>();

        //we cannot change contine if there is no LevelMenu
        if (lM == null)
        {
            Debug.LogError("No LevelMenu Exists.");
            return;
        }
	}

    //when the button is pressed we'll tell the LevelMenu to change the page
    public void Press()
    {
        lM.ChangePage(pageDelta);
    }
}
