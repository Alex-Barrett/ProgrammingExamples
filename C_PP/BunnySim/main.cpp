/*
 * main.cpp
 *
 *  Created on: Apr 21, 2018
 *      Author: alex
 */

#include <list>
#include <stdlib.h>
#include <iostream>
#include <vector>
#include <string>
#include <fstream>
#include "bunny.h"
#include "gameManager.h"

using namespace std;



int main(int argc, char* argv[])
{
	GameManager& myManager = GameManager::getInstance();
	string saveName;
	fstream saveFile;

	if(argc > 1)
	{
		saveName = argv[1];
		saveFile.open(saveName, fstream::in | fstream::out);
		if(saveFile)
		{
			cout << "Save File exists" << endl;
			if(saveFile.peek() == std::ifstream::traits_type::eof())
			{
				cout<< "SaveFile is empty" << endl;
				myManager.Initialize();
			}
			else
			{
				cout << "Attempting to load save" << endl;
				myManager.InitialzeLoad(saveFile);
			}
		}
		else
		{
			cout << "Save file does not exist" << endl;
			saveFile.open("BunnySave.txt", fstream::out);
			saveName = "BunnySave.txt";
			myManager.Initialize();
		}
	}
	else
	{
		saveFile.open("BunnySave.txt", fstream::out);
		saveName = "BunnySave.txt";
		myManager.Initialize();
	}


	//cout << argc<< endl;

	while(myManager.NextTurn(cout))
	{
		saveFile.open(saveName, fstream::out);
		myManager.SaveGame(saveFile);
		saveFile.close();
		char x;
		cout << "Press enter for next turn or 'k' to Cull...\n";
		x = cin.get();


		if(x == '\n')
		{
			//cin.ignore();
			continue;
		}
		else if(x == 'k')
		{
			myManager.Cull();
			cin.ignore();
		}
		cin.clear();
	}
	cout << "GAME OVER\n";
	return 0;
}

