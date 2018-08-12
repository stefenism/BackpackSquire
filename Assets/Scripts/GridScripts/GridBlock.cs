using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : GridObject {

	public AudioClip gridSelectSound;

	public override void Update () {
		base.Update ();
	}

	public override void doIdleState()
	{
		base.doIdleState();
		MouseUtilities.currentGridPoint = MouseUtilities.invalidGridPoint;
	}

	public override void doMouseOverState()
	{
		base.doMouseOverState ();
		MouseUtilities.currentGridPoint = transform.position;
		checkSnap ();

		AudioManager.playSfx (gridSelectSound);
	}

	void checkSnap()
	{
		ItemBlock currentItem = GameManager.GetCurrentItem ();
		if (currentItem != null && currentItem.isDragging())
		{
			currentItem.SnapPosition (transform.position);
			currentItem.setSnapped ();
		}

		if (currentItem != null && currentItem.isSnapped ())
		{			
			currentItem.SnapPosition (transform.position);
		}
	}
}
