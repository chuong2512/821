/*
this script manages the movement of objects
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MoveObject : MonoBehaviour {

	/*<summary>possible move states<summary>*/
	public enum States
	{
		INACTIVE,
		MOVE,
		WAIT,
	}

	/*<summary>current move state<summary>*/
	public States State = States.INACTIVE;

	/*<summary>previous move state<summary>*/
	public States State_prev = States.MOVE;

	/*<summary>Maps the functions to the states<summary>*/
	protected Dictionary<States, Action> StateDict = new Dictionary<States, Action>();

	/*<summary>transitions that the object will move between<summary>*/
	public List<Transform> Transforms = new List<Transform>();

	/*<summary>the index of the current transform<summary>*/
	private int transformIndex = 1;

	/*<summary>the speed of position change<summary>*/
	public float positionSpeed = 1f;

	public bool lerpPosition = true;

	/*<summary>the speed of rotation change<summary>*/
	public float rotationSpeed = 1f;

	public bool lerpRotation = true;

	/*<summary>the speed of scale change<summary>*/
	public float scaleSpeed = 1f;

	public bool lerpScale = true;

	/*<summary>time to wait between transitions<summary>*/
	public float waitTime = 0.5f;

	/*<summary>used to store the time waiting starts<summary>*/
	private float StartWaitTime;

	/*<summary>used if cycle type if pingpong<summary>*/
	private int PingPongDirection = 1;

	/*<summary>possible cycle types<summary>*/
	public enum CycleTypes
	{
		LOOP,
		PINGPONG,
	}

	/*<summary>current cycle type<summary>*/
	public CycleTypes CycleType = CycleTypes.LOOP;

	void Awake()
	{
		if (Transforms.Count < 2)
		{
			Debug.Log("you need 2 or more transitions");
		}

		StateDict.Add (States.INACTIVE,Inactive);
		StateDict.Add (States.MOVE,Move);
		StateDict.Add (States.WAIT,Wait);

		SetState(States.MOVE);

		gameObject.transform.position = Transforms[0].position;
		gameObject.transform.rotation = Transforms[0].rotation;
		gameObject.transform.localScale = Transforms[0].localScale;
		transformIndex = 1;
	}

	void FixedUpdate () 
	{
		//execute current state
		StateDict[State].Invoke();
	}

	//used to change state
	public void SetState(States NextState)
	{
		State_prev = State;
		State =  NextState;

		if (NextState == States.WAIT)
		{
			StartWaitTime = Time.time;
		}
			
	}

	//used to set to the previous state
	public void SetPrevState()
	{
		SetState(State_prev);
	}

	//used to move the object
	private void Move()
	{

		if (lerpPosition)
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,Transforms[transformIndex].position, Time.deltaTime * positionSpeed);
		}
		else
		{
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,Transforms[transformIndex].position, Time.deltaTime * positionSpeed);
		}

		if (lerpRotation)
		{
			gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation,Transforms[transformIndex].rotation, Time.deltaTime * rotationSpeed);
		}
		else
		{
			Vector3 V3 = Vector3.MoveTowards(gameObject.transform.rotation.eulerAngles,Transforms[transformIndex].rotation.eulerAngles, Time.deltaTime * rotationSpeed);
			gameObject.transform.rotation = Quaternion.Euler(V3);
		}

		if (lerpScale)
		{
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale,Transforms[transformIndex].localScale, Time.deltaTime * scaleSpeed);
		}
		else
		{
			gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale,Transforms[transformIndex].localScale, Time.deltaTime * scaleSpeed);
		}


		if (
			Vector3.Distance(gameObject.transform.position,Transforms[transformIndex].position) < 0.05f
			&& Vector3.Distance(gameObject.transform.localScale,Transforms[transformIndex].localScale) < 0.05f
			&& Vector3.Distance(gameObject.transform.rotation.eulerAngles,Transforms[transformIndex].rotation.eulerAngles) < 0.05f
			)
		{
			if (CycleType == CycleTypes.LOOP)
			{
				transformIndex = (transformIndex + 1) % Transforms.Count;
			}
			else if (CycleType == CycleTypes.PINGPONG)
			{
				if (
					(transformIndex + PingPongDirection) < 0
					||
					(transformIndex + PingPongDirection) >= Transforms.Count
					)
				{
					PingPongDirection *= -1;
				}

				transformIndex = transformIndex + PingPongDirection;
			}

			SetState(States.WAIT);
		}
	}

	//used during wait state
	private void Wait()
	{
		if (Time.time - StartWaitTime > waitTime)
		{
			SetState(States.MOVE);
		}
	}

	//used during inactive state
	private void Inactive()
	{

	}

	//used to reset the object's movment
	public void Reset()
	{
		gameObject.transform.position = Transforms[0].position;
		gameObject.transform.rotation = Transforms[0].rotation;
		gameObject.transform.localScale = Transforms[0].localScale;

		transformIndex = 1;
	}

}
