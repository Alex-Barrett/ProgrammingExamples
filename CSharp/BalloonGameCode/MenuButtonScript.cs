using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonScript : MonoBehaviour 
{
	public enum ButtonType {Play,Select,Chapter,Level};
	public ButtonType myType;
	public Sprite mainTextrure;
	public Sprite altTexture;
	public Sprite dimTexture;
	public Sprite[] chapterOneTextures;
	public Sprite[] chapterTwoTextures;
	public Sprite[] chapterThreeTextures;
	public Sprite[] chapterFourTextures;
	public Sprite[] chapterFiveTextures;
	public MenuScript myMenu;
	public int myNum;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	/*
	void Update () 
	{
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			//Ray2D ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			Collider2D hit = Physics2D.OverlapPoint(Input.GetTouch(0).position);
			if(hit != null)
			{
				if(hit.gameObject == this.gameObject)
				{
					OnMouseDown();
				}
			}
		}
	}

	void OnMouseDown()
	{
		if(myType == ButtonType.Play)
		{
			myMenu.Play();
		}
		else if(myType == ButtonType.Select)
		{
			myMenu.Select();
		}
		else if(myType == ButtonType.Chapter)
		{
			myMenu.Chapter(myNum);
		}
		else if(myType == ButtonType.Level)
		{
			if(GetComponent<Renderer>().material.mainTexture != dimTexture)
			{
				myMenu.Level(myNum);
			}

		}
	}
*/
	public void SwapTexture(int newChapter, int newLevel)
	{
		switch(newChapter)
		{
		case 1:
			mainTextrure = chapterOneTextures[0];
			altTexture = chapterOneTextures[1];
			dimTexture = chapterOneTextures[2];
			break;
		case 2:
			mainTextrure = chapterTwoTextures[0];
			altTexture = chapterTwoTextures[1];
			dimTexture = chapterTwoTextures[2];
			break;
		case 3:
			mainTextrure = chapterThreeTextures[0];
			altTexture = chapterThreeTextures[1];
			dimTexture = chapterThreeTextures[2];
			break;
		case 4:
			mainTextrure = chapterFourTextures[0];
			altTexture = chapterFourTextures[1];
			dimTexture = chapterFourTextures[2];
			break;
		case 5:
			mainTextrure = chapterFiveTextures[0];
			altTexture = chapterFiveTextures[1];
			dimTexture = chapterFiveTextures[2];
			break;
		}
		if(newLevel == myNum)
		{
			//GetComponent<Renderer>().material.mainTexture = mainTextrure;
			GetComponent<Image>().overrideSprite = mainTextrure;
		}
		else if(newLevel > myNum)
		{
			GetComponent<Image>().overrideSprite = altTexture;
		}
		else if(newLevel < myNum)
		{
			GetComponent<Image>().overrideSprite = dimTexture;
		}
	}

}
