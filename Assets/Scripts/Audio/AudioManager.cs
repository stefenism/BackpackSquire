using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public static AudioManager audioDaddy;

	public AudioSource sfxSource;
	public AudioSource bgmSource;

	public AudioClip BGM;

	void Awake()
	{
		if (audioDaddy == null)
			audioDaddy = this;
		else if (audioDaddy == this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		bgmSource.clip = BGM;
		bgmSource.Play ();
	}

	// Use this for initialization
	void Start () 
	{
		//bgmSource.Play();
	}
	
	static public void playSfx(AudioClip clip)
	{
		audioDaddy.sfxSource.PlayOneShot (clip);
	}
}
