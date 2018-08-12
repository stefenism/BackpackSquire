using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	static AudioManager audioDaddy;

	public AudioSource sfxSource;
	public AudioSource bgmSource;

	void Awake()
	{
		if (audioDaddy == null)
			audioDaddy = this;
		else if (audioDaddy == this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	static public void playSfx(AudioClip clip)
	{
		audioDaddy.sfxSource.PlayOneShot (clip);
	}
}
