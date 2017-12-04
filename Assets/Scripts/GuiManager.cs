using UnityEngine;
using UnityEngine.SceneManagement;

public class GuiManager : MonoBehaviour
{

	private int health;
	public int score;
	public UnityEngine.UI.Text scoreText;
	private int maxHearts = 5;
	private int startHearts = 5;
	public int curHealth;
	private int maxHealth;
	private int healthPerHeart = 1;

	public UnityEngine.UI.Image[] healthImages;
	public Sprite[] healthSprites;

	private GameObject[] GOObjects;
	//private GameObject[] WINObjects;
	
	
	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		
		//hideWinMenu();
		health = 5;
		score = 0;
		SetScoreText();
		curHealth = startHearts * healthPerHeart;
		maxHealth = maxHearts * healthPerHeart;
		checkHealthAmount();
		UpdateHearts();
		GOObjects = GameObject.FindGameObjectsWithTag("GOObjects");
		hideGOMenu();

	}
	
	// Update is called once per frame
	void Update () {
     		
     }
	
	void UpdateHearts()
	{

		for (int n = 0; n < 5; n++)
		{
			if (curHealth-1 < n)
			{
				healthImages[n].sprite = healthSprites[0];
			}
			else
			{

				healthImages[n].sprite = healthSprites[1];
               
			}
			if (curHealth <= 0)
			{
				showGOMenu();
			}
		}
	}
	
	void checkHealthAmount()
	{
		for (int i = 0; i < maxHearts; i++)
		{
			if (startHearts <= i)
			{
				healthImages[i].enabled = false;
			}
			else
			{
				healthImages[i].enabled = true;
			}
		}
	}

	public void TakeDamage()
	{
		curHealth--;
		health--;
		UpdateHearts();
	}
	
	void SetScoreText()
	{
		scoreText.text = score.ToString();
	}
	
	void showGOMenu()
	{
		foreach (GameObject g in GOObjects)
		{
			g.SetActive(true);
		}
	}

	void hideGOMenu()
	{
		foreach (GameObject g in GOObjects)
		{
			g.SetActive(false);
		}
	}

	/*void showWinMenu()
	{
		foreach (GameObject g in WinObjects)
		{
			g.SetActive(true);
		}
	}

	void hideWinMenu()
	{
		foreach (GameObject g in WinObjects)
		{
			g.SetActive(false);
		}
	}*/
	
	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
