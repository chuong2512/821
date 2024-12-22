/*
BreakBlock.cs
Version 1.0

Manages blocks that will break when the star hits it.
*/

using UnityEngine;
using System.Collections;

public class BreakBlock : MonoBehaviour 
{

	/// <summary>
	/// The break particles.
	/// </summary>
	public string breakParticles;

	/// <summary>
	/// this collider.
	/// </summary>
	private Collider2D thisCollider;

	/// <summary>
	/// this sprite render.
	/// </summary>
	private SpriteRenderer thisSpriteRender;

	void Start()
	{
		getComponents();
	}

	//on Collision
	void OnCollisionEnter2D(Collision2D coll) 
	{

		if (
			coll.gameObject.tag == "Star"
		    )
		{
			thisCollider.enabled = false;
			thisSpriteRender.enabled = false;

			GameObject BP = PoolManager.Instance.Spawn(breakParticles);
			BP.transform.position = gameObject.transform.position;
			BP.transform.rotation = gameObject.transform.rotation;
			BP.transform.localScale = gameObject.transform.localScale;
		}
	}

	//allows GameManager to reset this object
	public void Reset()
	{
		getComponents();

		thisCollider.enabled = true;
		thisSpriteRender.enabled = true;
	}

	//gets references to the components of objects
	private void getComponents()
	{
		if (thisCollider == null)
		{
			thisCollider = GetComponent<Collider2D>();
		}

		if (thisSpriteRender == null)
		{
			thisSpriteRender = GetComponent<SpriteRenderer>();
		}
	}

}
