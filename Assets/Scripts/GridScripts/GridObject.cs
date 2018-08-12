using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {

	enum BlockState
	{
		MOUSEOVER,
		IDLE,
	}

	public SpriteRenderer sprite;
	public Collider2D collider;
	BlockState blockState = BlockState.IDLE;

	public Color defaultColor = new Color(0.43f,0.43f,0.43f);
	public Color selectedColor = new Color(0.43f,0.43f,0);

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		collider = GetComponent<Collider2D> ();
		//sprite.color = defaultColor;
	}

	// Update is called once per frame
	public virtual void Update () 
	{
		CheckMouseOver ();
	}

	public virtual void CheckMouseOver()
	{
		
		if (collider.bounds.Contains (MouseUtilities.getMouseWorldPosition ()))
		{
			if (blockState == BlockState.MOUSEOVER)
				return;
			
			blockState = BlockState.MOUSEOVER;
			doMouseOverState ();
		}
		else
		{
			if (blockState == BlockState.IDLE)
				return;
			
			blockState = BlockState.IDLE;
			doIdleState ();
		}
	}

	public Collider2D getCollider(){return collider;}

	public virtual void doIdleState()
	{
		sprite.color = defaultColor;
	}

	public virtual void doMouseOverState()
	{
		sprite.color = selectedColor;
	}

	public bool isMouseOver(){return blockState == BlockState.MOUSEOVER;}

	public void setIdle(){blockState = BlockState.IDLE;}
}
