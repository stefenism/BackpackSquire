using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoints : MonoBehaviour {

	public List<Transform> itemGridPoints = new List<Transform>();
	public List<Vector2> itemGridPointss = new List<Vector2> ();

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform)
		{
			itemGridPoints.Add (child);
			itemGridPointss.Add (child.position);
		}
	}
}
