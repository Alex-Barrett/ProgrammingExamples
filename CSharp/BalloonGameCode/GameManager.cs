using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public static GameManager myManager;
	private enum Phase{Pick,Charge,Live,Stop,None};
	private enum LoadPhase{Start,Mid,End,Done};
	private LoadPhase myLoadPhase;
	private Phase myPhase;
	public GameObject mainCam;
	public GameObject GUICam;
	private GameObject currentBalloon;
	private GameObject[] balloons;
	private GameObject[] myPlacements;
	private GameObject[] myCastlePlacements;
	private GameObject[] myCasters;
	private GameObject[] terrain;
	private GameObject[] threats;
	private GameObject[] traps;
	private GameObject[] enemies;
	private GameObject[] castles;
	private GameObject[] groundStuff;
	public GameObject[] pickGUI;
	public GameObject[] chargeGUI;
    public GameObject specialGUI;
	public GameObject winMenu;
	public GameObject continueButton;
	public GameObject againButton;
	public GameObject loadScreen;
	public GameObject loadBar;
	public TextMesh scoreText;
	public TextMesh livesText;
	public GameObject explosion;	
	private List<GameObject> myEnemies;
	private List<GameObject> mySwarm;
	public Transform startPos;
	private int[] unBalloons;
	private int chapter;
	private int level;
	private int numEnemies = 0;
	private int lastNum = 0;
	private int lives;
	private int passScore = 0;
	private float score = 0;
	private bool called = false;
	private bool pickActive = false;
	private bool chargeActive = false;
	public Texture2D loadBarAngry;
	public Texture2D loadBarRage;

	// Use this for initialization


	//Game Assets
	//Animations
	//Other Balloons
	
 
	void Awake()
	{
		myManager = this;
	}
	     
	void Start () 
	{
		//OnLevelLoaded(1,1);
		myPhase = Phase.None;
		unBalloons = new int[]{0,0,0,0,0};
		ActiveCharge(false);
		WinMenu(false,0);
		myEnemies = new List<GameObject>();
		mySwarm = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!called)
		{
			//OnLevelLoaded(1,1);
			called = true;
		}
		if(myPhase == Phase.Pick && !pickActive)
		{
			ActivePick(true);
		}
		else if(myPhase == Phase.Charge && !chargeActive)
		{
			ActiveCharge(true);
		}
		else if(myPhase == Phase.Live)
		{

		}
	}

	public void OnLevelLoaded(int newChapter, int newLevel)
	{
		Debug.Log(newChapter);
		Debug.Log(newLevel);
		level = newLevel;
		chapter = newChapter;
		lives = 4 + chapter;
		scoreText.text = "Score: " + score.ToString();
		livesText.text = "Balloons: " + lives.ToString();
		myPlacements = GameObject.FindGameObjectsWithTag("Placement");
		myCasters = GameObject.FindGameObjectsWithTag("StartCaster");
		myCastlePlacements = GameObject.FindGameObjectsWithTag("CastleOptional");
		GameObject[] tempClouds = GameObject.FindGameObjectsWithTag("Cloud");
		for(int i = 0; i <= tempClouds.Length - 1; i++)
		{
			tempClouds[i].GetComponent<CloudScript>().SetGust(chapter);
		}
		for(int i = 0; i <= unBalloons.Length - 1; i++)
		{
			if(chapter > i)
			{
				unBalloons[i] = 1;
			}
		}
		StartCoroutine(LoadScreen());
		StartCoroutine(LevelSetup());
	}

	public void ChargeUp()
	{
		if(currentBalloon != null && myPhase == Phase.Charge)
		{
			//currentBalloon.GetComponent<BalloonScript>().ChargeUp();
			mainCam.GetComponent<LineDrawScript>().ResetLines();
		}
	}
	public void Launch()
	{
		if(currentBalloon != null && myPhase == Phase.Charge)
		{
			currentBalloon.GetComponent<BalloonScript>().Launch();
			ActiveCharge(false);
			myPhase = Phase.Live;
			mainCam.GetComponent<CameraScript>().SetLive(true,currentBalloon.transform);
			lives --;
			livesText.text = "Balloons: " + lives;
            if(currentBalloon.GetComponent<BalloonScript>().GetBallType() > 1)
            {
                specialGUI.SetActive(true);
            }
           
		}
	}

	public void RePick()
	{
		if(currentBalloon != null && myPhase == Phase.Charge)
		{
			mainCam.GetComponent<CameraScript>().SetLive(false,null);
			myPhase = Phase.Pick;
			ActiveCharge(false);
			Destroy(currentBalloon);
			currentBalloon = null;
		}
	}

	public void ReTry()
	{
		LevelManager.levelManager.LoadCustom(chapter,level);
	}

    public void TrySpecial()
    {
        currentBalloon.GetComponent<BalloonScript>().GoSpecial();
        specialGUI.SetActive(false);
    }

	private void ActivePick(bool x)
	{
		Debug.Log("PICKCALLED");
		for(int i = 0; i <= pickGUI.Length - 1; i++)
		{
			pickGUI[i].SetActive(x);
		}
        for (int i = chapter; i <= pickGUI.Length - 2; i++)
        {
            pickGUI[i].GetComponent<Button>().interactable = false;
        }
		pickActive = x;

	}
	private void ActiveCharge(bool x)
	{
		chargeActive = x;
		for(int i = 0; i <= chargeGUI.Length - 1; i++)
		{
			chargeGUI[i].SetActive(x);
		}
		mainCam.GetComponent<LineDrawScript>().SetReady(x);
	}
	
	public void NewPick(int type)
	{
		if(unBalloons[type] == 1)
		{
			SpawnBalloon(type);
            mainCam.GetComponent<LineDrawScript>().ResetLines();
		}
	}

	private void WinMenu(bool x, int y)
	{
		winMenu.SetActive(x);
		if(y == 0)
		{
			continueButton.SetActive(true);
			againButton.SetActive(false);
		}
		if(y == 1)
		{
			continueButton.SetActive(false);
			againButton.SetActive(true);
		}
	}

	private void SpawnBalloon(int type)
	{
		GameObject newBalloon = null;
		BalloonClass newClass = new BalloonClass();
		newClass.SetType(type);
		for(int i = 0; i <= balloons.Length - 1; i++)
		{
			switch(type)
			{
			case 0:
				if(balloons[i].name.Equals("BalloonStandard"))
				{
					newBalloon = balloons[i];
					break;
				}
				break;
			case 1:
				if(balloons[i].name.Equals("BalloonLeather"))
				{
					newBalloon = balloons[i];
					break;
				}
				break;
			case 2:
				if(balloons[i].name.Equals("BalloonSniper"))
				{
					newBalloon = balloons[i];
					break;
				}
				break;
			case 3:
				if(balloons[i].name.Equals("BalloonBomber"))
				{
					newBalloon = balloons[i];
					break;
				}
				break;
			case 4:
				if(balloons[i].name.Equals("BalloonSwarmer"))
				{
					newBalloon = balloons[i];
					break;
				}
				break;
			}
		}
		currentBalloon = (GameObject)Instantiate(newBalloon,startPos.position,Quaternion.identity);
		currentBalloon.GetComponent<BalloonScript>().SetClass(newClass);
		currentBalloon.GetComponent<PolygonCollider2D>().enabled = false;
		myPhase = Phase.Charge;
		ActivePick(false);
		if(type == 4)
		{
			mySwarm.Add(currentBalloon);
		}

	}

	public void Swarm(GameObject pos, int num, BalloonClass newClass, float liveTime)//UGLY
	{
		mySwarm.Remove(pos);
		if(num <= 0)
		{
			if(pos == currentBalloon)
			{
				Debug.Log("True");

				if(mySwarm.Count > 0)
				{
					currentBalloon = mySwarm[0];
					mainCam.GetComponent<CameraScript>().SetLive(true,currentBalloon.transform);
				}
				else
				{
					CallReset();
				}
			}
			return;
		}
		for(int i = 0; i <= 2; i++)
		{
			GameObject newBalloon = (GameObject)Instantiate(balloons[4],pos.transform.position,Quaternion.identity);
            BalloonScript tempBalloon = newBalloon.GetComponent<BalloonScript>();
            BalloonScript posBalloon = pos.GetComponent<BalloonScript>();
			mySwarm.Add(newBalloon);
			tempBalloon.SetClass(newClass);
			//newBalloon.transform.localScale = pos.transform.localScale;
			tempBalloon.SetSwarm(num,liveTime);
            tempBalloon.SetLines(posBalloon.GetLines());
            tempBalloon.SetNewPoint(posBalloon.GetCurrentPoint());
			newBalloon.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(Random.Range(-5f,5f),Random.Range(-5f,5f)),ForceMode2D.Impulse);
			if(pos == currentBalloon && i == 0)
			{
				currentBalloon = newBalloon;
				mainCam.GetComponent<CameraScript>().SetLive(true,currentBalloon.transform);
			}
		}

	}

	public void Explosion(Vector2 pos)
	{
		explosion.transform.position = new Vector3(pos.x,pos.y,explosion.transform.position.z);
		explosion.GetComponent<ParticleSystem>().Play();
	}

	public void CallReset()
	{
		StartCoroutine(Reset());
	}

	IEnumerator Reset()
	{
        //Score();
        //specialGUI.SetActive(false);
		if(CheckWin())
		{
			myPhase = Phase.Stop;
			yield break;
		}
		currentBalloon = null;
		yield return new WaitForSeconds(2);//
		CamReset();
		myPhase = Phase.Pick;


	}

	private void CamReset()
	{
		mainCam.GetComponent<CameraScript>().SetLive(false,null);
		mainCam.transform.position = new Vector3 (0,0,10);
	}

	public void Score()//what if I hit 2? Call seperate from Reset?
	{

			float num = currentBalloon.GetComponent<BalloonScript>().GetLiveTime();
			score += Mathf.CeilToInt((1000 * chapter * level) / num);
			scoreText.text = "Score: " + score.ToString();
			lastNum --;
	}

	private bool CheckWin()
	{
		bool stop = false;
		if(myEnemies.Count == 0 || lives==0)
		{
			if(score >= passScore)
			{
				stop = true;
				CamReset();
				WinMenu(true, 0);
				LevelManager.levelManager.Passed(chapter,level,(int)score);
			}
			else
			{
				stop = true;
				CamReset();
				WinMenu(true, 1);
			}
		}


		return stop;
	}

	public int GetChapter()
	{
		return chapter;
	}

	public void DestroyEnemy(GameObject x)
	{
		myEnemies.Remove(x);
	}

	IEnumerator LoadScreen()
	{
		Renderer ballRend = null;
		foreach(Transform child in loadBar.transform)
		{
			ballRend = child.GetComponent<Renderer>();
		}

		while(myLoadPhase != LoadPhase.Done)
		{
			//loadScreen.SetActive(true);
			//GUICam.SetActive(false);
			if(myLoadPhase == LoadPhase.Start)
			{
				loadBar.transform.localScale = Vector3.Lerp(loadBar.transform.localScale,new Vector3(30f,30f,1f),Time.deltaTime);
			}
			else if(myLoadPhase == LoadPhase.Mid)
			{

				loadBar.transform.localScale = Vector3.Lerp(loadBar.transform.localScale,new Vector3(45f,45f,1f),Time.deltaTime);
				if(ballRend.material.mainTexture != loadBarAngry)
				{
					ballRend.material.mainTexture = loadBarAngry;
				}
			}
			else if(myLoadPhase == LoadPhase.End)
			{
			
				loadBar.transform.localScale = Vector3.Lerp(loadBar.transform.localScale,new Vector3(60f,60f,1f),Time.deltaTime);
				if(ballRend.material.mainTexture != loadBarRage)
				{
					ballRend.material.mainTexture = loadBarRage;
				}
			}
			yield return null;
		}
		while(loadBar.transform.localScale.x < 60)
		{
			loadBar.transform.localScale = Vector3.Lerp(loadBar.transform.localScale,new Vector3(61f,61f,1f),Time.deltaTime);
			yield return null;
		}
		yield return new WaitForSeconds(2);
		myPhase = Phase.Pick;
		loadScreen.SetActive(false);
		loadBar.SetActive(false);
		GUICam.SetActive(true);
	}

	IEnumerator LevelSetup()
	{
		//Load Stuff
		myLoadPhase = LoadPhase.Start;
		Debug.Log("called");
		Object[] temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/Terrain");
		terrain = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			terrain[i] = (GameObject)temp[i];
		}

		temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/Traps");
		traps = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			traps[i] = (GameObject)temp[i];
		}

		temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/Threats");
		threats = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			threats[i] = (GameObject)temp[i];
		}

		temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/Enemies");
		enemies = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			enemies[i] = (GameObject)temp[i];
		}
		temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/Castle");
		castles = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			castles[i] = (GameObject)temp[i];
		}
		temp = Resources.LoadAll("Chapter" + chapter.ToString() + "/GroundObj");
		groundStuff = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			groundStuff[i] = (GameObject)temp[i];
		}
		temp = Resources.LoadAll("Balloons");
		balloons = new GameObject[temp.Length];
		for(int i = 0; i <= temp.Length - 1; i++)
		{
			balloons[i] = (GameObject)temp[i];
		}

		yield return null;
		int numTerrain = 5 + (chapter * level);
		int maxTraps = 5 + (chapter * level);
		int maxThreats = Mathf.FloorToInt((chapter + level)/2);
		int maxCastle = 5 + chapter + level;
		int maxEnemies = 3 + chapter;
		passScore = maxEnemies * 100 * chapter * level;
		List<int> placementIndex = new List<int>();
		List<GameObject> newTerrain = new List<GameObject>();
		List<GameObject> threatPlacement = new List<GameObject>();
		bool setup = false;
	//
		while(!setup)
		{
			placementIndex = new List<int>();
			newTerrain = new List<GameObject>();
			for(int i = 0; i <= myPlacements.Length - 1; i++)
			{
				placementIndex.Add(i);//Add numbers
				myPlacements[i].tag = ("Placement");
			}
			for(int i = 0; i <= numTerrain - 1; i++)//randomly pick placements
			{
				int rand = Random.Range(0, placementIndex.Count);
				newTerrain.Add(myPlacements[placementIndex[rand]]);

				placementIndex.RemoveAt(rand);
			}
			for(int i = 0; i <= placementIndex.Count - 1; i++)
			{
				myPlacements[placementIndex[i]].tag = "Path";
			}
			for(int i = 0; i <= myCasters.Length - 1; i++)//Check if path is available
			{
				if(myCasters[i].GetComponent<PathFinderScript>().FindPath())
				{
					setup = true;
					break;
				}
			}
			Debug.Log("calling");
			yield return null;
		}

		for(int i = 0; i <= placementIndex.Count - 1; i++)// check where threats can go
		{
			foreach(Transform child in myPlacements[placementIndex[i]].transform)
			{
				if(child.CompareTag("Caster"))
				{
					if(child.GetComponent<PathFinderScript>().ThreatValid())//Check if there is room for threat
					{
						myPlacements[placementIndex[i]].tag = ("Placement");
						for(int x = 0; x <= myCasters.Length - 1; x++)//check if path is still available
						{

							if(myCasters[x].GetComponent<PathFinderScript>().FindPath())
							{
								threatPlacement.Add(myPlacements[placementIndex[i]]);
								break;
							}


						}
						myPlacements[placementIndex[i]].tag = ("Path");
					}
				}
			}
			yield return null;
		}
		myLoadPhase = LoadPhase.Mid;
		for(int i = 0; i <= myPlacements.Length - 1; i++)//turn off "path" placements
		{
			if(myPlacements[i].activeInHierarchy)
			{
				if(myPlacements[i].CompareTag("Path"))
				{
					myPlacements[i].SetActive(false);
				}
			}
		}

		if(threatPlacement.Count > 0) //place threats
		{
			for(int i = 0; i <= maxThreats; i++)
			{
				int num = Random.Range(0,threatPlacement.Count);
				SetThreat(threatPlacement[num]);//Set threat
				threatPlacement.RemoveAt(num);
				yield return null;
			}
		}


		for(int i = 0; i <= newTerrain.Count - 1; i++)//place terrain
		{
			newTerrain[i].SetActive(true);
			SetTerrain(newTerrain[i]);

		}

		for(int i = 0; i <= newTerrain.Count - 1; i++)//check available sides for traps
		{
			foreach(Transform child in newTerrain[i].transform)
			{
				if(child.CompareTag("Caster"))
				{
					child.GetComponent<PathFinderScript>().CheckSides();
				}
			}
		}

		placementIndex = new List<int>();
		for(int i = 0; i <= newTerrain.Count - 1; i++)//reset placementindex
		{
			placementIndex.Add(i);
		}

		for(int i = 0; i <= maxTraps; i++)//Set traps
		{
			//Debug.Log(i);
			bool y = false;
			int num = Random.Range(0,placementIndex.Count);
			int[] tempSides = new int[0];
			Transform newChild = null;
			foreach(Transform child in newTerrain[placementIndex[num]].transform)
			{
				tempSides =  child.GetComponent<PathFinderScript>().GetSides();
				newChild = child;
			}
			for(int x = 0; x <= tempSides.Length - 1; x++)
			{
				if(tempSides[x] == 1)
				{
					Debug.Log("Here");
					SetTrap(newTerrain[placementIndex[num]]);//consider sending side too
					newChild.GetComponent<PathFinderScript>().BlockSide(x);
					y = true;
					break;
				}
			}
			if(!y)
			{
				placementIndex.RemoveAt(num);
				i--;
			}
			if(placementIndex.Count == 0)
			{
				break;
			}
			yield return null;
		}
		myLoadPhase = LoadPhase.End;
		yield return null;
		for(int i = 0; i <= myPlacements.Length - 1; i++)
		{
			myPlacements[i].SetActive(false);
		}

		setup = false;
		while(!setup)
		{
			placementIndex = new List<int>();
			List<GameObject> newCastle = new List<GameObject>();
			for(int i = 0; i <= myCastlePlacements.Length - 1; i++)
			{
				placementIndex.Add(i);//Add numbers
				myCastlePlacements[i].tag = ("CastleOptional");
			}
			List<GameObject> enemyPlacement = new List<GameObject>();
			for(int i = 0; i <= maxCastle; i++)//turn castle pieces on
			{
				int rand = Random.Range(0,placementIndex.Count);
				newCastle.Add(myCastlePlacements[placementIndex[rand]]);
				placementIndex.RemoveAt(rand);
			}
			for(int i = 0; i <= placementIndex.Count - 1; i++)//change tag
			{
				myCastlePlacements[placementIndex[i]].tag = ("Path");
			}
			for(int i = 0; i <= placementIndex.Count - 1; i++)//find viable placements
			{
				GameObject[] tempArr = new GameObject[0];
				if(myCastlePlacements[placementIndex[i]].GetComponent<PathFinderScript>().CastleValid(tempArr))
				{
					if(myCastlePlacements[placementIndex[i]].GetComponent<PathFinderScript>().CastleSides())
					{
						enemyPlacement.Add(myCastlePlacements[placementIndex[i]]);
					}
				}
			}
			Debug.Log(enemyPlacement.Count);
			if(enemyPlacement.Count >= maxEnemies)//Place enemies
			{
				for(int i = 0; i <= maxEnemies - 1; i++)
				{
					int rand  = Random.Range(0,enemyPlacement.Count);
					Debug.Log(rand);
					Debug.Log(enemyPlacement.Count);
					SetEnemy(enemyPlacement[rand]);
					enemyPlacement.RemoveAt(rand);
				}
				for(int i = 0; i <= placementIndex.Count - 1; i++)
				{
					myCastlePlacements[placementIndex[i]].SetActive(false);
				}
				for(int i = 0; i <= newCastle.Count - 1; i++)
				{
					SetCastle(newCastle[i].gameObject);
					newCastle[i].SetActive(false);
				}
				GameObject[] staticCastle = GameObject.FindGameObjectsWithTag("CastleStatic");
				for(int i = 0; i <= staticCastle.Length - 1; i++)
				{
					SetCastle(staticCastle[i]);
					staticCastle[i].SetActive(false);
				}
				setup = true;
			}
			yield return null;

		}
		GameObject[] tempGround = GameObject.FindGameObjectsWithTag("Ground");//Setup ground
		for(int i = 0; i <= tempGround.Length - 1; i++)
		{
			SetGround(tempGround[i]);
			tempGround[i].SetActive(false);
		}
		int rando = Random.Range(1,4);
		placementIndex = new List<int>();
		for(int i = 0; i <= tempGround.Length - 1; i++)//Set GroundObstacles
		{
			placementIndex.Add(i);
		}
		for(int i = 0; i <= rando; i++)//
		{
			int rand = Random.Range(0,placementIndex.Count);
			SetGroundObj(tempGround[placementIndex[rand]]);
			placementIndex.RemoveAt(rand);
			
		}
		tempGround = GameObject.FindGameObjectsWithTag("GroundStatic");//setup platform
		for(int i = 0; i <= tempGround.Length - 1; i++)
		{
			SetStaticGround(tempGround[i]);
			tempGround[i].SetActive(false);
		}
		myLoadPhase = LoadPhase.Done;

	}

	private void SetTrap(GameObject location)
	{
		int[] num = new int[0];
		foreach(Transform child in location.transform)
		{
			if(child.CompareTag("Caster"))
			{
				num = child.GetComponent<PathFinderScript>().GetSides();
			}
		}
		int rand = Random.Range(0,traps.Length);
		GameObject newTrap = traps[rand];
		GameObject tempTrap;
		for(int i = 0; i <= num.Length - 1; i++)
		{
			if(num[i] == 1)
			{
				switch(i)
				{
				case 0:
					tempTrap = (GameObject)Instantiate(newTrap,location.transform.position,Quaternion.identity);
					tempTrap.transform.Translate(3.75f,0,0);
					break;
				case 1:
					tempTrap = (GameObject)Instantiate(newTrap,location.transform.position,Quaternion.identity);
					tempTrap.transform.Translate(0,3.75f,0);
					tempTrap.transform.Rotate(0,0,90f);
					break;
				case 2:
					tempTrap = (GameObject)Instantiate(newTrap,location.transform.position,Quaternion.identity);
					tempTrap.transform.Translate(-3.75f,0,0);
					tempTrap.transform.Rotate(0,0,180f);
					break;
				case 3:
					tempTrap = (GameObject)Instantiate(newTrap,location.transform.position,Quaternion.identity);
					tempTrap.transform.Translate(0,-3.75f,0);
					tempTrap.transform.Rotate(0,0,270f);
					break;
				}
				break;
			}

		}
	}

	private void SetThreat(GameObject location)
	{
		ThreatClass newThreat = new ThreatClass();
		Transform tempTrans = newThreat.SetThreat(location.transform);
		if(tempTrans == null)
		{
			return;
		}
		List<int> pick = new List<int>();
		for(int i = 0; i <= threats.Length - 1; i++)
		{
			if(threats[i].CompareTag(newThreat.GetMyType()))
			{
				pick.Add(i);
			}
		}
		int rand = Random.Range(0, pick.Count);
		GameObject obj = (GameObject)Instantiate(threats[pick[rand]],tempTrans.position,tempTrans.rotation);
		obj.GetComponent<ThreatScript>().SetClass(newThreat);
	}

	private void SetTerrain(GameObject location)
	{
		int rand = Random.Range(0,terrain.Length);
		GameObject newTerrain = terrain[rand];
		GameObject tempTerrain = (GameObject)Instantiate(newTerrain, location.transform.position, Quaternion.identity);
	}

	private void SetEnemy(GameObject location)
	{
		int rand = Random.Range(0,enemies.Length);
		GameObject newEnemy = enemies[rand];
		GameObject tempEnemy = (GameObject)Instantiate(newEnemy,location.transform.position,Quaternion.identity);
		myEnemies.Add(tempEnemy);
		numEnemies ++;
		lastNum ++;
	}

	private void SetCastle(GameObject location)
	{
		int rand = Random.Range(0,castles.Length);
		GameObject newTerrain = castles[rand];
		GameObject tempTerrain = (GameObject)Instantiate(newTerrain, location.transform.position, Quaternion.identity);
	}

	private void SetGround(GameObject location)
	{
		int rand = Random.Range(0,terrain.Length);
		GameObject newTerrain = terrain[rand];
		GameObject tempTerrain = (GameObject)Instantiate(newTerrain, location.transform.position, Quaternion.identity);
		tempTerrain.transform.localScale = location.transform.localScale;

	}

	private void SetStaticGround(GameObject location)
	{
		int rand = Random.Range(0,castles.Length);
		GameObject newTerrain = terrain[1];
		GameObject tempTerrain = (GameObject)Instantiate(newTerrain, location.transform.position, Quaternion.identity);
		tempTerrain.transform.localScale = location.transform.localScale;
	}
	private void SetGroundObj(GameObject location)
	{
		int rand = Random.Range(0,groundStuff.Length);
		GameObject newTerrain = groundStuff[rand];
		GameObject tempTerrain = (GameObject)Instantiate(newTerrain, location.transform.position, Quaternion.identity);
		tempTerrain.transform.Translate(0,5f,0);

	}
}

















