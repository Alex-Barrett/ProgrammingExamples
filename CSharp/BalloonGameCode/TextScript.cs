using UnityEngine;
using System.Collections;

public class TextScript : MonoBehaviour 
{
	public TextMesh myTextMesh;
	private float time = 1f;
	private Color newColour;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		time += Time.deltaTime;
		if(time >= 1f)
		{
			newColour = ChangeColour();
			time = 0;
		}
		myTextMesh.color = new Color(Mathf.Lerp(myTextMesh.color.r,newColour.r, Time.deltaTime),Mathf.Lerp(myTextMesh.color.g,newColour.g, Time.deltaTime),Mathf.Lerp(myTextMesh.color.b,newColour.b, Time.deltaTime));
	}

	private Color ChangeColour()
	{
		Color randColour = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
		return randColour;
	}

}
