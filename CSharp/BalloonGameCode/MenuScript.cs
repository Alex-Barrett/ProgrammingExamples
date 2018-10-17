using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour 
{
	public GameObject[] mainMenu;
	public GameObject[] selectMenu;
	public GameObject[] selectLevel;
	private LevelManager levelManager;
	private int nextLevel;
	private int nextChapter;
	private int selectedChapter = 1;

	// Use this for initialization
	void Start () 
	{
		levelManager = LevelManager.levelManager;
		Select();
		LoadStuff();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void LoadStuff()
	{
		nextLevel = levelManager.GetNextLevel();
		nextChapter = levelManager.GetNextChapter();
	}

	public void Play()
	{
		levelManager.LoadNext();
	}

	public void Select()
	{
		for(int i = 0; i <= selectMenu.Length - 1; i++)
		{
			selectMenu[i].SetActive(!selectMenu[i].activeInHierarchy);
		}
		for(int i = 0; i <= selectLevel.Length - 1; i++)
		{
			selectLevel[i].SetActive(!selectLevel[i].activeInHierarchy);
		}
		Chapter(selectedChapter);
	}

	public void Chapter(int x)
	{
		selectedChapter = x;
		if(selectedChapter == nextChapter)
		{
			for(int i = 0; i <= selectLevel.Length - 1; i++)
			{
				selectLevel[i].GetComponent<MenuButtonScript>().SwapTexture(nextChapter,nextLevel);
			}
		}
		else if(selectedChapter < nextChapter)
		{
			for(int i = 0; i <= selectLevel.Length - 1; i++)
			{
				selectLevel[i].GetComponent<MenuButtonScript>().SwapTexture(selectedChapter,9);
			}
		}
		else if(selectedChapter > nextChapter)
		{
			for(int i = 0; i <= selectLevel.Length - 1; i++)
			{
				selectLevel[i].GetComponent<MenuButtonScript>().SwapTexture(selectedChapter,0);
			}
		}
	}

	public void Level(int x)
	{
		levelManager.LoadCustom(selectedChapter,x);
	}

}
