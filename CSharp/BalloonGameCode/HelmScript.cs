using UnityEngine;
using System.Collections;

public class HelmScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		Invoke("DestroyMe",10f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void PassVelocity(Vector2 speed)
	{
		GetComponent<Rigidbody2D>().velocity = speed;
	}

	private void DestroyMe()
	{
		Destroy(gameObject);
	}
}
