using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGridBlock : MonoBehaviour {

	SpriteRenderer sprite;
	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnMouseOver()
	{
		sprite.color = Color.yellow;
	}

	void OnMouseExit()
	{
		sprite.color = Color.white;
	}
}
