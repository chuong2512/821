/*
GameManager.cs
Version 1.0

This script manages the game's state.
Possible states include; StartGame, Play, Win, Reset, and Pause.
*/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

#region Variables
	/// <summary>
	/// Possible States
	/// </summary>
	public enum States
	{
		STARTGAME = 0,
		PLAY = 1,
		WIN = 2,
		RESET = 3,
		PAUSE = 4,
	}


	/// <summary>
	/// The time when the state switched
	/// </summary>
	public float StateTm;

	/// <summary>
	/// The current state.
	/// </summary>
	public States State = States.PLAY;

	/// <summary>
	/// Maps the methods to the states
	/// </summary>
	protected Dictionary<States, Action> StateDict = new Dictionary<States, Action>();

	/// <summary>
	/// The animation UI...used for menus.
	/// </summary>
	public Animator AnimUI;

	/// <summary>
	/// The "double tap to reset" animation.
	/// </summary>
	public Animator DTTRAnim;

	/// <summary>
	/// The pause button.
	/// </summary>
	public Button PauseButton;

	[Header ("StartSettings")]
	/// <summary>
	/// The max number of bounces a star can make for this level
	/// </summary>
	public int maxBounceCount = 9;

	/// <summary>
	/// The time mouse is clicked down
	/// </summary>
	private float mouseDownTm = 0;

	/// <summary>
	/// The position of the mouse when clicked down.
	/// </summary>
	private Vector3 mouseDownPos;

#endregion

#region MonoMethods
	void Awake()
	{
		//maps the States to the methods
		StateDict.Add (States.STARTGAME,StartGame);
		StateDict.Add (States.PLAY,Play);
		StateDict.Add (States.WIN,Win);
		StateDict.Add (States.RESET,Reset);
		StateDict.Add (States.PAUSE,Pause);
		
	}


	void Start()
	{
		//set the State to StartGame
		SetState(States.STARTGAME);
	}


	void Update()
	{
		//execute the method mapped to the current state 
		StateDict[State].Invoke();

		//if win or pause ...return
		if (
			State == States.WIN
			|| State == States.PAUSE
			)
		{
			return;
		}

		//if double tapped...reset the level
		if (Input.GetMouseButtonDown(0))
		{

			if (
				(Time.time - mouseDownTm) < 0.5f
				&& Vector3.Distance(mouseDownPos,Input.mousePosition) < 50f
				)
			{
				SetState(States.RESET);
			}

			mouseDownTm = Time.time;
			mouseDownPos = Input.mousePosition;
		}
	}
#endregion

#region miscMethods
	/// <summary>
	/// method used for setting a new state
	/// </summary>
	/// <param name="NextState">Next state.</param>
	public void SetState(States NextState)
	{
		//do not set the same state twice
		if (State == NextState)
		{
			return;
		}

		//set the current state and record the time
		State = NextState;
		StateTm = Time.time;


		if (State == States.WIN)
		{
			//mark the current level as Won, and Unlock the next Level
			int levelNum = LevelData.getCurrentLevelNum();
			LevelData.setWon(levelNum,true);
			LevelData.Unlock(levelNum +1);

			//do not allow people to pause the game
			PauseButton.interactable = false;

			//don't show the DTTR
			DTTRAnim.SetBool("active",false);
		}

		if (State == States.PAUSE)
		{
			//set timeScale to 0
			Time.timeScale = 0f;

			//don't show the DTTR
			DTTRAnim.SetBool("active",false);
		}
		else if (State != States.PAUSE)
		{
			//set the timeScale = 1
			Time.timeScale = 1f;
		}

		//set int in AnimUI...to turn off/on menus 
		AnimUI.SetInteger("State",(int)State);
	}

	/// <summary>
	/// this method will recycle stars...see PoolManager for more info
	/// </summary>
	private void RecycleStars()
	{
		PoolManager pm = PoolManager.Instance;

		GameObject[] Stars =  GameObject.FindGameObjectsWithTag("Star");

		for(int i = 0; i < Stars.Length ;i++)
		{
			pm.Recycle("Star",Stars[i]);
		}
	}

	/// <summary>
	/// this method will spawns a star...see PoolManager for more info
	/// </summary>
	private void CreateStar()
	{

		GameObject newStar = PoolManager.Instance.Spawn("Star");


		GameObject SP = GameObject.FindGameObjectWithTag("SpawnPoint");
		if (SP == null)
		{
			Debug.LogError("No SpawnPoint Found!");
		}

		newStar.transform.position = SP.transform.position;
		newStar.GetComponent<Star>().maxBounceCount = maxBounceCount;

	}

	/// <summary>
	/// Resets all objects.
	/// </summary>
	private void ResetAllObjects()
	{
		MoveObject[] moveObjects = GameObject.FindObjectsOfType<MoveObject>();

		for(int i = 0; i < moveObjects.Length ;i++)
		{
			moveObjects[i].Reset();
		}

		BreakBlock[] breakBlocks = GameObject.FindObjectsOfType<BreakBlock>();

		for(int i = 0; i < breakBlocks.Length ;i++)
		{
			breakBlocks[i].Reset();
		}
	}

	/// <summary>
	/// Pauses and Unpause.
	/// </summary>
	public void PauseUnPause()
	{
		if (State == States.PAUSE)
		{
			SetState(States.PLAY);
		}
		else
		{
			SetState(States.PAUSE);
		}

	}

	/// <summary>
	/// Exits the level.
	/// </summary>
	public void ExitLevel()
	{
		RecycleStars();
		SceneSwitchAnimator.Instance.ChangeScene("LevelMenu",Input.mousePosition);
	}

	/// <summary>
	/// Resets the current Level
	/// </summary>
	public void Retry()
	{
		SetState(States.RESET);
	}

	/// <summary>
	/// goes to the next level
	/// </summary>
	public void NextLevel()
	{
		RecycleStars();
		int LN =  LevelData.getCurrentLevelNum();
		SceneSwitchAnimator.Instance.ChangeScene(LN+1);
	}
#endregion

#region StateMethods

	/// <summary>
	/// method executes during StartGame
	/// </summary>
	void StartGame()
	{
		//don't show the DTTR
		DTTRAnim.SetBool("active",false);

		//recycle the stars
		RecycleStars();

		//create a new star
		CreateStar();

		//reset all objects
		ResetAllObjects();

		//Switch to play
		SetState(States.PLAY);
	}

	/// <summary>
	/// method executes during Play
	/// </summary>
	void Play()
	{

	}

	/// <summary>
	/// method executes during Win
	/// </summary>
	void Win()
	{

	}

	/// <summary>
	/// method executes during Reset
	/// </summary>
	void Reset()
	{
		//don't show the DTTR
		DTTRAnim.SetBool("active",false);

		//recycle the stars
		RecycleStars();

		//create a new star
		CreateStar();

		//reset all objects
		ResetAllObjects();

		//switch to play
		SetState(States.PLAY);
	}

	/// <summary>
	/// method executes during Pause
	/// </summary>
	void Pause()
	{

	}

#endregion


	//allows us to refer to this gameobject via a script with out referencing it in a variable
#region SINGLETON PATTERN
	private static GameManager _instance;
    
	public static GameManager Instance
    {
        get {
            if (_instance == null)
            {
				_instance = GameObject.FindObjectOfType<GameManager>();
                
                if (_instance == null)
                {
					Debug.LogError("No GameManager Exists");
                }
            }
            
            return _instance;
        }
    }
#endregion
}
