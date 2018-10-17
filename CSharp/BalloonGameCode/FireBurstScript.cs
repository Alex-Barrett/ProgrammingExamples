using UnityEngine;
using System.Collections;

public class FireBurstScript : MonoBehaviour 
{
	private float randTime;
	private float currTime = 0f;


	// Use this for initialization
	void Start () 
	{
		randTime = Random.Range(2f,5f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		currTime += Time.deltaTime;
		if(currTime >= randTime)
		{
			StartCoroutine(Burst());
			currTime = 0f;
		}
	}

	IEnumerator Burst()
	{
		while(transform.localScale.x < 1)
		{
			transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime ,transform.localScale.y + Time.deltaTime * 2,1);
			yield return null;
		}
		while(transform.localScale.x > 0.2f)
		{
			transform.localScale = new Vector3(transform.localScale.x  - Time.deltaTime ,transform.localScale.y - Time.deltaTime * 2,1);
			yield return null;
		}
		transform.localScale = new Vector3(0.2f,0.2f,1);
	}

}
