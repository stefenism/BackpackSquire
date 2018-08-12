using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : GridObject {

	public Color conveyorSelectedColor = new Color (.5f, .5f, .5f);

	public override void doMouseOverState()
	{
		sprite.color = conveyorSelectedColor;
	}
}
