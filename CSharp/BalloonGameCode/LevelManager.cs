using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager levelManager;

	private int[] myLevels = new int[40];
	private int[] myScores = new int[40];
	private int nextChapter;
	private int nextLevel;


	// Use this for initialization
	void Awake () 
	{
		if(levelManager == null)
		{
			DontDestroyOnLoad(gameObject);
			levelManager = this;
			Load();
		}
		else if(levelManager != this)
		{
			Destroy(gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void LoadNext()
	{
		StartCoroutine(LoadGame(nextChapter,nextLevel));
	}

	public void LoadCustom(int newChapter, int newLevel)
	{
		//Ads
		if(newChapter < nextChapter)
		{
			StartCoroutine(LoadGame(newChapter,newLevel));
		}
		else if(newChapter == nextChapter && newLevel <=nextLevel)
		{
			StartCoroutine(LoadGame(newChapter,newLevel));
		}
	}

	IEnumerator LoadGame(int c, int l)
	{
		Application.LoadLevel("NewLevel");
		yield return null;
		yield return null;
		GameManager.myManager.OnLevelLoaded(c,l);
		//GameManager.myManager.OnLevelLoaded(3,1);
	}

	public int[] GetLevels()
	{
		int[] temp = new int[myLevels.Length];
		Array.Copy(myLevels,temp,myLevels.Length);
		return temp;
	}

	public int[] GetScores()
	{
		int[] temp = new int[myScores.Length];
		Array.Copy(myScores,temp, myScores.Length);
		return temp;
	}

	public int GetNextLevel()
	{
		return nextLevel;
	}

	public int GetNextChapter()
	{
		return nextChapter;
	}

	public void Passed(int passChapter,int passLevel,int passScore)
	{
		int num = ((passChapter - 1) * 8) + (passLevel - 1);
		myLevels[num] = 1;
		myScores[num] = passScore;
		for(int i = 0; i <= myLevels.Length - 1; i++)
		{
			if(myLevels[i] == 0)
			{
				int tempLevel = i + 1;
				int counter = 1;
				while(tempLevel > 8)
				{
					tempLevel -= 8;
					counter += 1;
				}
				nextLevel = tempLevel;
				nextChapter = counter;
				break;
			}
		}
		Save();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");
		PlayerData data = new PlayerData();
		//stuff
		System.Array.Copy(myLevels,data.levels,myLevels.Length);
		System.Array.Copy(myScores,data.scores,myScores.Length);

		bf.Serialize(file,data);
		file.Close();
	}

	public void Load()
	{
		if(File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat",FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();
            Debug.Log(Application.persistentDataPath);
			//stuff
			for(int i = 0; i <= data.levels.Length - 1; i++)
			{
				myLevels[i] = data.levels[i];
			}

			for(int i = 0; i <= data.scores.Length - 1; i++)
			{
				myScores[i] = data.scores[i];
			}
			for(int i = 0; i <= myLevels.Length - 1; i++)
			{
				if(myLevels[i] == 0)
				{
					int tempLevel = i + 1;
					int counter = 1;
					while(tempLevel > 8)
					{
						tempLevel -= 8;
						counter += 1;
					}
					nextLevel = tempLevel;
					nextChapter = counter;
					break;
				}
			}

		}
		else
		{
			Save();
			Load();
		}
	}

}

[Serializable]
class PlayerData
{

	public int[] levels = new int[40];
	public int[] scores = new int[40];



}
