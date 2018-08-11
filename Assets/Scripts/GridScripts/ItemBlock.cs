using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBlock : GridObject {

	enum DragState
	{
		IDLE,
		DRAGGING,
		SNAPPED,
	}

	DragState dragState = DragState.IDLE;
	Color dragColor = new Color(0.43f,0.18f,0.43f, 0.5f);

	public float rotateSpeed = 10;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		collider = GetComponent<BoxCollider2D> ();
		sprite.color = defaultColor;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();

		checkDrag ();
		checkSnap ();
		checkRotation ();
	}

	public override void CheckMouseOver()
	{
		if (isSnapped () || isDragging())
			return;
		
		base.CheckMouseOver ();
	}

	void checkDrag()
	{
		if (isSnapped ())
			return;
		
		if (isMouseOver ())
		{
			if (Input.GetMouseButton (0))
			{
				setDragging ();
				sprite.color = dragColor;
				GameManager.setCurrentItem (this);
			}
		}

		if (isDragging ())
			SnapPosition(MouseUtilities.getMouseWorldPosition ());

		if (Input.GetMouseButtonUp (0))
		{
			setDropped ();
		}
	}

	void checkSnap()
	{
		if (!isSnapped ())
			return;
		
//		if (MouseUtilities.currentGridPoint == MouseUtilities.invalidGridPoint)
//		{
//			setDragging ();
//		}

		if (Input.GetMouseButtonUp (0))
		{
			setDropped ();
		}
	}

	void checkRotation()
	{
		if (isSnapped () || isDragging ())
		{
			if (Input.GetMouseButtonDown (1))
				doRotation ();
		}
	}

	void doRotation()
	{

		StopAllCoroutines ();
		StartCoroutine(rotateBlock());
	}

	void setDropped()
	{
		setIdle ();
		setDragIdle ();
		GameManager.setCurrentItem (null);
	}

	public bool isDragging(){return dragState == DragState.DRAGGING;}
	public bool isSnapped(){return dragState == DragState.SNAPPED;}
	public void setDragging(){dragState = DragState.DRAGGING;}
	public void setDragIdle(){dragState = DragState.IDLE;}
	public void setSnapped(){dragState = DragState.SNAPPED;}

	public void SnapPosition(Vector2 newPosition)
	{
		transform.parent.position = newPosition;
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
	}
}
