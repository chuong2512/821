/*
PlaySound.cs
Version 1.0

This script is used to play a sound
*/

using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour 
{
	/// <summary>
	/// The sound clip
	/// </summary>
	public AudioClip Sound;

	/// <summary>
	/// This audio source.
	/// </summary>
	private AudioSource audioSource;

	void Start()
	{
		audioSource = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
	}

	/// <summary>
	/// plays the sound on button press
	/// </summary>
	public void OnPress()
	{
		audioSource.PlayOneShot(Sound);
	}
}
