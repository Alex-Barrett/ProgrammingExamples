using UnityEngine;
using System.Collections;


public class ThreatClass 
{


	private int[] openSides;
	private enum ThreatType {Free,Verticle,Horizontal,Radial,RadialDouble};
	private ThreatType myType;
	private Vector2 direction;
	private float distance;
	// Use this for initialization
	

	public void Action(Transform me)
	{
		if(myType == ThreatType.Free)
		{

		}
		else if(myType == ThreatType.Horizontal)
		{

		}
		else if(myType == ThreatType.Verticle)
		{

		}
        else
        {

        }
	}

	public void SetDirection(Transform location)
	{
		float[] distances = new float[4];
		RaycastHit2D hit = Physics2D.Raycast(location.position, location.right, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[0] = hit.distance - location.localScale.x;
			Debug.Log("GOOGLY");
		}
		else
		{
			distances[0] = 30f - location.position.x - location.localScale.x; 
		}
		hit = Physics2D.Raycast(location.position, location.up, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[1] = hit.distance - location.localScale.x;
		}
		else
		{
			distances[1] = 22.5f - location.position.y - location.localScale.x;
		}
		hit = Physics2D.Raycast(location.position, -location.right, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[2] = hit.distance - location.localScale.x;
		}
		else
		{
			distances[2] = -30f - location.position.x - location.localScale.x;
		}
		hit = Physics2D.Raycast(location.position, -location.up, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[3] = hit.distance - location.localScale.x;
		}
		else
		{
			distances[3] = -17.5f - location.position.y - location.localScale.x;
		}
		distance = 0f;
		for(int i = 0; i <= distances.Length - 1; i++)
		{
			if(Mathf.Abs(distances[i]) > distance)
			{
				Debug.Log(Mathf.Abs(distances[i]));
				distance = Mathf.Abs(distances[i]);
				if(i == 0)
				{
					direction = location.right;
				}
				else if(i == 1)
				{
					direction = location.up;
				}
				else if(i == 2)
				{
					direction = -location.right;
				}
				else if(i == 3)
				{
					direction = -location.up;
				}
			}
		}
		Debug.Log(direction);
	}

