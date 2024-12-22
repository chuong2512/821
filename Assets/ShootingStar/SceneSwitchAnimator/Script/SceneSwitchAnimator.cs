/*
SceneSwitchAnimator.cs
Version 1.0

This script manages the animation before and after the scene switchs.
*/

using UnityEngine;
using System.Collections;

public class SceneSwitchAnimator : MonoBehaviour {

	/// <summary>
	/// The animator
	/// </summary>
	private Animator Anim;

	/// <summary>
	/// The image...in this case a star
	/// </summary>
	private GameObject image; 

	//set up some references
	void Awake()
	{
		DontDestroyOnLoad(gameObject); //don't remove this on scene change
		Anim = GetComponent<Animator>();
		image = gameObject.transform.Find("Image").gameObject;
	}

	//some private variables
	private bool useLevelNum;
	private int LevelNum;
	private string SceneName = "LevelMenu";

	/// <summary>
	/// Changes the scene by passing in the levelNum
	/// </summary>
	/// <param name="levelNum">Level number.</param>
	public void ChangeScene(int levelNum)
	{
		//the image will appear in the middle of the screen
		ChangeScene(levelNum,new Vector3(Screen.width/2f,Screen.height/2f,0f));
	}

	/// <summary>
	/// Changes the scene by passing in the sceneName
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void ChangeScene(string sceneName)
	{
		//the image will appear in the middle of the screen
		ChangeScene(sceneName,new Vector3(Screen.width/2f,Screen.height/2f,0f));
	}

	/// <summary>
	/// Changes the scene by passing in the levelNum, and a custom image position
	/// </summary>
	/// <param name="levelNum">Level number.</param>
	/// <param name="imagePos">Image position.</param>
	public void ChangeScene(int levelNum, Vector3 imagePos)
	{
		image.transform.position = imagePos;

		useLevelNum = true;

		LevelNum = levelNum;


		Anim.SetTrigger("Play");
	}

	/// <summary>
	/// Changes the scene by passing in the sceneName, and a custom image position
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	/// <param name="imagePos">Image position.</param>
	public void ChangeScene(string sceneName, Vector3 imagePos)
	{
		image.transform.position = imagePos;

		useLevelNum = false;

		SceneName = sceneName;


		Anim.SetTrigger("Play");
	}


	/// <summary>
	/// this method is called in the middle of the animation to switch the scene
	/// </summary>
	public void CSN()
	{
		if (useLevelNum)
		{
			LevelData.goToLevel(LevelNum);
		}
		else
		{

			LevelData.goToScene(SceneName);
		}
	}

	//allows us to refer to this gameobject via a script with out referencing it in a variable
    #region SINGLETON PATTERN
	private static SceneSwitchAnimator _instance;
    
	public static SceneSwitchAnimator Instance
    {
        get {
            if (_instance == null)
            {
				_instance = GameObject.FindObjectOfType<SceneSwitchAnimator>();
                
                if (_instance == null)
                {
					_instance = Instantiate(Resources.Load("SceneSwitchAnimator", typeof(SceneSwitchAnimator))) as SceneSwitchAnimator;
					_instance.gameObject.name = _instance.gameObject.name.Replace("(Clone)",""); //removed the "(Clone)" from the name
                }
            }
            
            return _instance;
        }
    }
    #endregion
}
