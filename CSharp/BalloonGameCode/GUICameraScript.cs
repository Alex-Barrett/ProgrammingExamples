using UnityEngine;
using System.Collections;

public class GUICameraScript : MonoBehaviour 
{
	private Camera myCam;

	// Use this for initialization
	void Start () 
	{
		myCam = GetComponent<Camera>();
		//myCam.orthographicSize = myCam.pixelWidth/2;
		//myCam.transform.position = new Vector3(Screen.width/2,Screen.height/2,transform.position.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
