using UnityEngine;
using System.Collections;

public class GustScript : MonoBehaviour 
{
	public Rigidbody2D myBody;

	// Use this for initialization
	void Start () 
	{
		//myBody = GetComponent<Rigidbody2D>();
	}
	

	// Update is called once per frame
	void FixedUpdate () 
	{
		if(gameObject.activeInHierarchy)
		{
			//transform.Translate(0,-0.5f,0);
			//myBody.AddForce(new Vector2(0,-1000),ForceMode2D.Force);
			myBody.velocity = new Vector2(0,-25);
		}
	}

	void OnCollisionEnter2D(Collision2D c)
	{
		if(!c.gameObject.CompareTag("Cloud"))
		{
			if(c.gameObject.CompareTag("Balloon"))
			{
				//c.gameObject.GetComponent<Rigidbody2D>().AddForce();
			}
			gameObject.SetActive(false);
		}

	}
}
