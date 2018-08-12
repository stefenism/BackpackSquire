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
		CONVEYOR,
		DROPPING,
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
	private float leftTimer = 0;

	// Use this for initialization
	void Start () {
		initialize ();
	}

	public void initialize()
	{
		sprite = GetComponent<SpriteRenderer> ();
		collider = GetComponent<Collider2D> ();
		value = GetComponent<ItemValues> ();

		sprite.color = defaultColor;

		setIdle ();
		setConveyor ();
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
		if (isSnapped () || isDropping())
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
			if (GridManager.isOnConveyor (points.itemGridPoints))
				GridManager.getConveyor ().addToConveyor (this);
			else
			{
				setDropped ();
				if(!isInConveyor())
					setDragIdle ();
			}
		}
	}

	public virtual void checkSnap()
	{
		if (!isSnapped () || isDropping())
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

	public void checkLeft()
	{
		leftTimer += Time.fixedDeltaTime;
		if (isInConveyor () && GameManager.GetCurrentItem() != this)
		{
			if (leftTimer >= Time.fixedDeltaTime * fixedFramesToWaitForFall)
			{
				leftTimer = 0;
				doMoveLeft ();
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
		Vector2 newPosition = transform.parent.position;
		newPosition.y--;
		SnapPosition (newPosition, this);
	}

	void doMoveLeft()
	{		
		Vector2 newPosition = transform.parent.position;
		newPosition.x--;
		SnapLeft (newPosition);
	}

	void setDropped()
	{
		setIdle ();
		GameManager.setCurrentItem (null);
		sprite.color = defaultColor;
	}

	void doConveyorDrop()
	{	
		if (isDropping ())
			return;

		setDropping ();
		GridManager.getConveyor ().removeFromList (this);
		transform.parent.gameObject.AddComponent<Rigidbody2D> ();
		Destroy (transform.parent.gameObject, 5);



	}		

	public bool isDragging(){return dragState == DragState.DRAGGING;}
	public bool isSnapped(){return dragState == DragState.SNAPPED;}
	public bool isColliding(){return dragState == DragState.COLLIDING;}
	public bool isInConveyor(){return dragState == DragState.CONVEYOR;}
	public bool isIdle(){return dragState == DragState.IDLE;}
	public bool isDropping(){return dragState == DragState.DROPPING;}

	public void setDragging(){dragState = DragState.DRAGGING;}
	public void setDragIdle(){dragState = DragState.IDLE;}
	public void setSnapped(){dragState = DragState.SNAPPED;}
	public void setColliding(){dragState = DragState.COLLIDING;}
	public void setConveyor(){dragState = DragState.CONVEYOR;}
	public void setDropping(){dragState = DragState.DROPPING;}

	public void SnapPosition(Vector2 newPosition, ItemBlock testItem = null)
	{
		Vector2 currentPosition = transform.parent.position;
		transform.parent.position = newPosition;

		if (!GridManager.isInGrid (points.itemGridPoints) || !GridManager.isSpaceAvailable (points.itemGridPoints, testItem))
			transform.parent.position = currentPosition;
	}		

	public void SnapLeft(Vector2 newPosition)
	{
		transform.parent.position = newPosition;

		if (!GridManager.isOnConveyor (points.itemGridPoints))
		{			
			doConveyorDrop ();
		}			
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
