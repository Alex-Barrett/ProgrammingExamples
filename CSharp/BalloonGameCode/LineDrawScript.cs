using UnityEngine;
using System.Collections.Generic;

public class LineDrawScript : MonoBehaviour 
{

	List<Vector3> linePoints = new List<Vector3>();
	LineRenderer lineRenderer;
	public float startWidth = 2.0f;
	public float endWidth = 2.0f;
	public float threshold = 0.5f;
	Camera thisCamera;
	int lineCount = 0;
	bool drawReady = false;
	
	Vector3 lastPos = Vector3.one * float.MaxValue;
	
	
	void Awake()
	{
		thisCamera = Camera.main;
		lineRenderer = GetComponent<LineRenderer>();
	}

	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetMouseButton(0) && drawReady)
		{
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = thisCamera.transform.position.z-9f-thisCamera.nearClipPlane;
			Vector3 mouseWorld = thisCamera.ScreenToWorldPoint(mousePos);

			
			
			float dist = Vector3.Distance(lastPos, mouseWorld);
			if(dist <= threshold)
			{
				
				return;
			}
			
			lastPos = mouseWorld;
			if(linePoints == null)
			{
				linePoints = new List<Vector3>();
				
			}
			linePoints.Add(mouseWorld);
			UpdateLine();
		}

	}

	void UpdateLine()
	{
		lineRenderer.SetWidth(startWidth, endWidth);
		lineRenderer.SetVertexCount(linePoints.Count);
		Vector3 temp = Vector3.zero;
		
		for(int i = lineCount; i < linePoints.Count; i++)//Change this?
		{
			temp = linePoints[i];
			temp.z = 0.7f;
			lineRenderer.SetPosition(i, temp);
			//Debug.Log("Line Draw");
		}
		lineCount = linePoints.Count;
	}

	public void SetReady(bool x)
	{
		drawReady = x;
	}

	public void ResetLines()
	{
		lineRenderer.SetVertexCount(0);
		linePoints = new List<Vector3>();
		lineCount = 0;
	}

	public List<Vector3> GetLines()
	{
		return linePoints;
	}

}
