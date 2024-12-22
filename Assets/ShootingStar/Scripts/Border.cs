/*
Border.cs
Version 1.0

Sets a border based on the screen's edges.
*/

using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour 
{

	/// <summary>
	/// The padding of where the border should be
	/// </summary>
	public float padding = 0f;


	void Awake()
	{
		//find the camera 
		Camera cam = Camera.main;

		//camera's z position will be used later
		float camz = cam.gameObject.transform.position.z;

		//get the edge collider 
		EdgeCollider2D EColl = GetComponent<EdgeCollider2D>();
		if (EColl == null)
		{
			EColl = gameObject.AddComponent<EdgeCollider2D>();
		}

		//get the position of each cornder of the screen.
		Vector3[] ScreenCorners =  new Vector3[5];
		ScreenCorners[0] = new Vector3(0f,Screen.height,camz * -1f);
		ScreenCorners[1] = new Vector3(Screen.width,Screen.height,camz * -1f);
		ScreenCorners[2] = new Vector3(Screen.width,0f,camz * -1f);
		ScreenCorners[3] = new Vector3(0f ,0f ,camz * -1f);
		ScreenCorners[4] = ScreenCorners[0];

		//set these in a temp array
		Vector2[] tempArray = new Vector2[5];

		//loop for each point...and offset based on padding
		for(int i = 0; i < 5; i++)
		{
			tempArray[i] = (Vector2)cam.ScreenToWorldPoint(ScreenCorners[i]);

			if (tempArray[i].x > 0f)
			{
				tempArray[i].x += padding;
			}
			else
			{
				tempArray[i].x -= padding;
			}

			if (tempArray[i].y > 0f)
			{
				tempArray[i].y += padding;
			}
			else
			{
				tempArray[i].y -= padding;
			}
		}

		//set the points to the edge collider.
		EColl.points = tempArray;

	}


}
