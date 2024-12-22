/*
DemoScript
Version 1.0

This script is just used for the DemoLevel.

*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemoLevelScript : MonoBehaviour 
{

    public Text LevelNumTxt;

    public Text StarSliderLabel;
    public Slider StarSlider;

    public Text ScoreSliderLabel;
    public Slider ScoreSlider;

    public Toggle WinToggle;
    public Toggle UnLockNextLevel;

    void Awake()
    {
        int LevelNum =  PlayerPrefs.GetInt("currentLevelNum");

        LevelNumTxt.text = "LevelNum " + LevelNum.ToString();

        StarSlider.value = LevelData.getStarCount(LevelNum);
        ScoreSlider.value = LevelData.getScore(LevelNum);

        WinToggle.isOn = LevelData.isWon(LevelNum);

    }

    public void UpdateStarSlider()
    {
        StarSliderLabel.text = "Star Slider: " + StarSlider.value;
    }

    public void UpdateScoreSlider()
    {
        ScoreSliderLabel.text = "Score Slider: " + ScoreSlider.value;
    }

    public void GoToLevelMenu()
    {
        int LevelNum =  PlayerPrefs.GetInt("currentLevelNum");

        if (WinToggle.isOn)
        {
            LevelData.setWon(LevelNum,true);

            if (LevelData.getScore(LevelNum) < (int)ScoreSlider.value)
            {
                LevelData.setScore(LevelNum,(int)ScoreSlider.value);
            }

            if (LevelData.getStarCount(LevelNum) < (int)StarSlider.value)
            {
                LevelData.setStarCount(LevelNum,(int)StarSlider.value);
                LevelData.AnimateStars(LevelNum);
            }

            if (UnLockNextLevel.isOn)
            {
                LevelData.Unlock(LevelNum + 1);
            }
        }

        LevelData.goToLevelMenu();

    }




    public void GoToNextLevel()
    {
        int LevelNum =  PlayerPrefs.GetInt("currentLevelNum");
        int NextLevelNum = LevelNum +1;

        if (WinToggle.isOn)
        {
            LevelData.setWon(LevelNum,true);

            if (LevelData.getScore(LevelNum) < (int)ScoreSlider.value)
            {
                LevelData.setScore(LevelNum,(int)ScoreSlider.value);
            }

            if (LevelData.getStarCount(LevelNum) < (int)StarSlider.value)
            {
                LevelData.setStarCount(LevelNum,(int)StarSlider.value);
                LevelData.AnimateStars(LevelNum);
            }

            if (UnLockNextLevel.isOn)
            {
                LevelData.Unlock(NextLevelNum);
            }
        }


        LevelData.goToLevel(NextLevelNum); //go to Level automatically unlocks the Level
    }


    void FixedUpdate()
    {
        if (WinToggle.isOn)
        {
            ScoreSlider.interactable = true;    
            StarSlider.interactable = true;
            UnLockNextLevel.interactable = true;
        }
        else
        {
            ScoreSlider.interactable = false;    
            StarSlider.interactable = false;
            UnLockNextLevel.interactable = false;
        }
    }

}
