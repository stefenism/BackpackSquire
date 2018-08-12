using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : ItemBlock {

	public int healValue;

	public override void checkDrag()
	{
		base.checkDrag ();
		if (isMouseOver () && !isDragging() && GameManager.GetCurrentItem() != this)
		{
			if (Input.GetMouseButtonDown (1))
			{
				doHeal ();
			}
		}
	}

	public override void checkSnap()
	{
		base.checkSnap ();
		if (isMouseOver () && GameManager.GetCurrentItem() != this)
		{
			if (Input.GetMouseButtonDown (1))
				doHeal ();
		}
	}

	void doHeal()
	{
		removeItem ();
		GameManager.changeHealth (healValue);
		Destroy (transform.parent.gameObject);
	}
}
