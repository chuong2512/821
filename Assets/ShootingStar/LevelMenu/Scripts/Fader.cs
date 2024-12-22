
#if UNITY_EDITOR
    using UnityEditor;
#endif
/*
Fader
Version 1.0

Fades out an image when the scene starts.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class Fader : MonoBehaviour 
{
    /// <summary>
    /// The this image.
    /// </summary>
    private Image ThisImage;

    /// <summary>
    /// The alpha of the image
    /// </summary>
    private float alpha = 1f;

    /// <summary>
    /// whether or not the fade done.
    /// </summary>
    private bool fadeDone = false;

    /// <summary>
    /// The speed of the fade out
    /// </summary>
    public float speed = 0.5f;

	// Use this for initialization
	void Start () 
    {
        //get a reference to the image component
        ThisImage = GetComponent<Image>();

        //set the color
        Color c = ThisImage.color ;
        ThisImage.color =  new Color(c.r,c.g,c.b,alpha);
	}

    void Update()
    {
        //if we are in the editor and it's not playing ...don't show the fader.
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            ThisImage = GetComponent<Image>();
            Color c = ThisImage.color ;
            ThisImage.color =  new Color(c.r,c.g,c.b,0f);
        }
#endif
    }

	
	void FixedUpdate () 
    {
        //fade out this image over time...

        if (!fadeDone)
        {
            alpha = Mathf.MoveTowards(alpha,0f,Time.deltaTime * speed);
            Color c = ThisImage.color ;
            ThisImage.color =  new Color(c.r,c.g,c.b,alpha);

            if (alpha < 0.01f)
            {
                ThisImage.color = new Color(c.r,c.g,c.b,0f);
                fadeDone = true;
            }
        }


	}
}
