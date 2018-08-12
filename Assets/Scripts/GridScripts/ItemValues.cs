using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemValues : MonoBehaviour {

	int goldWorth = 1;

	public int lowPotentialWorth = 0;
	public int highPotentialWorth = 0;

	public void initialize()
	{
		goldWorth = Random.Range (lowPotentialWorth, highPotentialWorth);
	}

	public int getWorth(){return goldWorth;}
}