	public Transform SetThreat(Transform location)
	{
		openSides = new int[4];//left,up,right,down
		float[] distances = new float[4];
        bool canRadial = false;

		RaycastHit2D hit = Physics2D.Raycast(location.position, location.right, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[0] = hit.distance;
		}
		else
		{
			distances[0] = 100f; //change this
		}

		hit = Physics2D.Raycast(location.position, location.up, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[1] = hit.distance;
		}
		else
		{
			distances[1] = 100f; //change this
		}

		hit = Physics2D.Raycast(location.position, -location.right, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[2] = hit.distance;
		}
		else
		{
			distances[2] = 100f; //change this
		}

		hit = Physics2D.Raycast(location.position, -location.up, Mathf.Infinity, 1<<8);
		if(hit.collider != null)
		{
			distances[3] = hit.distance;

		}
		else
		{
			distances[3] = 100f; //change this
		}

        Collider2D newHits = Physics2D.OverlapCircle(location.position, 5f, 1 << 8);//Set Radial
        if(newHits == null)
        {
            canRadial = true;
            //Look at this later
            int newNum = Random.RandomRange(0, 2);
            if(newNum == 0)
            {
                newHits = Physics2D.OverlapCircle(location.position, 10f, 1 << 8);
                if(newHits == null)
                {
                    myType = ThreatType.RadialDouble;
                }
                else
                {
                    myType = ThreatType.Radial;
                }
                return location;
            }
        }

		float max = 0;
		int num = -1;
		for(int i = 0; i <= distances.Length - 1; i++)
		{
			if(distances[i] > max)
			{
				max = distances[i];
				num = i;
			}
		}
		switch(num)
		{
		case 0:
			hit = Physics2D.Raycast(location.position, -location.right, Mathf.Infinity, 1<<8); //Find closest anchor to the right
			if(hit.collider != null)
			{
				int rand = Random.Range(0,2);
				if(rand == 0)
				{
					myType = ThreatType.Free;
					return location;
				}
				else
				{
					myType = ThreatType.Horizontal;
					location.position = hit.collider.transform.position;
					location.Translate(5f,0,0.5f);
					return location;
				}
			}
			else if(hit.collider == null)
			{
				hit = Physics2D.Raycast(location.position, location.right, Mathf.Infinity, 1<<8);//Nothing right, fire left
				if(hit.collider != null)
				{
					int rand = Random.Range(0,2);
					if(rand == 0)
					{
						myType = ThreatType.Free;
						return location;
					}
					else
					{
						myType = ThreatType.Horizontal;
						location.position = hit.collider.transform.position;
						location.Translate(-5f,0,0.5f);
						location.Rotate(0,0,180f);
						return location;
					}
				}
				else //Nothing, make it free
				{
					myType = ThreatType.Free;
					return location;
				}
			}
			break;
		case 1:
			hit = Physics2D.Raycast(location.position, -location.up, Mathf.Infinity, 1<<8); //Find closest down
			if(hit.collider != null)
			{
				int rand = Random.Range(0,2);
				if(rand == 0)
				{
					myType = ThreatType.Free;
					return location;
				}
				else
				{
					myType = ThreatType.Verticle;
					location.position = hit.collider.transform.position;
					location.Translate(0,5f,0.5f);
					location.Rotate(0,0,90);
					return location;
				}
			}
			else if(hit.collider == null) //Nothing, Find up
			{
				hit = Physics2D.Raycast(location.position, location.up, Mathf.Infinity, 1<<8);
				if(hit.collider != null)
				{
					int rand = Random.Range(0,2);
					if(rand == 0)
					{
						myType = ThreatType.Free;
						return location;
					}
					else
					{
						myType = ThreatType.Verticle;
						location.position = hit.collider.transform.position;
						location.Translate(0,-5f,0.5f);
						location.Rotate(0,0,-90);
						return location;
					}
				}
				else
				{
					myType = ThreatType.Free;
					return location;
				}
			}
			break;
		case 2:
			hit = Physics2D.Raycast(location.position, location.right, Mathf.Infinity, 1<<8); //Find closest left
			if(hit.collider != null)
			{
				int rand = Random.Range(0,2);
				if(rand == 0)
				{
					myType = ThreatType.Free;
					return location;
				}
				else
				{
					myType = ThreatType.Horizontal;
					location.position = hit.collider.transform.position;
					location.Translate(-5f,0,0.5f);
					location.Rotate(0,0,180);
					return location;
				}
			}
			else if(hit.collider == null) //Nothing, Find right
			{
				hit = Physics2D.Raycast(location.position, location.up, Mathf.Infinity, 1<<8);
				if(hit.collider != null)
				{
					int rand = Random.Range(0,2);
					if(rand == 0)
					{
						myType = ThreatType.Free;
						return location;
					}
					else
					{
						myType = ThreatType.Horizontal;
						location.position = hit.collider.transform.position;
						location.Translate(5f,0,0.5f);
						//location.Rotate(0,0,0);
						return location;
					}
				}
				else
				{
					myType = ThreatType.Free;
					return location;
				}
			}
			break;
		case 3:
			hit = Physics2D.Raycast(location.position, location.up, Mathf.Infinity, 1<<8); //Find closest up
			if(hit.collider != null)
			{
				int rand = Random.Range(0,2);
				if(rand == 0)
				{
					myType = ThreatType.Free;
					return location;
				}
				else
				{
					myType = ThreatType.Verticle;
					location.position = hit.collider.transform.position;
					location.Translate(0,-5f,0.5f);
					location.Rotate(0,0,-90);
					return location;
				}
			}
			else if(hit.collider == null) //Nothing, Find down
			{
				hit = Physics2D.Raycast(location.position, -location.up, Mathf.Infinity, 1<<8);
				if(hit.collider != null)
				{
					int rand = Random.Range(0,2);
					if(rand == 0)
					{
						myType = ThreatType.Free;
						return location;
					}
					else
					{
						myType = ThreatType.Verticle;
						location.position = hit.collider.transform.position;
						location.Translate(0,5f,0.5f);
						location.Rotate(0,0,90);
						return location;
					}
				}
				else
				{
					myType = ThreatType.Free;
					return location;
				}
			}
			break;
		}
		return null;
	}

	public string GetMyType()
	{
		return myType.ToString();
	}

	public int GetIntType()
	{
		return (int)myType;
	}

	public Vector2 GetDir()
	{
		return direction;
	}

	public float GetDist()
	{
		return distance;
	}

}
