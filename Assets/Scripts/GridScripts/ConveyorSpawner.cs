using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : GridObject {

	List<ItemBlock> conveyorItems;
	public Color conveyorSelectedColor = new Color (.5f, .5f, .5f);

	public GameObject[] spawnableItems;
	public Transform spawnPoint;
	public float timeBeforeSpawning;
	private float currentSpawnTimer;


	void Awake()
	{
		conveyorItems = new List<ItemBlock>();
		spawnItem ();
	}

	void LateUpdate()
	{
		base.Update ();
		moveConveyorItems ();
		checkSpawn ();
	}

	public override void doMouseOverState()
	{		
		sprite.color = conveyorSelectedColor;
		if (GameManager.GetCurrentItem () != null && GameManager.GetCurrentItem().isSnapped())
		{
			GameManager.GetCurrentItem ().setIdle ();
			GameManager.GetCurrentItem ().setDragging();
			conveyorItems.Add (GameManager.GetCurrentItem ());
			GridManager.removeFromList (GameManager.GetCurrentItem ());
			GameManager.GetCurrentItem ().removeItem ();
		}

		if (GameManager.GetCurrentItem () != null && GameManager.GetCurrentItem ().isIdle())
		{
			if (Input.GetMouseButtonUp (0))
			{				
				GameManager.GetCurrentItem ().setIdle ();
				GameManager.GetCurrentItem ().setConveyor ();
				conveyorItems.Add (GameManager.GetCurrentItem ());
			}				
		}
	}

	public override void doIdleState()
	{
		base.doIdleState ();
		if (GameManager.GetCurrentItem () != null)
		{
			if (conveyorItems.Contains (GameManager.GetCurrentItem ()))
				removeFromList (GameManager.GetCurrentItem ());
		}
	}

	public void addToConveyor(ItemBlock addItem)
	{
		addItem.setIdle ();
		addItem.setConveyor ();
		conveyorItems.Add (addItem);
	}

	public void removeFromList(ItemBlock removeItem)
	{
		conveyorItems.Remove (removeItem);
	}		

	void moveConveyorItems()
	{
		if (conveyorItems.Count > 0)
		{
			foreach (ItemBlock item in conveyorItems)
			{
				item.checkLeft ();
			}
		}
	}

	void checkSpawn()
	{
		currentSpawnTimer += Time.fixedDeltaTime;
		if (currentSpawnTimer >= timeBeforeSpawning)
		{
			spawnItem();
			currentSpawnTimer = 0;
			Debug.Log ("gametimer current tally: " + GameManager.getGameTimer ());
		}			
		
	}

	void spawnItem()
	{
		int itemIndex = Random.Range (0, spawnableItems.Length - 1);
		GameObject clone = Object.Instantiate (spawnableItems [itemIndex], spawnPoint.position, Quaternion.identity, null);
		ItemBlock cloneItem = clone.transform.GetChild (0).GetComponent<ItemBlock> ();

		cloneItem.initialize ();
		conveyorItems.Add (cloneItem);
	}
}
