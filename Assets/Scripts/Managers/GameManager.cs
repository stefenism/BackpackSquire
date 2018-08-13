using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager gameDaddy = null;

	public ItemBlock currentItem;
	public List<ItemBlock> currentItems = new List<ItemBlock>();
	public KnightController knight;
	public OffsetScroll background;
	public Slider healthSlider;
	public TextMesh goldText;
	private List<GridBlock> currentGrid = new List<GridBlock> ();

	private int currentGoldValue = 0;
	private int currentHealth = 100;
	private float gameTimer = 0;
	private float healthTimer = 0;

	const string ENDSCREENSTRING = "WinScreen";

	void Awake()
	{		
		Initialize ();
	}

	void Update()
	{
		gameTimer += Time.deltaTime;
		healthTimer += Time.deltaTime;

		if (healthTimer >= 1)
		{
			healthTimer = 0;
			changeHealth (-1);
		}			
	}

	void Initialize()
	{
		if (gameDaddy == null)
			gameDaddy = this;
		else if (gameDaddy == this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);

		gameDaddy.currentGoldValue = 0;
		gameDaddy.currentHealth = 100;
	}

	static public void setCurrentItem(ItemBlock newItem)
	{
		gameDaddy.currentItem = newItem;
	}

	static public bool isSpaceAvailable(Vector2 newPosition)
	{
		foreach(ItemBlock item in gameDaddy.currentItems)
		{
			if (item.getCollider ().bounds.Contains (newPosition))
				return false;
		}
		return true;
	}

	static public bool IsInCurrentList(ItemBlock newItem)
	{
		if (gameDaddy.currentItems.Contains (newItem))
			return true;
		return false;
	}

	static public void addGold(int scrilla)
	{
		gameDaddy.currentGoldValue += scrilla;
		gameDaddy.currentGoldValue = Mathf.Clamp (gameDaddy.currentGoldValue, 0, System.Int32.MaxValue);

		gameDaddy.goldText.text = "$" + gameDaddy.currentGoldValue;
	}

	static public void changeHealth(int difference)
	{
		gameDaddy.currentHealth += difference;
		gameDaddy.currentHealth = Mathf.Clamp (gameDaddy.currentHealth, 0, 100);

		gameDaddy.healthSlider.value = gameDaddy.currentHealth / 100f;

		if (gameDaddy.currentHealth <= 0)
			gameDaddy.gameOver ();
	}

	void gameOver()
	{
		knight.setAsDead ();
		background.stopScroll ();

		Invoke ("switchScreen", 3);
	}

	void switchScreen()
	{
		Destroy (AudioManager.audioDaddy.gameObject);
		Destroy (GridManager.gridDaddy.gameObject);
		Destroy (gameObject);
		SceneManager.LoadScene (ENDSCREENSTRING, LoadSceneMode.Single);
	}

	static public int getGold(){return gameDaddy.currentGoldValue;}
	static public float getGameTimer(){return gameDaddy.gameTimer;}
		
	static public ItemBlock GetCurrentItem(){return GameManager.gameDaddy.currentItem;}
	static public void AddCurrentItemToList(ItemBlock newItem){gameDaddy.currentItems.Add (newItem);}
}
