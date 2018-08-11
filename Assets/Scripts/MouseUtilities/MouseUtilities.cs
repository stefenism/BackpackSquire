using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseUtilities : MonoBehaviour 
{
	static public Vector2 invalidGridPoint = new Vector2 (-9999, -9999);
	static public Vector2 currentGridPoint = MouseUtilities.invalidGridPoint;

	static public Vector2 getMouseWorldPosition()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		return mousePosition;
	}

	static public Vector2 getCurrentGridPoint(){return currentGridPoint;}

}
