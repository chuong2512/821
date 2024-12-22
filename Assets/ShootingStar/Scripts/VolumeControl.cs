/*
VolumeControl.cs
Version 1.0

this script can turn music or sound off/on.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VolumeControl : MonoBehaviour {

	
	/// <summary>
	/// the key should be Music or Sound
	/// </summary>
	public string Key = "Music";

	/// <summary>
	/// whether or not it's on
	/// </summary>
	public bool On;

	/// <summary>
	/// The on sprite.
	/// </summary>
	public Sprite OnSprite;

	/// <summary>
	/// The off sprite.
	/// </summary>
	public Sprite OffSprite;

	/// <summary>
	/// The current image.
	/// </summary>
	public Image image;

	//set up everything on start
	void Start()
	{
		On = (PlayerPrefsBool.HasBoolKey(Key))?PlayerPrefsBool.GetBool(Key):true;

		image.sprite = (On)?OnSprite:OffSprite;

		float v = (On)?1f:0f;

		if (Key == "Music")
		{
			MusicManager.Instance.SetVolume(v);
		}
		else if (Key == "Sound")
		{
			SoundManager.Instance.SetVolume(v);
		}
	}


	//OnPress switch 
	public void OnPress()
	{
		On = !On;

		PlayerPrefsBool.SetBool(Key,On);

		image.sprite = (On)?OnSprite:OffSprite;

		float v = (On)?1f:0f;

		if (Key == "Music")
		{
			MusicManager.Instance.SetVolume(v);
		}
		else if (Key == "Sound")
		{
			SoundManager.Instance.SetVolume(v);
		}
	}
}
