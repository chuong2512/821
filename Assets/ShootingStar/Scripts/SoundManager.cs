/*
SoundManager.cs
Version 1.0

This script is used to manage the Sounds.
*/

using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	[HideInInspector]
	//this audio source
	public AudioSource audioSource;

	//the max volume the sound should be
	public float maxVolume;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		bool On = (PlayerPrefsBool.HasBoolKey("Sound"))?PlayerPrefsBool.GetBool("Sound"):true;
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
	public void Play(AudioClip AC, float volume = 1f)
	{
		audioSource.PlayOneShot(AC, volume);
	}

	//allows us to refer to this gameobject via a script with out referencing it in a variable
    #region SINGLETON PATTERN
	private static SoundManager _instance;
    
	public static SoundManager Instance
    {
        get {
            if (_instance == null)
            {
				_instance = GameObject.FindObjectOfType<SoundManager>();
                
                if (_instance == null)
                {
					_instance = Instantiate(Resources.Load("SoundManager", typeof(SoundManager))) as SoundManager;
					_instance.gameObject.name = _instance.gameObject.name.Replace("(Clone)",""); //removed the "(Clone)" from the name
                }
            }
            
            return _instance;
        }
    }
    #endregion

}
