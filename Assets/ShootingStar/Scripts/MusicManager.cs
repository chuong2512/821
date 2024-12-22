/*
MusicManager.cs
Version 1.0

This script is used to manage the music.
*/

using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	[HideInInspector]
	//this audio source
	public AudioSource audioSource;

	//the max volume the music should be
	public float maxVolume;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		bool On = (PlayerPrefsBool.HasBoolKey("Music"))?PlayerPrefsBool.GetBool("Music"):true;
		float v = (On)?1f:0f;
		SetVolume(v);
	}


	/// <summary>
	/// set the volume for the music
	/// </summary>
	/// <param name="v">V.</param>
	public void SetVolume(float v)
	{
		audioSource.volume = Mathf.Clamp(v,0,maxVolume);
	}


	/// <summary>
	/// plays the music
	/// </summary>
	/// <param name="AC">A.</param>
	public void Play(AudioClip AC)
	{
		audioSource.clip = AC;
	}

	//allows us to refer to this gameobject via a script with out referencing it in a variable
    #region SINGLETON PATTERN
	private static MusicManager _instance;
    
	public static MusicManager Instance
    {
        get {
            if (_instance == null)
            {
				_instance = GameObject.FindObjectOfType<MusicManager>();
                
                if (_instance == null)
                {
					_instance = Instantiate(Resources.Load("MusicManager", typeof(MusicManager))) as MusicManager;
					_instance.gameObject.name = _instance.gameObject.name.Replace("(Clone)",""); //removed the "(Clone)" from the name
                }
            }
            
            return _instance;
        }
    }
    #endregion

}

