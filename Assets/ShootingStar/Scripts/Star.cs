/*
this script manages the Star
*/

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour {


	/*<summary>Possible Star States<summary>*/
	public enum States
	{
		INACTIVE,
		IDLE,
		PULLED,
		RELEASE,
		DIE,
	}

	/*<summary>the current Star State<summary>*/
	public States State = States.INACTIVE;

	/*<summary>Maps the methods to the states<summary>*/
	protected Dictionary<States, Action> StateDict = new Dictionary<States, Action>();

	/*<summary>
	the distance a touch must be to register
	<summary>*/
//	public float touchDistance = 1.5f;

	/*<summary>
	the maximum distance you can pull back
	<summary>*/
	public float maxPullDistance = 1f;

	/*<summary>
	the maximum force that will be added to the object
	<summary>*/
	public float forceFactor = 1000f;

	/*<summary>
	the number of bounces
	<summary>*/
	private int _bounceCount = 0;
	public int bounceCount
	{
		get{ return _bounceCount;}
		set 
		{
			_bounceCount = value;

			bounceDisplay.text = (maxBounceCount - bounceCount).ToString();

			if ((maxBounceCount - bounceCount) < 0)
			{
				SetState(States.DIE);
			}
		}
	}

	/*<summary>
	the max number of bounces
	<summary>*/
	private int _maxBounceCount = 5;
	public int maxBounceCount
	{
		get{ return _maxBounceCount;}
		set 
		{
			_maxBounceCount = value;

			bounceDisplay.text = (maxBounceCount - bounceCount).ToString();
		}
	}

	/*<summary>
	the text that displays the bounceCount
	<summary>*/
	private Text bounceDisplay;

	/*<summary>
	the sound for collision
	<summary>*/
	public AudioClip collisionSound;

	/*<summary>
	the sound for death
	<summary>*/
	public AudioClip dieSound;

	/*<summary>
	the sound for win
	<summary>*/
	public AudioClip winSound;

	/*<summary>
	the sound for release
	<summary>*/
	public AudioClip releaseSound;

//	/*<summary>
//	the AudioSource that will play the sound
//	<summary>*/
//	private AudioSource audioSource; 

	/*<summary>
	the particles for collision
	<summary>*/
	public string collisionParticles;

	/*<summary>
	the particles for die
	<summary>*/
	public string dieParticles;

	/*<summary>
	the particles for win
	<summary>*/
	public string winParticles;

	/*<summary>
	the particles for idel
	<summary>*/
	public ParticleSystem IdelParticleSystem;

	/*<summary>
	Allow to pull the object again
	<summary>*/
	public bool AllowRepull =  true;




	//the mouse's position
	private Vector3 mousePos;

	//the time when the object was released
	private float releaseTime;

	//used to render the pull back line
	private LineRenderer lineRenderer;

	//the angle the object is pulled back
	private float pullAngle;

	//the distance the object is pulled back
	private float pullDistance;


	private Rigidbody2D RB2D;

	//set some default values
	void Awake()
	{
		//set the lineRenerer
		lineRenderer = GetComponentInChildren<LineRenderer>(); //gameObject.transform.FindChild("Line").GetComponent<LineRenderer>();

		RB2D =  GetComponent<Rigidbody2D>();

		//maps the States to the methods
		StateDict.Add (States.INACTIVE,Inactive);
		StateDict.Add (States.IDLE,Idle);
		StateDict.Add (States.PULLED,Pulled);
		StateDict.Add (States.RELEASE,Release);
		StateDict.Add (States.DIE,Die);


		//get the bounceDisplay
		bounceDisplay = gameObject.transform.GetComponentInChildren<Text>();
		IdelParticleSystem = gameObject.transform.GetComponentInChildren<ParticleSystem>();

		SetState(States.IDLE);

	}

	void OnEnable()
	{
		bounceCount = 0;
		SetState(States.IDLE);
		releaseTime = Time.time;
//		GetComponent<SpriteRenderer>().enabled = true;
//		GetComponent<Collider2D>().enabled = true;
//		bounceDisplay.enabled = true;
//		IdelParticleSystem.gameObject.SetActive(true);
	}

	//this method will be used to change the state
	public void SetState(States NextState)
	{
		State =  NextState;


		if (State == States.DIE)
		{

		}

		if (NextState != States.IDLE)
		{
			IdelParticleSystem.Stop(); 
		}

		if (NextState == States.IDLE || AllowRepull)
		{
			IdelParticleSystem.Play();
		}
	}

	void Update () 
	{

		if (Time.timeScale == 0f)
		{
			return;
		}

		//get the mouse position
		mousePos = Input.mousePosition;
		mousePos.z = 10f;
		mousePos = Camera.main.ScreenToWorldPoint( mousePos);


		//invoke the method the corresponds to the current state
		StateDict[State].Invoke();


		//used for debugging
		//print(Vector2.Distance(Vector2.zero,RB2D.velocity));

		if ( 
			Vector2.Distance(Vector2.zero,RB2D.velocity) > 0f
			&& Vector2.Distance(Vector2.zero,RB2D.velocity) < 3f
			)
		{
			GameObject.Find("DTTR").GetComponent<Animator>().SetBool("active",true);
		}

		if (
			(Time.time - releaseTime) > 15f
			)
		{
			GameObject.Find("DTTR").GetComponent<Animator>().SetBool("active",true);
		}

		if (
			Mathf.Abs(transform.position.x) > 5f
			|| Mathf.Abs(transform.position.y) > 5f
			)
		{
			GameObject.Find("DTTR").GetComponent<Animator>().SetBool("active",true);
		}

	}

	public float getSpeed()
	{
		return Vector2.Distance(Vector2.zero,RB2D.velocity);
	}

	//execute during Inactive State
	private void Inactive()
	{

	}

	//execute during Idle State
	private void Idle()
	{

		if (Input.GetMouseButtonDown(0))
		{
			if (GetComponent<Collider2D>().OverlapPoint(mousePos))
		    {
				SetState(States.PULLED);
		    }
		}

	}

	//execute during Pulled State
	private void Pulled()
	{
		lineRenderer.SetPosition(0,gameObject.transform.position);

		pullAngle = GetAngleDirection(gameObject.transform.position,mousePos);

		pullDistance = Vector2.Distance(gameObject.transform.position,mousePos);
		pullDistance = Mathf.Clamp(pullDistance,0f,maxPullDistance);

		Vector2 pos = GetXYDirection(pullAngle,pullDistance);

		pos = new Vector2(gameObject.transform.position.x,gameObject.transform.position.y) + pos;

		lineRenderer.SetPosition(1,pos);

		if (Input.GetMouseButtonUp(0))
		{
			lineRenderer.SetPosition(0,gameObject.transform.position);
			lineRenderer.SetPosition(1,gameObject.transform.position);

			Vector2 force = GetXYDirection(pullAngle,(pullDistance/maxPullDistance) * forceFactor) * -1f ;
			gameObject.GetComponent<Rigidbody2D>().AddForce(force);

			releaseTime = Time.time;

			SetState(States.RELEASE);

			SoundManager.Instance.Play(releaseSound);

		}

	}

	//execute during Release State
	private void Release()
	{

		if (AllowRepull)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (GetComponent<Collider2D>().OverlapPoint(mousePos))
			    {
					SetState(States.PULLED);
			    }
			}	
		}
	}
	
	//execute during Die State
	private void Die()
	{

		
		GameObject DP = PoolManager.Instance.Spawn(dieParticles);
		DP.transform.position = gameObject.transform.position;
		DP.transform.rotation = gameObject.transform.rotation;

		SoundManager.Instance.Play(dieSound);


		//resetting will recycle all stars, create a new one, and reset all objects
		GameManager.Instance.SetState(GameManager.States.RESET);


	}

	//if the Star Collides with anything
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (
			coll.collider.gameObject.tag == "Border"
			|| coll.collider.gameObject.tag == "Block"
		    )
		{

			bounceCount += 1;

			for (int i =0; i < coll.contacts.Length; i++)
			{
				GameObject CP = PoolManager.Instance.Spawn(collisionParticles);
				CP.transform.position = coll.contacts[i].point;
				CP.transform.rotation = gameObject.transform.rotation;
			}

			SoundManager.Instance.Play(collisionSound);
			
		}
	}



	//if the Star Touches the StarGate
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.tag == "StarGate")
		{

			GameObject WP = PoolManager.Instance.Spawn(winParticles);
			WP.transform.position = other.gameObject.transform.position;
			WP.transform.rotation = other.gameObject.transform.rotation;

			SoundManager.Instance.Play(winSound);

			gameObject.SetActive(false);

			GameManager.Instance.SetState(GameManager.States.WIN);
		}
	}

	//gets the XY coordinates using the Angle and the Magnitude
	private Vector2 GetXYDirection( float angle, float magnitude)
	{
		angle *= -1f;
		angle -= 90f;
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * magnitude;
	}

	//gets the Angle using the origin point and another point
	private float GetAngleDirection( Vector2 point1, Vector2 point2)
	{
		Vector2 v = point1 - point2;
		
		return (float)Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg; 
	}
		

}
