using UnityEngine;
using System.Collections;

public class FaceScript : MonoBehaviour 
{
	public enum BalloonType {Standard, Leather, Sniper, Bomber, Swarmer,Enemy};
	public BalloonType myType;
	public Texture2D myIdle;
	public Texture2D myAngry;
	public Texture2D myRage;
	public AnimateTiledTexture myTiler;
	private Renderer myRend;

	// Use this for initialization
	void Start () 
	{
		myRend = GetComponent<Renderer>();
		InvokeRepeating("DistCheck",0.5f,0.5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	private void DistCheck()
	{
		Collider2D hit = Physics2D.OverlapCircle(transform.position, 50f, 1<<13);
		if(hit !=null)
		{
			Collider2D newHit = Physics2D.OverlapCircle(transform.position, 20f, 1<<13);
			if(newHit != null)
			{
				myRend.material.mainTexture = myRage;

			}
			else
			{
				myRend.material.mainTexture = myAngry;

			}
		}
		else if(myRend.material.mainTexture != myIdle)
		{
			myRend.material.mainTexture = myIdle;

		}
	}

}
