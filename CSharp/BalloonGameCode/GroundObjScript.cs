using UnityEngine;
using System.Collections;

public class GroundObjScript : MonoBehaviour 
{
	private Vector2 dir;
	private Collider2D[] hit;
	public int speed;
    private float bumpLeft;
    private float bumpRight;
    public Rigidbody2D myBody;
	private Animation anim;

	// Use this for initialization
	void Start () 
	{
		dir = -Vector2.right;
		anim = GetComponent<Animation>();
        bumpLeft = 45f - (transform.localScale.x / 2);
        bumpRight = -35 + (transform.localScale.x / 2);
	}
	
	// Update is called once per frame
	void Update () 
	{

        if (dir.x > 0)
        {
            if(transform.position.x >= bumpLeft)
            {
                dir = -dir;
                foreach (AnimationState a in anim)
                {
                    a.speed = -a.speed;
                }
            }
        }
        else if(dir.x < 0)
        {
            if(transform.position.x <= bumpRight)
            {
                dir = -dir;
                foreach (AnimationState a in anim)
                {
                    a.speed = -a.speed;
                }
            }

        }

        transform.Translate(dir * Time.deltaTime * speed,Space.World);
       // myBody.AddRelativeForce(dir * Time.deltaTime * speed, ForceMode2D.Impulse);
    }
/*
    hit = Physics2D.OverlapCircleAll(transform.position,transform.localScale.x/2.1f,1<<14);
		if(hit.Length > 0)
		{
			for(int i = 0; i <= hit.Length - 1; i++)
			{
				if(hit[i].gameObject != gameObject)
				{
					//Debug.Log(hit[i].gameObject.name);
					dir = -dir;
                    transform.Translate(dir* Time.deltaTime * speed, Space.World);
                    //myBody.AddRelativeForce(dir * Time.deltaTime * speed, ForceMode2D.Impulse);
					foreach(AnimationState a in anim)
					{
						a.speed = -a.speed;
					}
transform.position = new Vector2(transform.position.x + dir.x, transform.position.y);
                    break;
				}
			}

		}
        */
}
