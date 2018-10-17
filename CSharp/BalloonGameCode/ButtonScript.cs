using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour 
{
	public enum ButtoneType{Charge,Launch,Pick,Menu,Play,Again,Special};
	public ButtoneType myType;
	public GameManager myManager;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(myType != ButtoneType.Charge)
		{
			if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				//Ray2D ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				Collider2D hit = Physics2D.OverlapPoint(Input.GetTouch(0).position);
				if(hit != null)
				{
					if(hit.gameObject == this.gameObject)
					{
						OnMouseOver();
					}
				}
			}
		}
		else
		{
			if(Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)
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

	}

	void OnMouseDown()
	{

        if (myType == ButtoneType.Launch)
        {
            if (myManager != null)
            {
                myManager.Launch();
            }
        }
        else if (myType == ButtoneType.Pick)
        {
            if (myManager != null)
            {
                myManager.RePick();
            }
        }
        else if (myType == ButtoneType.Menu)
        {
            Application.LoadLevel("MainMenu");
        }
        else if (myType == ButtoneType.Play)
        {
            LevelManager.levelManager.LoadNext();
        }
        else if (myType == ButtoneType.Again)
        {
            GameManager.myManager.ReTry();
        }
        else if (myType == ButtoneType.Special)
        {
            GameManager.myManager.TrySpecial();
        }
	}

	void OnMouseOver()
	{
		if(Input.GetMouseButton(0))
		{
			if(myType == ButtoneType.Charge)
			{
				if(myManager != null)
				{
					myManager.ChargeUp();
				}
			}
		}
		else if(Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)
		{
			if(myType == ButtoneType.Charge)
			{
				if(myManager != null)
				{
					myManager.ChargeUp();
				}
			}
		}
	}

}
