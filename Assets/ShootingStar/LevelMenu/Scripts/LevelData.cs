/*
LevelData
Version 1.0

Contains Methods to easily get/set PlayerPref Data for each level.
please let me know if there are any Methods missing

*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelData : MonoBehaviour {

    /// <summary>
    /// Gets the star count.
    /// </summary>
    /// <returns>The star count.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static int getStarCount(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefs.HasKey(PPLevelKey + "stars"))
        {
            return PlayerPrefs.GetInt(PPLevelKey + "stars");
        }
        else
        {
            PlayerPrefs.SetInt(PPLevelKey + "stars",0);
            return 0;
        }
    }

    /// <summary>
    /// Sets the star count.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="StarCount">Star count.</param>
    public static void setStarCount(int LevelNum, int StarCount)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefs.HasKey(PPLevelKey + "stars"))
        {

            if (getStarCount(LevelNum) < StarCount)
            {
                AnimateStars(LevelNum,true);
                PlayerPrefs.SetInt(PPLevelKey + "stars",StarCount);
            }
        }
        else
        {
            PlayerPrefs.SetInt(PPLevelKey + "stars",StarCount);
        }
    }

    /// <summary>
    /// Animates the stars.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="animate">If set to <c>true</c> animate.</param>
    public static void AnimateStars(int LevelNum, bool animate = true)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "animateStars"))
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "animateStars",animate);
        }
        else
        {
            Debug.LogWarning("Level doesn't Exists");
        }
    }

    /// <summary>
    /// Whether the star animation will be played
    /// </summary>
    /// <returns><c>true</c>, if animate stars was gotten, <c>false</c> otherwise.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static bool getAnimateStars(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "animateStars"))
        {
            return PlayerPrefsBool.GetBool(PPLevelKey + "animateStars");
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "animateStars",false);
            return false;
        }
    }

    /// <summary>
    /// Gets the score.
    /// </summary>
    /// <returns>The score.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static float getScore(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefs.HasKey(PPLevelKey + "score"))
        {
            return PlayerPrefs.GetFloat(PPLevelKey + "score");
        }
        else
        {
            PlayerPrefs.SetFloat(PPLevelKey + "score",0f);
            return 0;
        }
    }

    /// <summary>
    /// Sets the score.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="score">Score.</param>
    public static void setScore(int LevelNum, float score)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefs.HasKey(PPLevelKey + "score"))
        {
            PlayerPrefs.SetFloat(PPLevelKey + "score",score);
        }
        else
        {
            Debug.LogWarning("Level doesn't Exists");
        }
    }

    /// <summary>
    /// determine if the Level is Locked
    /// </summary>
    /// <returns><c>true</c>, if locked was ised, <c>false</c> otherwise.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static bool isLocked(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {
            return PlayerPrefsBool.GetBool(PPLevelKey + "isLocked");
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "isLocked",true);
            return true;
        }
    }

    /// <summary>
    /// Sets the lock to a level
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="Locked">If set to <c>true</c> locked.</param>
    public static void setLock(int LevelNum, bool isLocked)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {
            if (
                PlayerPrefsBool.GetBool(PPLevelKey + "isLocked")
                && isLocked == false
                )
            {
                AnimateUnlock(LevelNum,true);
            }

            PlayerPrefsBool.SetBool(PPLevelKey + "isLocked",isLocked);
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "isLocked",true);
        }
    }

    /// <summary>
    /// Unlock a specific level
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    public static void Unlock(int LevelNum)
    {
        setLock(LevelNum,false);
    }

    //Used to Unlock all the Levels
    public static void UnlockAllLevels()
    {
    	int LN = 1;
		string PPLevelKey = getPPLevelKey(LN);

		while(PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
    	{
    		Unlock(LN);

    		LN++;
			PPLevelKey = getPPLevelKey(LN);
    	}

    }

    /// <summary>
    /// Lock a specific level
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    public static void Lock(int LevelNum)
    {
        setLock(LevelNum,true);
    }

    /// <summary>
    /// Unlocks the next level...must be in a level
    /// </summary>
    public static void UnlockNextLevel()
    {
        int LevelNum = PlayerPrefs.GetInt("currentLevelNum") + 1;
        setLock(LevelNum,false);
    }

    /// <summary>
    /// Gets the PPLevelkey. (basically a prefix the all the keys stored in PlayerPrefs)
    /// </summary>
    /// <returns>The PP level key.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static string getPPLevelKey(int LevelNum)
    {
        return "Lvl" + LevelNum.ToString() + "_";
    }

    /// <summary>
    /// Animates the unlock.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="animate">If set to <c>true</c> animate.</param>
    public static void AnimateUnlock(int LevelNum, bool animate = true)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        PlayerPrefsBool.SetBool(PPLevelKey + "animateUnlock",animate);

    }

    /// <summary>
    /// Whether the lock animation will be played
    /// </summary>
    /// <returns><c>true</c>, if animate lock was gotten, <c>false</c> otherwise.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static bool getAnimateLock(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "animateUnlock"))
        {
            return PlayerPrefsBool.GetBool(PPLevelKey + "animateUnlock");
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "animateUnlock",false);
            return false;
        }
    }

    /// <summary>
    /// determine if the level has been won
    /// </summary>
    /// <returns><c>true</c>, if won was ised, <c>false</c> otherwise.</returns>
    /// <param name="LevelNum">Level number.</param>
    public static bool isWon(int LevelNum)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
        {
            return PlayerPrefsBool.GetBool(PPLevelKey + "isWon");
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "isWon",false);
            return false;
        }
    }

    /// <summary>
    /// sets whether or not the level has been won
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="Won">If set to <c>true</c> won.</param>
    public static void setWon(int LevelNum, bool Won)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);

        if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "isWon",Won);
        }
        else
        {
            PlayerPrefsBool.SetBool(PPLevelKey + "isWon",false);
        }
    }

    /// <summary>
    /// Gets the current level number.
    /// </summary>
    /// <returns>The current level number.</returns>
    public static int getCurrentLevelNum()
    {
        return PlayerPrefs.GetInt("currentLevelNum");
    }

    /// <summary>
    /// Go to level.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    /// <param name="ignoreLock">If set to <c>true</c> ignore lock.</param>
    public static void goToLevel(int LevelNum, bool ignoreLock = false)
    {
        string PPLevelKey = getPPLevelKey(LevelNum);
        string sceneName = "";

        if (PlayerPrefs.HasKey(PPLevelKey + "sceneName"))
        {
            sceneName = PlayerPrefs.GetString(PPLevelKey + "sceneName");
        }
        else
        {
            //if scene doesn't exist...go to LevelMenu
            sceneName = "LevelMenu";
            LevelNum = 0;
        } 

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {

            if (isLocked(LevelNum) && !ignoreLock)
            {
                Debug.LogWarning("This Level is Currently Locked");
            }
            else
            {
                PlayerPrefs.SetInt("currentLevelNum",LevelNum);
                SceneManager.LoadScene(sceneName);
            }

        }
        else
        {
            Debug.LogError("The scene '" + sceneName + "' cannot be loaded.");
        }
    }

    /// <summary>
	/// Allows us to change scene based on the sceneName
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    public static void goToScene(string sceneName)
    {
		if (Application.CanStreamedLevelBeLoaded(sceneName))
        {

			Time.timeScale = 1f;
            PlayerPrefs.SetInt("currentLevelNum",0);
            SceneManager.LoadScene(sceneName);

        }
        else
        {
            Debug.LogError("The scene '" + sceneName + "' cannot be loaded.");
        }
    }


	/// <summary>
	/// returns the screen name based on LevelNum
	/// </summary>
	/// <returns>The scene name.</returns>
	/// <param name="LevelNum">Level number.</param>
    public static string getSceneName(int LevelNum)
    {
		string PPLevelKey = getPPLevelKey(LevelNum);
        string Result = "";

		if (PlayerPrefs.HasKey(PPLevelKey + "sceneName"))
        {
			Result = PlayerPrefs.GetString(PPLevelKey + "sceneName");
        }
        else
        {
            //if scene doesn't exist...go to LevelMenu
			Result = "LevelMenu";
            LevelNum = 0;
        } 

		return Result;
    }

 
    /// <summary>
    /// Sets the current level number.
    /// </summary>
    /// <param name="LevelNum">Level number.</param>
    public static void setCurrentLevelNum(int LevelNum)
    {
		PlayerPrefs.SetInt("currentLevelNum",LevelNum);
    }

    /// <summary>
    /// go to level menu.
    /// </summary>
    public static void goToLevelMenu()
    {
		Time.timeScale = 1f;
        PlayerPrefs.SetInt("currentLevelNum",0);
        SceneManager.LoadScene("LevelMenu");
    }


    /// <summary>
    /// Resets the level data. (deletes PlayerPrefs for all LevelData)
    /// </summary>
    public static void ResetLevelData()
    {
//        int LevelNum = 1;
//        string PPLevelKey = getPPLevelKey(LevelNum);
//
//        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
//        {
//
//			PlayerPrefs.DeleteKey(PPLevelKey + "sceneName");
//
//            PlayerPrefs.DeleteKey(PPLevelKey + "score");
//            PlayerPrefs.DeleteKey(PPLevelKey + "stars");
//
//            PlayerPrefsBool.DeleteBool(PPLevelKey + "isWon");
//            PlayerPrefsBool.DeleteBool(PPLevelKey + "isLocked");
//
//            PlayerPrefsBool.DeleteBool(PPLevelKey + "animateStars");
//            PlayerPrefsBool.DeleteBool(PPLevelKey + "animateUnlock");
//
//            LevelNum++;
//            PPLevelKey = getPPLevelKey(LevelNum);
//        }

		string PPLevelKey = "";

		for(int i = 1; i < 1001; i++)
		{
			PPLevelKey = getPPLevelKey(i);

			if (PlayerPrefs.HasKey(PPLevelKey + "score"))
			{
				PlayerPrefs.DeleteKey(PPLevelKey + "score");
			}

			if (PlayerPrefs.HasKey(PPLevelKey + "stars"))
			{
				PlayerPrefs.DeleteKey(PPLevelKey + "stars");
			}

			if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
			{
				PlayerPrefsBool.DeleteBool(PPLevelKey + "isWon");
			}

			if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
			{
				PlayerPrefsBool.DeleteBool(PPLevelKey + "isLocked");
			}

			if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "animateStars"))
			{
				PlayerPrefsBool.DeleteBool(PPLevelKey + "animateStars");
			}

			if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "animateUnlock"))
			{
				PlayerPrefsBool.DeleteBool(PPLevelKey + "animateUnlock");
			}

			if (PlayerPrefs.HasKey(PPLevelKey + "sceneName"))
			{
				PlayerPrefs.DeleteKey(PPLevelKey + "sceneName");
			}
		}
    }

    /// <summary>
    /// Resets the level data. (deletes PlayerPrefs for all LevelData from level 1 to level "Upto")
    /// </summary>
    /// <param name="Upto">Upto.</param>
    public static void ResetLevelData(int Upto)
    {
        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (LevelNum <= Upto)
        {

            if (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
            {
                PlayerPrefs.DeleteKey(PPLevelKey + "score");
                PlayerPrefs.DeleteKey(PPLevelKey + "stars");

                PlayerPrefsBool.DeleteBool(PPLevelKey + "isWon");
                PlayerPrefsBool.DeleteBool(PPLevelKey + "isLocked");

                PlayerPrefsBool.DeleteBool(PPLevelKey + "animateStars");
                PlayerPrefsBool.DeleteBool(PPLevelKey + "animateUnlock");
            }

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }
    }

    /// <summary>
    /// Calculates the sum of all stars collected in all levels
    /// </summary>
    /// <returns>The sum.</returns>
    public static int StarSum()
    {   
        int result = 0;
        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefs.HasKey(PPLevelKey + "stars"))
        {
            result += PlayerPrefs.GetInt(PPLevelKey + "stars");

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }

        return result;
    }

    /// <summary>
    /// Calculates the sum of all the scores in all the levels
    /// </summary>
    /// <returns>The sum.</returns>
    public static float ScoreSum()
    {   
        float result = 0;
        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefs.HasKey(PPLevelKey + "score"))
        {
            result += PlayerPrefs.GetInt(PPLevelKey + "score");

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }

        return result;
    }

    /// <summary>
    /// Calculates the sum of all levels won
    /// </summary>
    /// <returns>The sum.</returns>
    public static int WinSum()
    {
        int result = 0;
        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isWon"))
        {
            result += (PlayerPrefsBool.GetBool(PPLevelKey + "isWon"))? 1: 0;

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }

        return result;
    }

    /// <summary>
    /// Unlocks the levels based on the StarSum...read this method and change for your own use
    /// </summary>
    public static void UnlockLevels_StarSum()
    {
        int Sum = StarSum();

        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {
            //Level will be unlocked if the StarSum is greater than of equal to the (LevelNum-1) * 2
            if (Sum >= ((LevelNum-1) * 2)) 
            {
                Unlock(LevelNum);
            }

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }
    }

    /// <summary>
    /// Unlocks the levels based on the ScoreSum...read this method and change for your own use
    /// </summary>
    public static void UnlockLevels_ScoreSum()
    {
        float Sum = ScoreSum();

        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {
            //Level will be unlocked if the StarSum is greater than of equal to the (LevelNum-1) * 80f
            //i think this score could be like a percent, but this can be adjusted based on how your game works
            if (Sum >= ((LevelNum-1) * 80f))
            {
                Unlock(LevelNum);
            }

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }
    }

    /// <summary>
    /// Unlocks the levels based on the WinSum...read this method and change for your own use
    /// </summary>
    public static void UnlockLevels_WinSum()
    {
        int Sum = WinSum();

        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {

            //Level will be unlocked if the WinSum + 1 is greater than of equal to the LevelNum
            if ( (Sum + 1) >= LevelNum)
            {
                Unlock(LevelNum);
            }

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }
    } 

    /// <summary>
    /// Gets the max unlocked level.
    /// </summary>
    /// <returns>The max unlocked level.</returns>
    public static int getMaxUnlockedLevel()
    {
        int result = 0;

        int LevelNum = 1;
        string PPLevelKey = getPPLevelKey(LevelNum);

        while (PlayerPrefsBool.HasBoolKey(PPLevelKey + "isLocked"))
        {

            if (
                !PlayerPrefsBool.GetBool(PPLevelKey + "isLocked")
                && LevelNum >= result
                )
            {
                result = LevelNum;
            }

            LevelNum++;
            PPLevelKey = getPPLevelKey(LevelNum);
        }

        return result;
    }
}
