/*
BackgroundParalax
Version 1.0

Moves an object depending on value of the scrollBar.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BackgroundParalax : MonoBehaviour 
{

    /// <summary>
    /// The position of this object when the first page is in focus
    /// </summary>
    public Vector3 pos0;

    /// <summary>
    /// The position of this object when the last page is in focus
    /// </summary>
    public Vector3 pos1;

    /// <summary>
    /// The LevelMenu 
    /// </summary>
    private LevelMenu lM;

    /// <summary>
    /// the scroll bar length...don't worry about it
    /// </summary>
    private float sbl;

    /// <summary>
    /// determines how much this object will move when scrolling past the first or past the last page.
    /// 1 means it doesn't move at all and 1000 means it will move lot.
    /// </summary>
    [Range(1.0f, 1000.0f)]
    public float overPullFactor = 500f;

	// Use this for initialization
	void Start () 
    {
        lM = GameObject.FindObjectOfType<LevelMenu>();

        if (lM == null)
        {
            Debug.LogError("No LevelMenu Exists.");
            return;
        }

        sbl = 1f / lM.pageCount;

	}
	   

    void FixedUpdate()
    {
        //if there is no LevelMenu we can't continue!
        if (lM == null)
        {
            Debug.LogError("No LevelMenu Exists.");
            return;
        }

        //if the scrollbar is at zero we are at the first page
        if ( lM.thisScrollBar.value == 0f)
        {
            Vector3 v3 = new Vector3(pos0.x + ((sbl - lM.thisScrollBar.size) * overPullFactor),pos0.y,pos0.z); 

            if (
                lM.direction == LevelMenu.directions.Vertical_B2T
                || lM.direction == LevelMenu.directions.Vertical_T2B
                )
            {
                v3 = new Vector3(pos0.x ,pos0.y + ((sbl - lM.thisScrollBar.size) * overPullFactor),pos0.z); 
            }
            
            gameObject.transform.localPosition = v3;      
        }
        //if the scrollbar is at one we are at the last page
        else if ( lM.thisScrollBar.value == 1f)
        {
            Vector3 v3 = new Vector3(pos1.x - ((sbl - lM.thisScrollBar.size) * overPullFactor),pos1.y,pos1.z); 

            if (
                lM.direction == LevelMenu.directions.Vertical_B2T
                || lM.direction == LevelMenu.directions.Vertical_T2B
                )
            {
                v3 = new Vector3(pos1.x ,pos1.y - ((sbl - lM.thisScrollBar.size) * overPullFactor),pos1.z); 
            }

            gameObject.transform.localPosition = v3;
        }
        //else lerp to the correct position between position zero and position one
        else
        {
            gameObject.transform.localPosition = Vector3.Lerp(pos0,pos1,lM.thisScrollBar.value);
        }


    }



}
