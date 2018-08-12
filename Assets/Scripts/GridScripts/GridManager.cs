using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	public static GridManager gridDaddy = null;

	public List<GridBlock> gridSpaces = new List<GridBlock>();
	public List<ItemBlock> itemsInGrid = new List<ItemBlock> ();

	static BoxCollider2D gridCollider;

	void Awake()
	{
		if (gridDaddy == null)
			gridDaddy = this;
		else if (gridDaddy == this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

	}

	void Start () 
	{
		gridCollider = GetComponent<BoxCollider2D> ();
		foreach (Transform child in transform)
		{
			GridBlock gridBlock = child.GetComponent<GridBlock> ();
			if (gridBlock != null)
				gridSpaces.Add (gridBlock);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	static public void removeFromList(ItemBlock removeItem)
	{
		gridDaddy.itemsInGrid.Remove (removeItem);
	}

	static public bool IsInCurrentList(ItemBlock newItem)
	{
		if (gridDaddy.itemsInGrid.Contains (newItem))
			return true;
		return false;
	}

	static public bool isSpaceAvailable(List<Transform> newPositions, ItemBlock testItem = null)
	{
		if (gridDaddy.itemsInGrid.Count > 0)
		{
			foreach (ItemBlock item in gridDaddy.itemsInGrid)
				if (item != GameManager.GetCurrentItem () && testItem == null)
				{
					foreach (Transform inGridItemPoints in item.points.itemGridPoints)
						foreach (Transform itempoint in newPositions)
							if (itempoint.position == inGridItemPoints.position)
								return false;
				}
				else if (testItem != null && item != testItem)
				{
					foreach (Transform inGridItemPoints in item.points.itemGridPoints)
						foreach (Transform itemPoint in newPositions)
							if (itemPoint.position == inGridItemPoints.position)
								return false;
				}

			return true;
		}
		return true;
	}

	static public bool isInGrid(List<Transform> newPositions)
	{
		foreach (Transform itemPoint in newPositions)
			if (!gridCollider.bounds.Contains (itemPoint.position))
				return false;
		return true;
	}

	static public void AddCurrentItemToList(ItemBlock newItem){gridDaddy.itemsInGrid.Add (newItem);}
}
