/*
 * task.cpp
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
#include "task.h"


using namespace std;



/*Task::Task(Task& myCopy)
{
	if(this != &myCopy)
	{
		this->m_Name = myCopy.m_Name;
		this->m_Slots = myCopy.m_Slots;
		this->m_Pass = myCopy.m_Pass;
		this->m_Fail = myCopy.m_Fail;
	}
}*/

Task::Task(vector<string>& myData)
{
	int tempNum = 0;

	try{
		switch(myData.size())
		{
		case 4:
			if(checkName(myData[3]))
			{
				m_Fail = myData[3];
			}
			else
			{
				throw string("m_Fail fucked up");
			}
		case 3:
			if(checkName(myData[2]))
			{
				m_Pass = myData[2];
			}
			else
			{
				throw string("m_Pass fucked up");
			}
		case 2:
			if(checkNum(myData[1]))
			{
				m_Slots = stoi(myData[1], nullptr);
			}
			else
			{
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

bool Task::isEmpty() const
{
	if(m_Name.empty())
	{
		return true;
	}
	return false;
}



void Task::display(ostream& os) const
{
	os << m_Name << " " << m_Slots << " " << m_Pass << " " << m_Fail << "\n";

}



void Task::graph(fstream& myFile)
{
	if(myFile.is_open())
	{
		if(!m_Pass.empty())
		{
			myFile << "\"" << m_Name <<"\"" " -> " << "\"" << m_Pass << "\"" << " [color=green];" << endl;
		}
		if(!m_Fail.empty())
		{
			myFile << "\"" << m_Name << "\"" << " -> " << "\"" << m_Fail << "\"" << " [color=red];" << endl;
		}

		if(m_Pass.empty() && m_Fail.empty())
		{
			myFile << "\"" << m_Name << "\""<< " [shape=box];" << endl;
		}
	}
}





TaskManager::TaskManager(const char* myFName, char myDelim)
{
	fstream myFile(myFName);
	vector<vector<string> > tempData;
	csvReader(myFName, myDelim, tempData);

	if(!tempData.empty())
	{
		for(size_t i = 0; i < tempData.size(); i++)
		{
			Task tempTask(tempData[i]);
			if(!tempTask.isEmpty())
			{

				m_Tasks.push_back(move(tempTask));

			}
		}
	}
}
TaskManager::TaskManager(vector<vector<string> >& myData)
{
	if(!myData.empty())
	{
		for(size_t i = 0; i < myData.size(); i++)
		{
			Task tempTask(myData[i]);
			if(!tempTask.isEmpty())
			{

				m_Tasks.push_back(move(tempTask));

			}
		}
	}
}

void TaskManager::display(ostream& os) const
{
	for(size_t i = 0; i < m_Tasks.size(); i++)
	{
		m_Tasks[i].display(os);
	}
}

void TaskManager::graph(const char* myFName)
{
	fstream myFile(myFName, fstream::out);
	if(myFile.is_open())
	{
		myFile << "digraph tasks{ \n";
		for(size_t i = 0; i < m_Tasks.size(); i++)
		{
			m_Tasks[i].graph(myFile);
		}
		myFile << "}\n";
	}
	else
	{
		std::cout << "not created";
	}
}

Task* TaskManager::find(string myName)
{
	for(size_t i = 0; i < m_Tasks.size(); i++)
	{
		if(myName == m_Tasks[i].getTask())
		{
			return &m_Tasks[i];
		}
	}
	return nullptr;
}

bool TaskManager::validate()
{
	int err = 0;

	for (auto& t : m_Tasks)
	{
		std::string pass = t.getPass();
		if (!pass.empty())
		{
			if (find(pass) == nullptr)
			{
				err++;
				std::cerr << "Pass task '" << pass << "' is missing\n";
			}
		}
		std::string fail = t.getFail();
		if (!fail.empty())
		{
			if (find(fail) == nullptr)
			{
				err++;
				std::cerr << "Fail task '" << fail << "' is missing\n";
			}
		}
	}
	return err ? false : true;
}



ostream& operator << (ostream& os, Task& myTask)
{
	myTask.display(os);
	return os;
}

ostream& operator << (ostream& os, TaskManager& myManager)
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

	TaskManager tempMan(fileName, delim);
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


	//printByRange(cout, data);
	//printByLoop(cout, data);
	//printByIterator(cout, data);

}*/







