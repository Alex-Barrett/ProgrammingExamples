using UnityEngine;
using System.Collections;

public class CloudScript : MonoBehaviour 
{
	public GameObject gust;
	public GameObject coldGust;
	public GameObject fireGust;
	private GameObject myGust;
	private float randTime;
	private float timer = 0f;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if(myGust != null && !myGust.activeInHierarchy)
		{
			timer += Time.deltaTime;
			if(timer >= randTime)
			{
				myGust.transform.position = transform.position;

				myGust.SetActive(true);
				//myGust.GetComponent<Rigidbody2D>().velocity = new Vector2(0,10);
				timer = 0;
			}
		}
		if(myGust != null && myGust.transform.rotation != transform.rotation)
		{
			myGust.transform.rotation = transform.rotation;
		}

	}

	public void SetGust(int chapter)
	{
		if(chapter < 4)
		{
			myGust = (GameObject)Instantiate(gust,transform.position,gust.transform.rotation);
		}
		else if(chapter == 4)
		{
			myGust = (GameObject)Instantiate(coldGust,transform.position,coldGust.transform.rotation);
		}
		else if(chapter == 5)
		{
			myGust = (GameObject)Instantiate(fireGust,transform.position,fireGust.transform.rotation);
		}

		myGust.SetActive(false);
		randTime = Random.Range(2,5);
	}

}
