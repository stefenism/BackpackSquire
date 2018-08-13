using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	public Text winScore;

	const string MAINSCENENAME = "MainScene";

	void Awake()
	{
		winScore.text = "Gold:" + GameManager.getGold ();
	}
}
