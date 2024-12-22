/*
GoToScene.cs
Version 1.0

Allows a button to change the Scene
*/

using UnityEngine;
using System.Collections;

public class GoToScene : MonoBehaviour {

	//scene to switch to
	public string SceneName;

	//executes on press
	public void OnPress()
	{
		//displays animation than switch scene...
		SceneSwitchAnimator.Instance.ChangeScene(SceneName,gameObject.transform.position);
	}
}
