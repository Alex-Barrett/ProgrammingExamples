/*
 * gameManager.h
 *
 *  Created on: Apr 22, 2018
 *      Author: alex
 */

#ifndef GAMEMANAGER_H_
#define GAMEMANAGER_H_

#include <list>
#include <stdlib.h>
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <ctime>
#include <fstream>
#include "bunny.h"


const int xMax = 80;
const int yMax = 80;

class GameManager
{
	int turn = 0;
	std::list<Bunny> bunnyList;
	int grid[xMax][yMax] = {{0}};


	GameManager(){};


	void Mate()//find bunnies that are able to reproduce
	{
		bool mating = false;
		std::list<Bunny*> females;
		for(auto& i : bunnyList) //find number of females
		{
			if(i.getSex() == 1)
			{
				females.push_back(&i);
				continue;
			}
			if(i.getAge() > 1 && i.getSex() == 0 && !i.getMutant()) //if one male exists, reproduce
			{
				mating = true;
			}
		}

		if(mating && females.size() > 0) //if females and males, reproduce
		{
			for(auto& i : females)
			{
				MakeBunny(*i);
			}
		}
	}

	void MakeBunny(int myColour) //create a new bunny and insert into List
	{
		Bunny temp(myColour);
		//std::cout << temp.getName() << " was born!\n";
		int x = 0;
		int y = 0;
		Place(x,y);
		//std::cout << x;
		temp.Place(x,y);
		bunnyList.push_back(temp);
	}

	void MakeBunny(const Bunny& myBunny) //create new bunny from mother
	{
		int colour = myBunny.getColour();
		Bunny temp(colour);
		int x = 0;
		int y = 0;
		if(Place(myBunny, x, y))
		{
			temp.Place(x,y);
			bunnyList.push_back(temp);
			std::cout << temp.getName() << " was born!\n";

		}
	}

	void CleanUp() //Find all bunnies marked dead and remove from List
	{
		auto i = bunnyList.begin();
		while(i != bunnyList.end())//cleanup the dead
		{
			if(!i->getAlive())
			{
				std::cout << "Bunny " << i->getName() << " has died!\n";
				bunnyList.erase(i++);
			}
			else
			{
				i++;
			}
		}
	}

	void Zombify()
	{
		std::list<Bunny*> tempList;

		for(auto i = bunnyList.begin(); i != bunnyList.end(); i++)
		{
			if(i->getMutant())
			{
				for(auto x = bunnyList.begin(); x != bunnyList.end(); x++)
				{
					if(*i == *x || x->getMutant())
					{
						continue;
					}
					if(i->getX() - x->getX() < 2 && i->getX() - x->getX() > -2 && i->getY() - x->getY() < 2 && i->getY() - x->getY() > -2)
					{
						tempList.push_back(&(*x));

					}
				}
			}
		}

		for(auto i = tempList.begin(); i != tempList.end(); i++)
		{
			(*i)->Zombify();
		}
	}


	bool Place(int& x, int& y) //find random place on grid
	{
		bool found = false;
		int tempX;
		int tempY;
		while(!found)
		{
			tempX= rand() % xMax;
			tempY = rand() % yMax;
			if(grid[tempX][tempY] == 0)
			{
				found = true;
				x = tempX;
				y = tempY;
				grid[tempX][tempY] = 1;
			}
		}
		return found;
	}

	bool Place(const Bunny& myBunny, int& x, int& y) //return xy of empty adjacent square
	{
		int t_X = myBunny.getX();
		int t_Y = myBunny.getY();
		int adjX[] = {t_X -1, t_X-1, t_X-1, t_X, t_X+1, t_X+1,t_X+1, t_X}; //matrix
		int adjY[] = {t_Y-1, t_Y, t_Y+1, t_Y+1, t_Y+1, t_Y, t_Y-1, t_Y-1};
		bool found = false;
		for(int  i = 0; i < 8; i++)
		{
			if(adjX[i] > xMax -1 || adjX[i] < 0 || adjY[i] > yMax -1 || adjY[i] < 0)
			{
				continue;
			}
			if(grid[adjX[i]][adjY[i]] == 0)
			{
				found = true;
				x = adjX[i];
				y = adjY[i];
				break;
			}
		}
		return found;
	}

	void PrintGrid(std::ostream& os)
	{
		char pGrid[xMax][yMax];
		std::fill(&pGrid[0][0], &pGrid[0][0]+ sizeof(pGrid), '+');
		for(auto i = bunnyList.begin(); i != bunnyList.end(); i++)
		{
			pGrid[i->getX()][i->getY()] = i->getGridChar();
			//os << i->getGridChar() << std::endl;
			//os << i->getX() << std::endl;
		}
		for(int i = 0; i < xMax; i++)
		{
			os << '\n';
			for(int y = 0; y < yMax; y++)
			{
				os << pGrid[i][y];
			}
		}

	}

