using UnityEngine;
using System.Collections;

public class PickUIScript : MonoBehaviour 
{
	public int myType;
	private GameManager myManager;

	// Use this for initialization
	void Start () 
	{
		myManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
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
		myManager.NewPick(myType);
	}
}
