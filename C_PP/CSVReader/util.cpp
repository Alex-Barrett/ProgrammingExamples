/*
 * util.cpp
 *
 *  Created on: Mar 31, 2018
 *      Author: alex
 */




#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <utility>
#include "util.h"

//using namespace std;

void printByRange(std::ostream& myOutput, std::vector<std::vector<std::string> >& myData)
{
	for(auto row: myData)
	{
		for(auto col: row)
		{
			myOutput << col << " ";
		}
		myOutput << std::endl;
	}
	myOutput << std::endl;
}

void printByLoop(std::ostream& myOutput, std::vector<std::vector<std::string> >& myData)
{
	for(size_t i = 0; i < myData.size(); i++)
	{
		for(size_t x = 0; x < myData[i].size(); x++ )
		{
			myOutput << myData[i][x] << " ";
		}
		myOutput << std::endl;
	}
	myOutput << std::endl;
}

void printByIterator(std::ostream& myOutput, std::vector<std::vector<std::string> >& myData)
{
	for(auto it = myData.begin(); it < myData.end(); it++)
	{
		for(auto xt = it->begin(); xt < it->end(); xt++)
		{
			myOutput << *xt << " ";
		}
		myOutput << std::endl;
	}
	myOutput << std::endl;
}

std::string& trim(std::string& myString, char myDelim)
{

	while(!myString.empty() && myString[0] == myDelim)
	{
		myString.erase(0, 1);
	}


	while(!myString.empty() && myString[myString.size()-1] == myDelim)
	{
		myString.erase(myString.size() -1, 1);
	}

	return myString;
}

void csvReader(const char* myFileName, char myDelim, std::vector< std::vector<std::string> >& myData)
{
	std::fstream myFile(myFileName);


		//cout << "HERE";
		if(myFile.is_open())
		{

			std::string tempString;
			std::vector<std::string> toPush;
			try{
			while(getline(myFile, tempString))
			{
				auto cr = tempString.find('\r');
				if(cr != std::string::npos)
				{
					tempString.erase(cr, 1);
				}
				std::string fieldString;

				for(size_t i = 0; i < tempString.length(); i++)
				{

					if(tempString[i] != myDelim)
					{
						fieldString.push_back(tempString[i]); //Need to put into temp array then into 2d array
					}
					else
					{

						trim(fieldString);
						if(!fieldString.empty())
						{
							toPush.push_back(std::move(fieldString));
						}
					}
				}
				//cout << data[count][iCount];

				trim(fieldString);
				if(!fieldString.empty())
				{
					toPush.push_back(std::move(fieldString));
				}
				if(!toPush.empty())
				{
					myData.push_back(move(toPush));
				}

			}}catch(...){std::cout << "Somethin really bad happened";}
		}
		else
		{
			std::cout << "Cannot open file" << std::endl;
		}

}

bool checkName(const std::string& myName)
{
	if(myName.length() > 1 && (isalnum(myName[0]) || myName[0] == '_') )
	{
		return true;
	}
	return false;
}

bool checkNum(const std::string& myNum)
{
	if(myNum.length() > 0 && isdigit(myNum[0]))
	{
		return true;
	}
	return false;
}







