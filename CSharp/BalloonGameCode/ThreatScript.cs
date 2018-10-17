using UnityEngine;
using System.Collections;

public class ThreatScript : MonoBehaviour
{
	private enum ThreatType {Free,Verticle,Horizontal,Radial,RadialDouble};
	private ThreatType myType;
	private ThreatClass myClass;
	public GameObject dart = null;
	private GameObject[] myDart = {null,null,null};
	private Vector2 dir;
	private Vector3 startPos;
	private Vector3 endPos;
    private Vector3 currentRot;
	private float dist;
	private float waitTime;
	private float timer = 0;
    private float rotSpeed = 10f;
    private int rotDir = 1;
	private bool live = false;
	private bool moving = true;
	// Use this for initialization


	void Start()
	{
		waitTime = Random.Range(1,4);

	}

	void Update()
	{
		if(live)
		{
			//myClass.Action(transform);
			Action();
		}
	}

	private void Action()
	{
        if (myType == ThreatType.Free)
        {
            if (moving)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime);
            }
            if (Vector3.Distance(transform.position, endPos) <= 0.5f)
            {
                moving = false;
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    Vector3 tempPos = endPos;
                    endPos = startPos;
                    startPos = tempPos;
                    moving = true;
                }
            }

        }
        else if (myType == ThreatType.Horizontal)
        {
            Shoot();
        }
        else if (myType == ThreatType.Verticle)
        {
            if (moving)
            {
                transform.position = Vector3.Lerp(transform.position, endPos, Time.deltaTime * 5f);
            }
            if (transform.position == endPos)
            {
                moving = false;
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    //Vector3 tempPos = endPos;
                    endPos = startPos;
                    startPos = transform.position;
                    moving = true;
                }
            }
        }
        else if (myType == ThreatType.Radial || myType == ThreatType.RadialDouble)
        {
            currentRot = transform.eulerAngles;
            currentRot.z = Mathf.Lerp(currentRot.z, currentRot.z + (10f * rotDir), Time.deltaTime * rotSpeed);
            transform.eulerAngles = currentRot;
        }
	}

	private void Shoot()
	{
		for(int i = 0; i <= myDart.Length - 1; i++)
		{
			if(myDart[i] == null)
			{
				myDart[i] = (GameObject)Instantiate(dart,transform.position,transform.rotation);
			}
		}
		bool x = true;
		for(int i = 0; i <= myDart.Length - 1; i++)
		{
			if(myDart[i].activeInHierarchy)
			{
				x = false;
			}
		}
		if(x)
		{
			timer += Time.deltaTime;
			if(timer >= waitTime)
			{
				timer = 0;
				int counter = 0;
				foreach(Transform child in transform)
				{
					myDart[counter].transform.position = child.position;
					myDart[counter].SetActive(true);
					counter += 1;
				}
			}
		}
			
	}

	public void SetClass(ThreatClass newClass)
	{
		myClass = newClass;
		myType = (ThreatType)myClass.GetIntType();
		SetDirection();
		startPos = transform.position;
		endPos = startPos + ((Vector3)dir.normalized * dist);
		live = true;

	}

	private void SetDirection()
	{
		if(myType == ThreatType.Free)
		{
			myClass.SetDirection(transform);
			dir = myClass.GetDir();
			dist = myClass.GetDist();

		}
        else if(myType == ThreatType.Radial|| myType == ThreatType.RadialDouble)
        {
            int[] nums = new int[] { -1, 1 };
            int rand = Random.Range(0, nums.Length - 1);
            rotDir = nums[rand];
            rotSpeed = Random.Range(10, 15);
        }
		else
		{
			dir = transform.right;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, 1<<9);
			if(hit.collider != null)
			{
				dist = hit.distance - transform.localScale.x;
			}
			else
			{
				if(transform.right.normalized == Vector3.up)
				{
					Debug.Log("UP");
					dist = Mathf.Abs(22.5f - transform.position.y);
				}
				else if(transform.right.normalized == Vector3.down)
				{
					Debug.Log("Down");
					dist = Mathf.Abs(-17.5f - transform.position.y);
				}
			}
		}
	}

}
