using UnityEngine;
using System.Collections;

public class DeadBalloonScript : MonoBehaviour 
{
	public enum MyType {Standard,Leather,Sniper,Bomber,Swarmer};
	public MyType type;
	private Rigidbody2D myBody;
	private bool called = false;


	// Use this for initialization
	void Start () 
	{
		myBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(myBody.velocity.sqrMagnitude <= 0.1f && !called)
		{
			if(type != MyType.Swarmer)
			{
				GameManager.myManager.CallReset();//What if swarmer?
				called = true;
			}

		}
	}

	public void PassVelocity(Vector2 speed)
	{
		GetComponent<Rigidbody2D>().velocity = speed;
	}
}
