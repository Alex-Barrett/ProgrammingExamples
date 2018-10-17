using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	private Transform target;
	private Camera myCam;
	private bool live;

	// Use this for initialization
	void Start ()
	{
		//target = GameObject.FindGameObjectWithTag("Balloon").transform;
		myCam = this.GetComponent<Camera>();
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		Resolution[] resolutions = Screen.resolutions;
		//Screen.SetResolution(resolutions[0].width,resolutions[0].height,true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(live)
		{
			Movement();
		}
		else
		{
			myCam.orthographicSize = 40f;
		}
	}

	private void Movement()
	{
		if(target != null)
		{
			myCam.orthographicSize = 20f;
			transform.position = new Vector3(target.transform.position.x, target.transform.position.y,transform.position.z);
			transform.position = new Vector3(Mathf.Clamp(transform.position.x,-28.5f,28.5f),Mathf.Clamp(transform.position.y,-20f,20f),transform.position.z);
		}
	}

	public void SetLive(bool x, Transform newTarget)
	{
		live = x;
		target = newTarget;
	}

}