	void Move(Bunny& myBunny)
	{
		int x = 0;
		int y = 0;
		if(Place(myBunny, x, y)) //call Place to get an adjacent square
		{
			grid[myBunny.getX()][myBunny.getY()] = 0;
			grid[x][y] = 1;
			myBunny.Place(x,y);
		}

	}


public:
	static GameManager& getInstance() //for singleton
	{
		static GameManager instance; //make static instance and return
		return instance;
	}

	GameManager(const GameManager&) = delete; //for singleton
	void operator =(const GameManager&) = delete;//for singleton

	bool Initialize() //startup game, make 5 bunnies
	{
		if(turn == 0)
		{
			srand(time(NULL));
			for(int i = 0; i < 5; i++)
			{
				MakeBunny(rand() % 4);
			}
			turn = 1;
		}
		else
		{
			return false;
		}
		return false;
	}

	bool InitialzeLoad(std::fstream& myLoad)
	{
		try
		{
			srand(time(NULL));
			myLoad >> turn;
			std::string tName;
			int tAge;
			int tSex;
			int tColour;
			int tX;
			int tY;
			bool tMut;
			bool tLive;
			if(myLoad.is_open())
			{

				while(std::getline(myLoad >> std::ws, tName, '\n'))
				{
					myLoad >> tAge >> tSex >> tColour >> tX >> tY >> tMut >> tLive;
					Bunny temp(tName, tAge, tSex, tColour, tX, tY, tMut, tLive);
					bunnyList.push_back(temp);
					//myLoad.ignore();
				}
			}

		}catch(...){std::cout << "FAILED TO LOAD!";return Initialize();}
		return true;
	}

	bool NextTurn(std::ostream& os) //start next turn
	{
		if(bunnyList.size() == 0) // check for bunnies
		{
			return false;
		}
		bool flag = false;
		for(auto& i : bunnyList) // check if theres a non mutant
		{
			if(!i.getMutant())
			{
				flag = true;
				break;
			}
		}
		if(!flag) //No non mutants, game over
		{
			return false;
		}

		for_each(bunnyList.begin(), bunnyList.end(), [&](Bunny& myBunny){myBunny.NextAge();}); //age all bunnies, we need to remove dead bunnies

		os << "Bring out your dead!\n";
		CleanUp(); //Cleanup bunnies that are marked dead
		os << "Mating\n";
		Mate(); //Reproduce

		for_each(bunnyList.begin(), bunnyList.end(), [&](Bunny& myBunny){Move(myBunny);}); //move each bunny

		os << "New zombies!\n";
		//for_each(bunnyList.begin(), bunnyList.end(), [&](Bunny& myBunny){if(myBunny.getMutant()){Zombify(myBunny);}}); //find Zombies, send to Zombify
		//os << "Got here\n";
		Zombify();

		os << "Turn: " << turn << "\n";
		os << "Age| Colour|Sex|    Name|Mutant?\n";
		os << "--------------------------------\n";
		std::list<Bunny> tempList(bunnyList); //copy list to organise and print
		tempList.sort([](const Bunny& a, const Bunny& b) -> bool{return a.getAge() < b.getAge();});//sort the temp bunnies
		for_each(tempList.begin(), tempList.end(), [](const Bunny& myBunny){myBunny.print(std::cout);}); //print the bunnies

		if(bunnyList.size() == 0)
		{
			return false;
		}
		if(bunnyList.size() > 1000)
		{
			std::cout << "Too many bunnies!\n";
			Cull();
		}

		PrintGrid(std::cout);

		turn++;
		return true;
	}

	void Cull()
	{
		std::cout << "Off with their heads!\n";
		size_t size = bunnyList.size()/2;
		while(size > 0)
		{
			for(auto i = bunnyList.begin(); i != bunnyList.end(); i++)
			{
				if(rand() % 2 == 1)
				{
					i->Cull();
					size--;
				}
				if(size == 0)
				{
					break;
				}
			}
		}
		CleanUp();
	}

	void SaveGame(std::fstream& myFile)const
	{

		//std::ofstream file(myFile);

		myFile << turn<< std::endl;
		for(auto i = bunnyList.begin(); i != bunnyList.end(); i++)
		{
			myFile << i->getName()<< '\n';
			myFile << i->getAge()<< '\n';
			myFile << i->getSex() << '\n';
			myFile << i->getColour() <<'\n';
			myFile << i->getX() << '\n';
			myFile << i->getY() << '\n';
			myFile << i->getMutant() << '\n';
			myFile << i->getAlive() << '\n';
		}
		//myFile.close();
	}



	int getTurn()const{return turn;}

};



#endif /* GAMEMANAGER_H_ */
