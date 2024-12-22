/*
ForceArea.cs
Version 1.0


this is used to add a force to all objects that have a tag in the TagList
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Collider2D))]
public class ForceArea : MonoBehaviour {

    /*<summary>
    The Force Types that this area will include
	</summary>*/
	public enum ForceTypes
	{
		Inward,
		Outward,
		Directional,
	}

    /*<summary>The Force Types that this is currently including </summary>
    */
	public ForceTypes forceType;

	/*<summary>the amount of force this will hve on an object</summary>*/
	public float forceAmount = 1f;

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

		//if Tag in TagList
		if (TagList.Contains(other.gameObject.tag))
		{
			if (forceType == ForceTypes.Directional)
			{
				//apply directional force
				other.GetComponent<Rigidbody2D>().AddForce(gameObject.transform.up * forceAmount);
			}
			else if (forceType == ForceTypes.Outward)
			{
				//apply inward, sucking force
				float angle = GetAngleDirection( gameObject.transform.position, other.gameObject.transform.position);
				Vector2 XY = GetXYDirection(angle,forceAmount);

				other.GetComponent<Rigidbody2D>().AddForce(XY);
			}
			else if (forceType == ForceTypes.Inward)
			{
				//apply outward force
				float angle = GetAngleDirection( gameObject.transform.position, other.gameObject.transform.position);
				Vector2 XY = GetXYDirection(angle,forceAmount) * -1f;

				other.GetComponent<Rigidbody2D>().AddForce(XY);
			}
		}
	}


	//get XY coordinate from angle and magnitude  
	private Vector2 GetXYDirection( float angle, float magnitude)
	{
		angle *= -1f;
		angle -= 90f;
		return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * magnitude;
	}

	//get angle between two points
	private float GetAngleDirection( Vector2 point1, Vector2 point2)
	{
		Vector2 v = point1 - point2;

		return (float)Mathf.Atan2(v.x, v.y) * Mathf.Rad2Deg; 
	}
}
