/*
BrakeArea.cs
Version 1.0

this is used to slow down all objects that have a tag in the TagList
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Collider2D))]
public class BrakeArea : MonoBehaviour {

	/*<summary>amount we are breaking the object<summary>*/
	public float breakAmount = 1.1f;

	/*<summary>List of tag this will interact with<summary>*/
	public List<string> TagList =  new List<string>();

	/*<summary>get the ridgiebody and set the tigger to null<summary>*/
	void Awake()
	{
		gameObject.GetComponent<Collider2D>().isTrigger = true;
	}

	void OnTriggerStay2D(Collider2D other) 
	{
		//the Object must have a RigidBody2D
		if (other.GetComponent<Rigidbody2D>() == null)
		{
			return;
		}

		//if Tag in TagList slowdown the object
		if (TagList.Contains(other.gameObject.tag))
		{
			Vector2 velocity = other.GetComponent<Rigidbody2D>().velocity;
			velocity = velocity/breakAmount;
			other.GetComponent<Rigidbody2D>().velocity = velocity;
		}
	}
}
