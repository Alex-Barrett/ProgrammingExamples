/*
 * bunny.h
 *
 *  Created on: Apr 21, 2018
 *      Author: alex
 */

#ifndef BUNNY_H_
#define BUNNY_H_

#include <string>
#include <stdlib.h>

enum Sex{MALE, FEMALE}; //0,1
enum Colour{white, black, brown, spotted};
const int maleSize = 7;
const int femSize = 8;
std::string maleNames[] = {"Mark", "Jezz", "Hans","Jeff", "Iain", "Alan", "Gerrard"};
std::string femaleNames[]{"Dobby", "Big Suze", "Sophie", "Toni", "Nancy", "Elena", "Zahra", "Gail"};

class Bunny
{
	Sex m_Sex;
	Colour m_Colour;
	int m_Age;
	int m_Xpos = -1;
	int m_Ypos = -1;
	std::string m_Name;
	bool m_Mutant = false;
	bool m_Alive = true;


public:
	Bunny(){}

	Bunny(int myColour) : m_Colour((Colour)myColour) // pass colour, baby has to be mums colour
	{
		m_Sex = (Sex)(rand() % 2); //0-1
		m_Age = 0;
		m_Mutant = !(rand() % 50); //2% chance of being true
		if(m_Sex == MALE)
		{
			m_Name = maleNames[rand() % maleSize];
		}
		else
		{
			m_Name = femaleNames[rand() % femSize];
		}

	}

	Bunny(std::string myName, int myAge, int mySex, int myColour, int myX, int myY, bool myMutant, bool myAlive)
	{
		m_Name = myName;
		m_Age = myAge;
		m_Sex = (Sex)mySex;
		m_Colour = (Colour)myColour;
		m_Xpos = myX;
		m_Ypos = myY;
		m_Mutant = myMutant;
		m_Alive = myAlive;
	}

	std::ostream& print(std::ostream& os)const
	{
		os <<std::right;
		os.width(4); os << std::right << m_Age;
		os.width(8); os << StringColour(m_Colour);
		os.width(4); os << StringSex(m_Sex);
		os.width(9); os << m_Name;
		os.width(7); os << m_Mutant;
		os << std::endl;
		return os;
	}

	void NextAge()
	{
		m_Age++;
		if(!m_Mutant && m_Age > 10)
		{
			m_Alive = false;
		}
		else if(m_Age > 50)
		{
			m_Alive = false;
		}
	}

	void Cull()
	{
		m_Alive = false;
	}

	std::string StringColour(Colour myColour)const
	{
		switch(myColour)
		{
		case white: return "White";
		case black: return "Black";
		case brown: return "Brown";
		case spotted: return "Spotted";
		}
	}

	char StringSex(Sex mySex)const
	{
		switch(mySex)
		{
		case MALE: return 'M';
		case FEMALE: return 'F';
		}
	}

	void Place(int x, int y)
	{
		m_Xpos = x;
		m_Ypos = y;
	}

	void Zombify()
	{
		m_Mutant = true;
	}

	void setColour(int myColour){m_Colour = Colour(myColour);}
	void setAge(int myAge){m_Age = myAge;}
	void setSex(int mySex){m_Sex = Sex(mySex);}
	void setY(int myY){m_Ypos = myY;}
	void setX(int myX){m_Xpos = myX;}
	void setMutant(bool myMut){m_Mutant = myMut;}
	void setAlive(bool myAlive){m_Alive = myAlive;}
	void setName(std::string myName){m_Name = myName;}

	int getColour()const {return (int)m_Colour;}
	int getAge()const{return m_Age;}
	int getSex()const{return (int)m_Sex;}
	int getX()const{return m_Xpos;}
	int getY()const{return m_Ypos;}
	bool getMutant()const{return m_Mutant;}
	bool getAlive()const{return m_Alive;}
	std::string getName()const{return m_Name;}
	char getGridChar()const
	{
		if(m_Mutant)
		{
			return 'X';
		}
		if(getSex() == 0) //if Male
		{
			return getAge() < 2 ? 'm':'M'; //if age < 2 return m, else M
		}
		else //if Female
		{
			return getAge() < 2 ? 'f':'F'; //if age <2 return f, else F
		}
	}

	bool operator ==(Bunny& lhs) const
	{
		return (this == &lhs ? true:false);
	}


};






#endif /* BUNNY_H_ */
