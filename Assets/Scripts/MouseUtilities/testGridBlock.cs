using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGridBlock : MonoBehaviour {

	enum BlockState
	{
		MOUSEOVER,
		IDLE,
	}

	SpriteRenderer sprite;
	BoxCollider2D collider;
	BlockState blockState = BlockState.IDLE;

	Color defaultColor = new Color(0.43f,0.43f,0.43f);
	Color selectedColor = new Color(0.43f,0.43f,0);

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
		collider = GetComponent<BoxCollider2D> ();
		//sprite.color = defaultColor;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CheckMouseOver ();
	}

	void CheckMouseOver()
	{
		if (collider.bounds.Contains (MouseUtilities.getMouseWorldPosition ()))
		{
			blockState = BlockState.MOUSEOVER;
			doMouseOverState ();
		}
		else
		{
			blockState = BlockState.IDLE;
			doIdleState ();
		}

	}

	void doIdleState()
	{
		sprite.color = defaultColor;
	}

	void doMouseOverState()
	{
		sprite.color = selectedColor;
	}

	public bool isMouseOver(){return blockState == BlockState.MOUSEOVER;}
}
