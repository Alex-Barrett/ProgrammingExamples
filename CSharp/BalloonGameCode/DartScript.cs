using UnityEngine;
using System.Collections;

public class DartScript : MonoBehaviour 
{
	private float timer = 0;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//OverlapArea
		transform.Translate(1,0,0);
		timer += Time.deltaTime;
		if(timer >= 2f)
		{
			gameObject.SetActive(false);
			timer = 0f;
		}
	}

	void OnCollisionEnter(Collision c)
	{
		gameObject.SetActive(false);
	}
}
