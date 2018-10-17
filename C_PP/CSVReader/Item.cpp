/*
 * Item.cpp
 *
 *  Created on: Mar 31, 2018
 *      Author: alex
 */

#include <vector>
#include <string>
#include <iostream>
#include <fstream>
#include <locale>
#include <stdlib.h>
#include <sstream>
#include "util.h"
#include "Item.h"


using namespace std;


Item::Item(vector<string>& myData)
{
	//int tempNum = 0;

	try{
		switch(myData.size())
		{
		case 5:
			if(checkName(myData[4]))
			{
				m_Description = myData[4];
			}
			else
			{
				throw string("m_Description fucked up");
			}
		case 4:
			if(checkNum(myData[3]))
			{
				m_SeqCode = stoi(myData[3]);
			}
			else
			{
				throw string("m_SeqCode fucked up");
			}
		case 3:
			if(checkName(myData[2]))
			{
				m_RemoveTask = myData[2];
			}
			else
			{
				throw string("m_RemoveTask fucked up");
			}
		case 2:
			if(checkName(myData[1]))
			{
				m_InstallTask = myData[1];
			}
			else
			{
				cout << myData[1];
				throw string("m_Slots fucked up");
			}
		case 1:
			if(checkName(myData[0]))
			{
				m_Name = myData[0];
			}
			else
			{
				throw string("m_Name fucked up");
			}
			break;
		default:
			//throw("Blank Line");
			break;

		}}catch(string& error){cout << error << endl;}
		catch(...){cout << "Missing something\n";}

}

bool Item::isEmpty() const
{
	if(m_Name.empty())
	{
		return true;
	}
	return false;
}


void Item::display(ostream& os) const
{
	os << m_Name << " " << m_InstallTask << " " << m_RemoveTask << " " << m_SeqCode << "" << m_Description <<  "\n";
}

/*int getSize()const
	{
		if(!isEmpty())
		{
			if(!m_Name.empty() && !m_InstallTask.empty() && !m_RemoveTask.empty() && !m_Description.empty())
			{
				return 5;//HERE
			}
			else if(!m_Name.empty() && !m_Pass.empty())
			{
				return 3;
			}
			else
			{
				return 2;
			}
		}
		return 0;
	}*/

void Item::graph(fstream& myFile)
{
	vector<string> tempData;


	if(myFile.is_open())
	{
		try{
			if(!m_InstallTask.empty())
			{
				myFile << "\"Item " << m_Name <<"\"" " -> " << "\"Installer " << m_InstallTask << "\"" << " [color=green];" << endl;

			}
			if(!m_RemoveTask.empty())
			{
				myFile << "\"Item " << m_Name << "\"" << " -> " << "\"Remover " << m_RemoveTask << "\"" << " [color=red];" << endl;
			}
			if(m_InstallTask.empty() && m_RemoveTask.empty())
			{
				myFile << "\"" << m_Name << "\""<< " [shape=box];" << endl;
			}}catch(...){cout << "Error reading size?";}

	}
}





ItemManager::ItemManager(const char* myFName, char myDelim)
{
	fstream myFile(myFName);
	vector<vector<string> > tempData;
	csvReader(myFName, myDelim, tempData);

	if(!tempData.empty())
	{
		for(size_t i = 0; i < tempData.size(); i++)
		{
			Item tempItem(tempData[i]);
			if(!tempItem.isEmpty())
			{

				m_Items.push_back(move(tempItem));

			}
		}
	}
}
ItemManager::ItemManager(vector<vector<string> >& myData)
{
	if(!myData.empty())
	{
		for(size_t i = 0; i < myData.size(); i++)
		{
			Item tempItem(myData[i]);
			if(!tempItem.isEmpty())
			{

				m_Items.push_back(move(tempItem));

			}
		}
	}
}

void ItemManager::display(ostream& os) const
{
	for(size_t i = 0; i < m_Items.size(); i++)
	{
		m_Items[i].display(os);
	}
}

void ItemManager::graph(const char* myFName)
{
	fstream myFile(myFName, fstream::out);
	if(myFile.is_open())
	{
		myFile << "digraph tasks{ \n";
		for(size_t i = 0; i < m_Items.size(); i++)
		{
			m_Items[i].graph(myFile);
		}
		myFile << "}\n";
	}
	else
	{
		std::cout << "not created";
	}
}

Item* ItemManager::find(string myName)
{
	for(size_t i = 0; i < m_Items.size(); i++)
	{
		if(myName == m_Items[i].getItem())
		{
			return &m_Items[i];
		}
	}
	return nullptr;
}

bool ItemManager::validate(TaskManager& tm)
{
	int err = 0;

	for (auto& item : m_Items)
	{
		std::string installer = item.getInstaller();
		if (!installer.empty())
		{
			if (tm.find(installer) == nullptr)
			{
				err++;
				std::cerr << "Installer task '" << installer << "' is missing\n";
			}
		}
		std::string remover = item.getRemover();
		if (!remover.empty())
		{
			if (tm.find(remover) == nullptr)
			{
				err++;
				std::cerr << "Remover task '" << remover << "' is missing\n";
			}
		}
	}
	return err ? false : true;
}



ostream& operator << (ostream& os, Item& myItem)
{
	myItem.display(os);
	return os;
}

ostream& operator << (ostream& os, ItemManager& myManager)
{
	myManager.display(os);
	return os;
}

/*int main(int argc, char** argv)
{
	if(argc != 3)
	{
		cout << "Not enough arguments. Please provide a file and a deliminating character"<< endl;
		return 1;
	}

	vector< vector<string> > data;
	char delim = argv[2][0];
	char* fileName = argv[1];
	csvReader(fileName, delim, data);

	ItemManager tempMan(fileName, delim);
	cout << tempMan;
	cout << "Creating graph...\n";

	std::ostringstream oss;
	oss << fileName <<".gv";
	string graphName = oss.str();
	tempMan.graph(graphName.c_str());
	//oss.clear();
	cout << "Outputting graph...\n";

	std::ostringstream ossTwo;
	ossTwo << "dot -Tpng " << fileName << ".gv > " << fileName << ".gv.png";
	string cmd = ossTwo.str();
	system(cmd.c_str());

	cout << "Graph complete! File name: " << fileName << ".gv.png\n";




}*/







