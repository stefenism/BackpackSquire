using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager gameDaddy = null;

	public ItemBlock currentItem;
	public List<ItemBlock> currentItems = new List<ItemBlock>();
	private List<GridBlock> currentGrid = new List<GridBlock> ();

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

	static public bool isSpaceAvailable(Vector2 newPosition)
	{
		foreach(ItemBlock item in gameDaddy.currentItems)
		{
			if (item.getCollider ().bounds.Contains (newPosition))
				return false;
		}
		return true;
	}

	static public bool IsInCurrentList(ItemBlock newItem)
	{
		if (gameDaddy.currentItems.Contains (newItem))
			return true;
		return false;
	}
		
	static public ItemBlock GetCurrentItem(){return GameManager.gameDaddy.currentItem;}
	static public void AddCurrentItemToList(ItemBlock newItem){gameDaddy.currentItems.Add (newItem);}
}
