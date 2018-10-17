using UnityEngine;
using System.Collections;

public class BalloonClass 
{
	// Use this for initialization
	public enum BalloonType {Standard,Leather,Sniper,Bomber,Swarmer};
	public BalloonType myType;
	private int speed;
	private int maxFill;
	private int traction;

	public void SetType(int newType)
	{
		myType = (BalloonType)newType;

		if(myType == BalloonType.Standard)
		{
			speed = 25;
			maxFill = 10;
			traction = 3;
            
		}
		else if(myType == BalloonType.Leather)
		{
			speed = 15;
			maxFill = 12;
			traction = 2;
		}
		else if(myType == BalloonType.Sniper)
		{
			speed = 30;
			maxFill = 10;
			traction = 3;
		}
		else if(myType == BalloonType.Bomber)
		{
			speed = 15;
			maxFill = 12;
			traction = 2;
		}
		else if(myType == BalloonType.Swarmer)
		{
			speed = 25;
			maxFill = 6;
			traction = 1;
		}
	}

	public int[] GetStuff()
	{
		int[] toReturn = new int[]{speed, maxFill,traction};
		return toReturn;
	}

}










