using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour 
{
	private Vector2 myPos;
	private Vector3 myScale;
	private Material myMat;

	// Use this for initialization
	void Start ()
	{
		myPos = transform.position;
		myScale = transform.localScale;
		myMat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Gas(float curr, float max)
	{
		float percent = curr/max;
		transform.localScale = new Vector3(myScale.x * percent,myScale.y,myScale.z);
		transform.position = new Vector3(myPos.x + myScale.x/2 - transform.localScale.x/2,transform.position.y,transform.position.z);
		myMat.color = new Color((1-percent)*2,percent * 2f,0);
	}
}
