/*
LevelButton
Version 1.0

1. Obtains data from the levelPicker when created.
2. Collect scores, stars, etc from the PlayerPrefs data storage.
3. Renders the button with the correct lock status, win status, scores, stars etc.
4. Goes to the scene that matches the scene name when pressed.

*/


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour 
{


//this is the Level Data
#region LevelData
    [Header("LevelData")]

    /// <summary>
    /// The level number.
    /// The value of this is set by the LevelPicker.cs
    /// </summary>
    public int levelNum;

    /// <summary>
    /// The name of the level, this can be anything you'd like.
    /// The value of this is set by the LevelPicker.cs
    /// </summary>
    public string levelName;

    /// <summary>
    /// The name of the scene, that will be loaded when the LevelButton is pressed
    /// The value of this is set by the LevelPicker.cs
    /// </summary>
    public string sceneName;

    /// <summary>
    /// whether or not this level has been won
    /// </summary>
    private bool isWon;

    /// <summary>
    /// whether or not this level is Locked
    /// </summary>
    private bool isLocked;

    /// <summary>
    /// The score that is recorded for this level
    /// </summary>
    private float score;

    /// <summary>
    /// The stars count that has been recorded for this level
    /// </summary>
    private int stars;

#endregion

//Options to how to render the LevelButton
#region Options
    [Header("Options")]
    /// <summary>
    /// Whether or not the LevelNum will be shown
    /// </summary>
    public bool showLevelNum = true;

    /// <summary>
    /// Whether or not the LevelName will be shown
    /// </summary>
    public bool showLevelName = false;

    /// <summary>
    /// Whether or not the WonToggle will be shown
    /// </summary>
    public bool showWonToggle = false;

    /// <summary>
    /// Whether or not the Stars will be shown
    /// </summary>
    public bool showStars = false;

    /// <summary>
    /// Whether or not the Score will be shown
    /// </summary>
    public bool showScore = true;

    [Header("options for score")]
    /// <summary>
    /// how many digits to round the score to
    /// </summary>
    public int roundScore = 1;

    /// <summary>
    /// The score prefix.
    /// </summary>
    public string scorePrefix = "";

    /// <summary>
    /// The score suffix.
    /// </summary>
    public string scoreSuffix = "%";

#endregion

//Components this script will alter
#region Components  
    private Text levelNumTxt;
    private Text levelNameTxt;
    private Toggle wonToggle;
    private Image lockImg;
    private Text scoreTxt;
    private GameObject starsGO;
#endregion

#region MonoMethod
	// Use this for initialization
	void Awake () 
    {
        SetUp();
	}
#endregion

#region Methods

    private void SetUp()
    {
        //get data from PlayerPrefs
        getPPData();

        //get the Components (Text,Images, etc)
        getComponents();

        //set the LevelNum
        if (levelNameTxt != null)
        {
            levelNumTxt.text = levelNum.ToString();
            levelNumTxt.gameObject.SetActive(showLevelNum);
        }
        else
        {
            Debug.LogWarning("LevelNum doesn't exist or doesn't have a Text Component");
        }

        //set the LevelName
        if (levelNameTxt != null)
        {
            levelNameTxt.text = levelName.ToString();
            levelNameTxt.gameObject.SetActive(showLevelName);
        }
        else
        {
            Debug.LogWarning("LevelName doesn't exist or doesn't have a Text Component");
        }

        //set the ScoreText
        if (scoreTxt != null)
        {
            scoreTxt.text = scorePrefix + score.ToString("F" + roundScore.ToString()) + scoreSuffix;
        }
        else
        {
            Debug.LogWarning("Score doesn't exist or doesn't have a Text Component");
        }

        //Won Toggle
        if (wonToggle != null)
        {
            wonToggle.isOn = isWon;
        }
        else
        {
            Debug.LogWarning("Won doesn't exist or doesn't have a Toggle Component");
        }

        //deactivate objects based on isLocked, and other bool(s)
        levelNumTxt.gameObject.SetActive(!isLocked);
        levelNameTxt.gameObject.SetActive(!isLocked);
        scoreTxt.gameObject.SetActive(!isLocked && showScore);
        wonToggle.gameObject.SetActive( (!isLocked && showWonToggle));
        starsGO.gameObject.SetActive( (!isLocked && showStars));

        //diable button if Level is Locked
        GetComponent<Button>().interactable = !isLocked;

        //set the Lock 
        SetLock();

        //set the Stars
        SetStars();

        //checks to see if the scene exists
        CheckScene();
    }

    //get data from PlayerPrefs
    private void getPPData()
    {
        string PPLevelKey = LevelData.getPPLevelKey(levelNum);

        score = LevelData.getScore(levelNum);
        stars = LevelData.getStarCount(levelNum);

        isWon = LevelData.isWon(levelNum);
        isLocked = LevelData.isLocked(levelNum);

        if (levelNum == 1)
        {
            LevelData.Unlock(levelNum);
            isLocked = false;
        }

        PlayerPrefs.SetString(PPLevelKey + "sceneName",sceneName);

    }

    //get the Components (Text,Images, etc)
    private void getComponents()
    {
        levelNumTxt = gameObject.transform.Find("LevelNum").GetComponent<Text>();

        levelNameTxt = gameObject.transform.Find("LevelName").GetComponent<Text>();
        scoreTxt = gameObject.transform.Find("Score").GetComponent<Text>();

        wonToggle = gameObject.transform.Find("Won").GetComponent<Toggle>();
        lockImg = gameObject.transform.Find("Lock").GetComponent<Image>();

        starsGO = gameObject.transform.Find("Stars").gameObject;

    }

    //set the Lock 
    private void SetLock()
    {
        if (lockImg == null)
        {
            Debug.LogWarning("Lock doesn't exist or doesn't have an image Component");
            return;
        }
        
        bool animateLock = LevelData.getAnimateLock(levelNum);

        //animate the Unlocking if "animateUnlock" is true and the level is not locked
        if (animateLock && !isLocked)
        {
            lockImg.gameObject.SetActive(true);
        }

        //set animator with bool values
        lockImg.gameObject.GetComponent<Animator>().SetBool("IsLocked",isLocked);
        lockImg.gameObject.GetComponent<Animator>().SetBool("Animate",animateLock);
        lockImg.gameObject.GetComponent<Animator>().SetBool("IsOn",true);


        LevelData.AnimateUnlock(levelNum,false);
    }

    //set the Stars
    private void SetStars()
    {
        if (starsGO == null)
        {
            Debug.LogWarning("Stars doesn't exist");
            return;
        }

        bool animateStars = LevelData.getAnimateStars(levelNum);

        //if the starGO is not active then return
        if (!starsGO.activeSelf)
        {
            return;
        }

        //active of deactivate stars based on the number of stars collected in the level
        for(int i  = 0; i < starsGO.transform.childCount; i++)
        {
            if (stars >= i+1)
            {
                starsGO.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                starsGO.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //set animator with bool values
        starsGO.GetComponent<Animator>().SetBool("Animate",animateStars);
        starsGO.GetComponent<Animator>().SetBool("IsOn",true);

        //set animate stars to false
        LevelData.AnimateStars(levelNum, false);
    }

    //checks to see if the scene can be loaded, if not display error
    private void CheckScene()
    {

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("The scene '" + sceneName + "' cannot be loaded.");
        }

    }

    //On button Press go to Level
    public void OnPress()
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
//            LevelData.goToLevel(levelNum);
			SceneSwitchAnimator.Instance.ChangeScene(levelNum, gameObject.transform.position);
        }
        else
        {
            Debug.LogError("The scene '" + sceneName + "' cannot be loaded.");
        }
    }

#endregion

}
