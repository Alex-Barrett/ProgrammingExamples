using UnityEngine;
using System.Collections;


public class PathFinderScript : MonoBehaviour 
{

	private int[] openSides;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool FindPath()
	{
		System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
		timer.Start();
		RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.right,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("Path"))
			{
				Debug.Log("hit");
				foreach(Transform child in hit.collider.transform)
				{
					if(child.CompareTag("Caster"))
					{
						GameObject[] temp = new GameObject[]{gameObject};
						if(child.GetComponent<PathFinderScript>().ContinuePath(temp))
						{
							timer.Stop();
							Debug.Log(timer.Elapsed);
							return true;
						}
					}
				}
			}
		}
		else
		{
			return true;
		}
		return false;
	}
	public bool ContinuePath(GameObject[] caller)
	{
		int oldLayer = transform.parent.gameObject.layer;
		transform.parent.gameObject.layer = 2;
		RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.right,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}

			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					foreach(Transform child in hit.collider.transform)
					{
						if(child.CompareTag("Caster"))
						{
							GameObject[] temp = new GameObject[caller.Length + 1];
							for(int i = 0; i <= caller.Length - 1; i++)
							{
								temp[i] = caller[i];
							}
							temp[temp.Length - 1] = transform.parent.gameObject;
							transform.parent.gameObject.layer = oldLayer;
							if(child.GetComponent<PathFinderScript>().ContinuePath(temp))
							{
								return true;
							}
						}
					}
				}
			}
		}
		else
		{
			transform.parent.gameObject.layer = oldLayer;
			return true;
		}
		transform.parent.gameObject.layer = 2;
		hit = Physics2D.Raycast(transform.position,transform.up,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					foreach(Transform child in hit.collider.transform)
					{
						if(child.CompareTag("Caster"))
						{
							GameObject[] temp = new GameObject[caller.Length + 1];
							for(int i = 0; i <= caller.Length - 1; i++)
							{
								temp[i] = caller[i];
							}
							temp[temp.Length - 1] = transform.parent.gameObject;
							transform.parent.gameObject.layer = oldLayer;
							if(child.GetComponent<PathFinderScript>().ContinuePath(temp))
							{
								return true;
							}
						}
					}
				}
			}
		}
		transform.parent.gameObject.layer = 2;
		hit = Physics2D.Raycast(transform.position,-transform.up,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					foreach(Transform child in hit.collider.transform)
					{
						if(child.CompareTag("Caster"))
						{
							GameObject[] temp = new GameObject[caller.Length + 1];
							for(int i = 0; i <= caller.Length - 1; i++)
							{
								temp[i] = caller[i];
							}
							temp[temp.Length - 1] = transform.parent.gameObject;
							transform.parent.gameObject.layer = oldLayer;
							if(child.GetComponent<PathFinderScript>().ContinuePath(temp))
							{
								return true;
							}
						}
					}
				}
			}
		}
		transform.parent.gameObject.layer = 2;
		hit = Physics2D.Raycast(transform.position,transform.right,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					foreach(Transform child in hit.collider.transform)
					{
						if(child.CompareTag("Caster"))
						{
							GameObject[] temp = new GameObject[caller.Length + 1];
							for(int i = 0; i <= caller.Length - 1; i++)
							{
								temp[i] = caller[i];
							}
							temp[temp.Length - 1] = transform.parent.gameObject;
							transform.parent.gameObject.layer = oldLayer;
							if(child.GetComponent<PathFinderScript>().ContinuePath(temp))
							{
								return true;
							}
						}
					}
				}
			}
		}

		transform.parent.gameObject.layer = oldLayer;
		return false;
	}

	public void CheckSides()
	{
		openSides = new int[4];
		int oldLayer = transform.parent.gameObject.layer;
		transform.parent.gameObject.layer = 2;
		RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.right,3f,1<<8);
		if(hit.collider != null)
		{
			openSides[0] = 0;
		}
		else 
		{
			openSides[0] = 1;
		}
		hit = Physics2D.Raycast(transform.position,transform.up,3f,1<<8);
		{
			if(hit.collider != null)
			{
				openSides[1] = 0;
			}
			else 
			{
				openSides[1] = 1;
			}
		}
		hit = Physics2D.Raycast(transform.position,-transform.right,3f,1<<8);
		{
			if(hit.collider != null)
			{
				openSides[2] = 0;
			}
			else 
			{
				openSides[2] = 1;
			}
		}
		hit = Physics2D.Raycast(transform.position,-transform.up,3f,1<<8);
		{
			if(hit.collider != null)
			{
				openSides[3] = 0;
			}
			else 
			{
				openSides[3] = 1;
			}
		}
		transform.parent.gameObject.layer = oldLayer;
	}

	public bool ThreatValid()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.right,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("Path"))
			{
				return true;
			}
		}
		else if(hit.collider == null)
		{
			return true;
		}
		hit = Physics2D.Raycast(transform.position,transform.up,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("Path"))
			{
				return true;
			}
		}
		else if(hit.collider == null)
		{
			return true;
		}
		hit = Physics2D.Raycast(transform.position,-transform.up,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("Path"))
			{
				return true;
			}
		}
		else if(hit.collider == null)
		{
			return true;
		}
		hit = Physics2D.Raycast(transform.position,transform.right,Mathf.Infinity,1<<8);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("Path"))
			{
				return true;
			}
		}
		else if(hit.collider == null)
		{
			return true;
		}
		return false;
	}

	public bool CastleValid(GameObject[] caller)
	{
		int oldLayer = gameObject.layer;
		gameObject.layer = 2;
		RaycastHit2D hit = Physics2D.Raycast(transform.position,transform.right,Mathf.Infinity,1<<10);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					GameObject[] temp = new GameObject[caller.Length + 1];
					for(int i = 0; i <= caller.Length - 1; i++)
					{
						temp[i] = caller[i];
					}
					temp[temp.Length - 1] = gameObject;
					gameObject.layer = oldLayer;
					if(hit.collider.GetComponent<PathFinderScript>().CastleValid(temp))
					{

						return true;
					}
				}
			}

		}
		else if(hit.collider == null)
		{
			gameObject.layer = oldLayer;
			return true;
		}
		hit = Physics2D.Raycast(transform.position,transform.up,Mathf.Infinity,1<<10);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					GameObject[] temp = new GameObject[caller.Length + 1];
					for(int i = 0; i <= caller.Length - 1; i++)
					{
						temp[i] = caller[i];
					}
					temp[temp.Length - 1] = gameObject;
					gameObject.layer = oldLayer;
					if(hit.collider.GetComponent<PathFinderScript>().CastleValid(temp))
					{
						return true;
					}
				}
			}
		}
		else if(hit.collider == null)
		{
			gameObject.layer = oldLayer;
			return true;
		}
		hit = Physics2D.Raycast(transform.position,-transform.up,Mathf.Infinity,1<<10);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					GameObject[] temp = new GameObject[caller.Length + 1];
					for(int i = 0; i <= caller.Length - 1; i++)
					{
						temp[i] = caller[i];
					}
					temp[temp.Length - 1] = gameObject;
					gameObject.layer = oldLayer;
					if(hit.collider.GetComponent<PathFinderScript>().CastleValid(temp))
					{
						return true;
					}
				}
			}
		}
		else if(hit.collider == null)
		{
			gameObject.layer = oldLayer;
			return true;
		}
		hit = Physics2D.Raycast(transform.position,-transform.right,Mathf.Infinity,1<<10);
		if(hit.collider != null)
		{
			bool x = true;
			for(int i = 0; i <= caller.Length - 1; i++)
			{
				if(hit.collider.gameObject == caller[i])
				{
					x = false;
					break;
				}
			}
			if(x)
			{
				if(hit.collider.CompareTag("Path"))
				{
					GameObject[] temp = new GameObject[caller.Length + 1];
					for(int i = 0; i <= caller.Length - 1; i++)
					{
						temp[i] = caller[i];
					}
					temp[temp.Length - 1] = gameObject;
					gameObject.layer = oldLayer;
					if(hit.collider.GetComponent<PathFinderScript>().CastleValid(temp))
					{
						return true;
					}
				}
			}
		}
		else if(hit.collider == null)
		{
			gameObject.layer = oldLayer;
			return true;
		}
		gameObject.layer = oldLayer;
		return false;
	}

	public bool CastleSides()
	{
		//Debug.Log("I am");
		int oldLayer = gameObject.layer;
		gameObject.layer = 2;
		RaycastHit2D hit = Physics2D.Raycast(transform.position,-transform.right,5f,1<<10);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("CastleOptional") || hit.collider.CompareTag("CastleStatic"))
			{
				gameObject.layer = oldLayer;
				return true;
			}
		}
		hit = Physics2D.Raycast(transform.position,transform.up,5f,1<<10);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("CastleOptional"))
			{
				gameObject.layer = oldLayer;
				return true;
			}
		}

		hit = Physics2D.Raycast(transform.position,-transform.up,5f,1<<10);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("CastleOptional"))
			{
				gameObject.layer = oldLayer;
				return true;
			}
		}
		hit = Physics2D.Raycast(transform.position,transform.right,5f,1<<10);
		if(hit.collider != null)
		{
			if(hit.collider.CompareTag("CastleOptional"))
			{
				gameObject.layer = oldLayer;
				return true;
			}
		}
		gameObject.layer = oldLayer;
		return false;
	}

	public void BlockSide(int side)
	{
		openSides[side] = 0;
	}

	public int[] GetSides()
	{
		int[] temp = new int[openSides.Length]; 
		System.Array.Copy(openSides, temp, openSides.Length);
		return temp;
	}
}
