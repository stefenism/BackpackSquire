using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameDaddy = null;

	private ItemBlock currentItem;

	void Awake()
	{
		if (gameDaddy == null)
			gameDaddy = this;
		else if (gameDaddy == this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		Initialize ();
	}

	void Initialize()
	{
		//initialize stuff here
	}

	static public void setCurrentItem(ItemBlock newItem)
	{
		gameDaddy.currentItem = newItem;
	}

	static public ItemBlock GetCurrentItem(){return GameManager.gameDaddy.currentItem;}
}
