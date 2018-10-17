using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalloonScript : MonoBehaviour 
{
	BalloonClass myClass;
	AnimateTiledTexture myAnimator;
	public int speed;
	private int maxFill = 1;
	private int swarmNum = 3;
	private int currentPoint = 0;
	private int traction = 1;
	private float fill = 1f;
	private float liveTime = 0;
	private float snipeTime = 0;
	private float minDist = 5f;
	private Rigidbody2D myRigidbody;
	private HealthBarScript myHealthBar;
	private bool live = false;
	private bool sniping = false;
	private Vector2 oldScale;
	private List<Vector3> pathPoints;
	public Material normMat;
	public Material altMat;
	public Material popMat;
	public GameObject myGust;
    public ParticleSystem myParticles;
	public GameObject myFlat;
	public GameObject myHelm;
	private GameObject target = null;


	// Use this for initialization


	void Awake () 
	{
		myRigidbody = this.GetComponent<Rigidbody2D>();
		oldScale = transform.localScale;
		myHealthBar = GameObject.Find("HealthBar").GetComponent<HealthBarScript>();
		myAnimator = GetComponent<AnimateTiledTexture>();
		pathPoints = new List<Vector3>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.localScale = new Vector3(oldScale.x * Mathf.Clamp(fill / maxFill*1.5f ,1,1.5f), oldScale.y * Mathf.Clamp(fill / maxFill * 1.5f ,1f,1.5f),1f);
		myHealthBar.Gas(fill,maxFill);
		if(!live && fill > (float)maxFill/2)
		{
			//fill -= 0.01f;
		}
		if(live)
		{
			liveTime += Time.deltaTime;
			fill -= Time.deltaTime;

			if(fill <= 0f)
			{
				live = false;

				if(myClass.myType != BalloonClass.BalloonType.Swarmer)
				{
					GameObject temp = (GameObject)Instantiate(myFlat,transform.position,transform.rotation);
                    GameObject tempHelm = (GameObject)Instantiate(myHelm, transform.position, transform.rotation);
					temp.GetComponent<DeadBalloonScript>().PassVelocity(myRigidbody.velocity);
                    tempHelm.GetComponent<HelmScript>().PassVelocity(myRigidbody.velocity);
                    Camera.main.GetComponent<CameraScript>().SetLive(true, temp.transform);
				}
				else
				{
					GameObject temp = (GameObject)Instantiate(myFlat,transform.position,transform.rotation);
					temp.GetComponent<DeadBalloonScript>().PassVelocity(myRigidbody.velocity);
					GetComponent<Collider2D>().enabled = false;
					swarmNum --;
					GameManager.myManager.Swarm(gameObject, swarmNum,myClass,liveTime);
				}
				Destroy(gameObject);
                //Debug.Log("DUMBASS");
}
		}
	}
	void FixedUpdate()
	{
		if(sniping)
		{
			snipeTime += Time.deltaTime;
			myRigidbody.AddRelativeForce(new Vector2(0,speed),ForceMode2D.Force);
			myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity,speed*3);
			Collider2D hit = Physics2D.OverlapCircle(transform.position,transform.localScale.y/2,1<<13);
			if(hit != null)
			{
				hit.GetComponent<EnemyScript>().CallDestroy();
				Destroy(hit.gameObject);
				StartCoroutine(DestroyMe());
				sniping = false;
			}


			if(target != null)
			{
				Vector3 diff = target.transform.position - transform.position;
				diff.Normalize();
				
				float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
				//transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
				transform.rotation = Quaternion.AngleAxis(rot_z - 90, Vector3.forward);
			}

			if(snipeTime >= 1 && hit == null)
			{
				GameObject temp = (GameObject)Instantiate(myFlat,transform.position,transform.rotation);
				temp.GetComponent<DeadBalloonScript>().PassVelocity(myRigidbody.velocity);
				Camera.main.GetComponent<CameraScript>().SetLive(true, temp.transform);
                GameManager.myManager.CallReset();
				Destroy(gameObject);
                sniping = false;
                // Debug.Log("DUMBASS");
                //StartCoroutine(DestroyMe());

            }
        }
		else if(live)
		{
			Movement();
		}

	}

	private void Movement()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D hit = Physics2D.OverlapPoint(mousePos, 1<<12);

			if(hit != null)
			{
				Debug.Log("Yessir!");
				Special();
				return;
			}
		}
		else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Collider2D hit = Physics2D.OverlapPoint(mousePos, 1<<12);
			if(hit != null)
			{
				Special();
				return;
			}
		}

		if(pathPoints.Count > 0 && currentPoint <= pathPoints.Count-1)//USE FORCE TO INFLUENCE MOVEMENT
		{
			if(currentPoint <= pathPoints.Count-2)
			{
				Vector2 myDir = myRigidbody.velocity.normalized;
				Vector2 checkDir = (pathPoints[currentPoint+1]- pathPoints[currentPoint]).normalized;
               // Debug.Log(myDir);
               // Debug.Log(checkDir);
               // Debug.Log(Vector2.Dot(myDir, checkDir - myDir));
                if (Vector2.Dot(myDir,checkDir-myDir) < -1 && currentPoint != 0)//Consider Raycast too
				{
					
                    Debug.Log("TITTIES!!!");
					//Debug.Log(myRigidbody.velocity.normalized);
					int newPoint = currentPoint;
					for(int i = currentPoint;i <= pathPoints.Count - 1; i++)//Note this weird shit
					{
						if(i < pathPoints.Count - 2)//Watch this
						{
							checkDir= (pathPoints[i + 1] -pathPoints[i]).normalized;//FIX THIS
							if(Vector2.Dot(myDir,checkDir-myDir) > -1)
							{
								checkDir = pathPoints[i];
								if((checkDir-myDir).sqrMagnitude <= myRigidbody.velocity.sqrMagnitude)
								{
                                    newPoint = i;
                                    break;
								}
								else
								{
									break;
								}
							}
						}
					}
					currentPoint = newPoint;
				}
			}


			Vector2 lookPos = pathPoints[currentPoint];
			lookPos = lookPos - myRigidbody.position;
           // Debug.Log(lookPos.sqrMagnitude);

			
			if(currentPoint == 0)
			{
				if(lookPos.sqrMagnitude < minDist*minDist)
				{
					currentPoint += 1;
				}
			}
			else if(lookPos.sqrMagnitude < minDist*minDist)
			{
				for(int i = currentPoint; i <= pathPoints.Count-1; i++ )
                {
                    lookPos = pathPoints[i];
                    lookPos = lookPos - myRigidbody.position;
                    if(lookPos.sqrMagnitude > minDist*minDist)
                    {
                        currentPoint = i;
                        break;

                    }
                    
                }
                if (currentPoint >= pathPoints.Count-1)
                {
                    currentPoint += 1;
                }
			}
			else if(Mathf.Atan2(lookPos.x,lookPos.y)*Mathf.Rad2Deg > 30f)
			{
				//currentPoint +=1;
                    
			}

			else
			{
				//myRigidbody.MovePosition(myRigidbody.position + new Vector2(lookPos.x,lookPos.y).normalized*speed*Time.fixedDeltaTime);
			}

			
			//myRigidbody.MovePosition(myRigidbody.position + new Vector2(lookPos.x,lookPos.y).normalized*speed*Time.fixedDeltaTime);
			//myRigidbody.AddRelativeForce((lookPos.normalized)*speed,ForceMode2D.Force);
			if(currentPoint <= pathPoints.Count-1)
			{
				lookPos = pathPoints[currentPoint];
				lookPos = lookPos- myRigidbody.position;
			}

			float angle = Mathf.Atan2(lookPos.x,lookPos.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.AngleAxis(-angle,Vector3.forward),Time.deltaTime * speed);

			//Debug.Log(Vector2.Distance(transform.position,lookPos));
            //Debug.Log(currentPoint);
            //Debug.Log(Mathf.Atan2(lookPos.x, lookPos.y) * Mathf.Rad2Deg);
           // Debug.Log(pathPoints.Count);
        }
		/*
		if(Input.GetMouseButton(0))
		{
			Vector3 mousePos = new Vector3(Input.mousePosition.x,Input.mousePosition.y, 10);
			Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
			lookPos = lookPos - transform.position;
			float angle = Mathf.Atan2(lookPos.x,lookPos.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.AngleAxis(-angle,Vector3.forward),Time.deltaTime * speed);

		}
		else if(Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Ended)
		{
			Vector3 mousePos = new Vector3(Input.GetTouch(0).position.x,Input.GetTouch(0).position.y, 10);
			Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
			lookPos = lookPos - transform.position;
			float angle = Mathf.Atan2(lookPos.x,lookPos.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.AngleAxis(-angle,Vector3.forward),Time.deltaTime * speed);

		}
		*/

		myRigidbody.AddRelativeForce(new Vector2(0,traction),ForceMode2D.Impulse);
		myRigidbody.AddRelativeForce(new Vector2(0,speed),ForceMode2D.Force);
		myRigidbody.velocity = Vector2.ClampMagnitude(myRigidbody.velocity,speed);
		/*if(myRigidbody.velocity.sqrMagnitude > speed)
		{
			float brake = myRigidbody.velocity.sqrMagnitude - speed;
			myRigidbody.AddForce(myRigidbody.velocity.normalized * -brake, ForceMode2D.Force);
		}*/

	}



	private void Special() 
	{
		if(myClass.myType == BalloonClass.BalloonType.Sniper)//USE HOMING
		{
            SpecialSniper();

		}
		else if(myClass.myType == BalloonClass.BalloonType.Bomber)
		{
            SpecialBomber();
		}
		else if(myClass.myType == BalloonClass.BalloonType.Swarmer)
		{
            SpecialSwammer();
		}
		else
		{
			StartCoroutine(DestroyMe());
		}
	}

    private void SpecialSniper()
    {
        //myRigidbody.velocity = new Vector2(0,0);

        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        target = null;
        float num = Mathf.Infinity;
        for (int i = 0; i <= temp.Length - 1; i++)
        {
            float dist = Vector3.Distance(transform.position, temp[i].transform.position);
            if (dist < num)
            {
                num = dist;
                target = temp[i];
            }
        }
        Debug.Log(target);
        if (target != null)
        {
            Vector3 diff = target.transform.position - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            transform.rotation = Quaternion.AngleAxis(rot_z - 90, Vector3.forward);
        }
        myRigidbody.AddRelativeForce(new Vector2(0, speed), ForceMode2D.Impulse);
        GetComponent<Collider2D>().enabled = false;
        sniping = true;
    }

    private void SpecialBomber()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 10f);//Fix explosion radius
        this.gameObject.layer = 2;
        for (int i = 0; i <= hit.Length - 1; i++)
        {
            RaycastHit2D newHit = Physics2D.Raycast(transform.position, hit[i].transform.position - transform.position);
            if (newHit.collider != null)
            {
                if (newHit.collider.CompareTag("Enemy"))
                {
                    newHit.collider.GetComponent<EnemyScript>().CallDestroy();
                    Destroy(newHit.collider.gameObject);

                }
            }
        }
        GameManager.myManager.Explosion(transform.position);
        StartCoroutine(DestroyMe());
    }

    private void SpecialSwammer()
    {
        GetComponent<Collider2D>().enabled = false;
        swarmNum--;
        GameManager.myManager.Swarm(gameObject, swarmNum, myClass, liveTime);
        StartCoroutine(DestroyMe());
    }

	public void ChargeUp()
	{
		if(!live)
		{
			if(fill < maxFill)
			{
				fill += 0.1f;
			}
			else if(fill > maxFill)
			{
				fill = maxFill;
			}
		}
	}

	public void Launch()
	{
		if(!live)
		{
			live = true;
			pathPoints = GameManager.myManager.mainCam.GetComponent<LineDrawScript>().GetLines();
			GetComponent<PolygonCollider2D>().enabled = true;
			myAnimator.enabled = true;
			GetComponent<Renderer>().material = altMat;
			//myGust.SetActive(true);
            myParticles.Play();
		}
	}

	public void SetClass(BalloonClass newClass)
	{
		myClass = newClass;
		int[] getStuff = myClass.GetStuff();
		speed = getStuff[0];
		maxFill = getStuff[1];
		traction = getStuff[2];
		fill = (float)maxFill;
	}

	public void SetSwarm(int num, float newTime)
	{
		swarmNum = num;
		GetComponent<PolygonCollider2D>().enabled = false;
		live = true;
		liveTime = newTime;
		myAnimator.enabled = true;
		GetComponent<Renderer>().material = altMat;
		//myGust.SetActive(true);
        myParticles.Play();
		oldScale = transform.localScale;
        //fill -= liveTime;
		Invoke("ColliderSwitch",0.5f);
	}

	private void ColliderSwitch()
	{
		GetComponent<PolygonCollider2D>().enabled = true;
	}

	private void OnCollisionEnter2D(Collision2D c)
	{
		if(c.collider.CompareTag("Trap"))
		{
			if(myClass.myType != BalloonClass.BalloonType.Leather)
			{
				Special();
				//StartCoroutine(DestroyMe());
			}
		}
		else if(c.collider.CompareTag("Cloud"))
		{
			Special();
			//StartCoroutine(DestroyMe());
		}
		else if(c.collider.CompareTag("Free") || c.collider.CompareTag("Verticle") || c.collider.CompareTag("Horizontal")|| c.collider.CompareTag("Radial")|| c.collider.CompareTag("RadialDouble"))
		{
			Special();
			//StartCoroutine(DestroyMe());
		}
		else if(c.collider.CompareTag("Enemy"))
		{

			GameManager.myManager.DestroyEnemy(c.gameObject);
			c.collider.GetComponent<EnemyScript>().CallDestroy();
			Destroy(c.gameObject);
			Special();
			//StartCoroutine(DestroyMe());
		}
	}

	private IEnumerator DestroyMe() //Coroutine
	{
		Debug.Log("DESTROYED");
		GetComponent<Renderer>().material = popMat;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		GameObject temp = (GameObject)Instantiate(myHelm,transform.position,transform.rotation);
		temp.GetComponent<HelmScript>().PassVelocity(myRigidbody.velocity);
		if(myClass.myType != BalloonClass.BalloonType.Swarmer)
		{
			GameManager.myManager.CallReset();//What if swarmer?
		}
		else
		{
			//
		}

		Destroy(gameObject);
	}

	public float GetLiveTime()
	{
		return liveTime;
	}

    public List<Vector3> GetLines()
    {
        return pathPoints;
    }

    public void SetLines(List<Vector3> newPath)
    {
        pathPoints = newPath;
    }

    public int GetCurrentPoint()
    {
        return currentPoint;
    }

    public void SetNewPoint(int newPoint)
    {
        currentPoint = newPoint;
    }

    public void GoSpecial()
    {
        Special();
    }

    public int GetBallType()
    {
        return (int)myClass.myType;
    }
}










