using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{

	public Texture2D myIdle;
	public Texture2D myAngry;
	public Texture2D myRage;
	public AnimateTiledTexture myTiler;
	private Renderer myRend;


	// Use this for initialization
	void Start () 
	{
		foreach(Transform child in transform)
		{
			if(child.name.Equals("Face"))
			{
				myRend = child.GetComponent<Renderer>();
				myTiler = child.GetComponent<AnimateTiledTexture>();
			}
		}
		InvokeRepeating("DistCheck",0.5f,0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void CallDestroy()
	{
		GameManager.myManager.Score();
	}

	void DistCheck()
	{
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 50f, 1<<12);
		if(hit !=null)
		{
			Collider2D newHit = Physics2D.OverlapCircle(transform.position, 20f, 1<<12);
			if(newHit != null)
			{
				myRend.material.mainTexture = myRage;
			}
			else
			{
				myRend.material.mainTexture = myAngry;
			}
		}
		else
		{
			myRend.material.mainTexture = myIdle;
		}
	}

}
