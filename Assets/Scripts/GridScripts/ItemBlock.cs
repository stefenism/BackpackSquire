using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : GridObject {

	enum DragState
	{
		IDLE,
		DRAGGING,
		SNAPPED,
		COLLIDING,
	}

	DragState dragState = DragState.IDLE;
	ItemValues value;

	public Transform itemSprite;
	//Collider2D collider;
	public Color dragColor = new Color(0.43f,0.18f,0.43f, 0.5f);

	public ItemPoints points;
	public float rotateSpeed = 10;
	public float fixedFramesToWaitForFall = 5;

	public AudioClip rotateSound;

	private float fallTimer = 0;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		collider = GetComponent<Collider2D> ();
		value = GetComponent<ItemValues> ();

		sprite.color = defaultColor;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		checkDrag ();
		checkSnap ();
		checkRotation ();

		checkBelow ();
	}

	public override void CheckMouseOver()
	{
//		if (isSnapped () || isDragging())
//			return;
		
		base.CheckMouseOver ();
	}

	public virtual void checkDrag()
	{
		if (isSnapped ())
			return;
		
		if (isMouseOver ())
		{
			if (Input.GetMouseButton (0))
			{
				if (GameManager.GetCurrentItem () == null)
				{
					setDragging ();
					sprite.color = dragColor;
					GameManager.setCurrentItem (this);
				}
			}
		}

		if (isDragging ())
		{
			transform.parent.position = MouseUtilities.getMouseWorldPosition ();
		}

		if (Input.GetMouseButtonUp (0))
		{
			setDropped ();
			setDragIdle ();
		}
	}

	public virtual void checkSnap()
	{
		if (!isSnapped ())
			return;

		if (isMouseOver ())
		{
			if (GameManager.GetCurrentItem () != null)
				sprite.color = dragColor;
			if (Input.GetMouseButton (0))
			{
				if (GameManager.GetCurrentItem () == null)
				{
					sprite.color = dragColor;
					GameManager.setCurrentItem (this);
				}
			}
		}

		if (Input.GetMouseButtonUp (0))
		{
			setDropped ();

			if (!GridManager.IsInCurrentList (this))
			{
				GridManager.AddCurrentItemToList (this);
				GameManager.addGold (value.getWorth());
			}
				
		}
	}

	void checkRotation()
	{
		if (isSnapped () || isDragging ())
		{
			if (Input.GetMouseButtonDown (1))
			{
				if (GameManager.GetCurrentItem () == this)
					doRotation ();
			}
		}
	}

	void checkBelow()
	{
		fallTimer += Time.fixedDeltaTime;
		if (isSnapped () && !isMouseOver ())
		{
			if (fallTimer >= Time.fixedDeltaTime * fixedFramesToWaitForFall)
			{
				fallTimer = 0;
				doFall ();
			}
		}

	}

	void doRotation()
	{
		transform.parent.Rotate (0, 0, 90);
		if (isSnapped ())
		{
			if (!GridManager.isInGrid (points.itemGridPoints) || !GridManager.isSpaceAvailable (points.itemGridPoints))
				transform.parent.Rotate (0, 0, -90);
			else
			{
				AudioManager.playSfx (rotateSound);
				transform.parent.Rotate (0, 0, -90);
				StopAllCoroutines ();
				StartCoroutine (rotateBlock ());
			}
		}
		else
		{
			AudioManager.playSfx (rotateSound);
			transform.parent.Rotate (0, 0, -90);
			StopAllCoroutines ();
			StartCoroutine (rotateBlock ());
		}
	}

	void doFall()
	{
		Debug.Log ("doin' fall");
		Vector2 newPosition = transform.parent.position;
		newPosition.y--;
		SnapPosition (newPosition, this);
	}

	void setDropped()
	{
		setIdle ();
		GameManager.setCurrentItem (null);
		sprite.color = defaultColor;
	}

	public bool isDragging(){return dragState == DragState.DRAGGING;}
	public bool isSnapped(){return dragState == DragState.SNAPPED;}
	public bool isColliding(){return dragState == DragState.COLLIDING;}

	public void setDragging(){dragState = DragState.DRAGGING;}
	public void setDragIdle(){dragState = DragState.IDLE;}
	public void setSnapped(){dragState = DragState.SNAPPED;}
	public void setColliding(){dragState = DragState.COLLIDING;}

	public void SnapPosition(Vector2 newPosition, ItemBlock testItem = null)
	{
		Vector2 currentPosition = transform.parent.position;
		transform.parent.position = newPosition;

		if (!GridManager.isInGrid (points.itemGridPoints) || !GridManager.isSpaceAvailable (points.itemGridPoints, testItem))
			transform.parent.position = currentPosition;

		Debug.Log ("is point in grid: " + GridManager.isInGrid (points.itemGridPoints));
		Debug.Log("is set of points available: " + GridManager.isSpaceAvailable(points.itemGridPoints));
	}		

	public virtual void removeItem()
	{
		GridManager.removeFromList (this);
		GameManager.addGold (-value.getWorth ());
	}

	IEnumerator rotateBlock()
	{
		Quaternion oldRotation = transform.parent.rotation;
		transform.parent.Rotate (0, 0, 90);
		Quaternion newRotation = transform.parent.rotation;
		for(float t = 0; t < 1; t += Time.deltaTime * rotateSpeed)
		{
			transform.rotation = Quaternion.Slerp(oldRotation, newRotation, t);
			yield return null;
		}
		transform.rotation = transform.parent.rotation;


	}
}
