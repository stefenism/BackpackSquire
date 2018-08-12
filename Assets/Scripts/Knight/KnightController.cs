using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour {

	Animator anim;

	void Awake()
	{
		anim = GetComponent<Animator> ();
	}

	public void setAsDead()
	{
		anim.SetBool ("Dead", true);
	}
}
