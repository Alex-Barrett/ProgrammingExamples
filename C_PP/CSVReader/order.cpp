/*
 * order.cpp
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
#include "order.h"


using namespace std;


Order::Order(vector<string>& myData)
{
	try
	{
		if(myData.size() >= 3)
		{
			if(checkName(myData[0]))
			{
				m_Name = myData[0];
			}
			else
			{
				throw string("The Name is shit");
			}

			if(checkName(myData[1]))
			{
				m_ProdName = myData[1];
			}
			else
			{
				throw string("The Product Name is shit");
			}

			for(size_t i = 2; i < myData.size(); i++)
			{
				if(checkName(myData[i]))
				{
					m_Items.push_back(myData[i]);
				}
				else
				{
					throw string("Shit Item at position: ") + to_string(i);
				}
			}
			//cout << "AND I HAVE " << m_Items.size() << endl;
		}
	}
	catch(string & error){cout << error << endl;}
	catch(...){}

}

bool Order::isEmpty() const
{
	if(m_Name.empty())
	{
		return true;
	}
	return false;
}



void Order::display(ostream& os) const
{
	os << m_Name << " " << m_ProdName;
	for(size_t i = 0; i < m_Items.size(); i++)
	{
		os << " " << m_Items[i];
	}
	os << endl;
}


void Order::graph(fstream& myFile)
{
	vector<string> tempData;


	if(myFile.is_open())
	{
		try
		{
			if(m_Items.size() > 0)
			{
				for(size_t i = 0; i < m_Items.size(); i++)
				{
					myFile << "\"" << m_Name << " " << m_ProdName << "\"" << " -> " << "\"Item " << m_Items[i] << "\"" << " [color=blue];" << endl;
				}
			}
		}catch(...){cout << "Error while writing file\n";}

	}
}





OrderManager::OrderManager(const char* myFName, char myDelim)
{
	fstream myFile(myFName);
	vector<vector<string> > tempData;
	csvReader(myFName, myDelim, tempData);

	if(!tempData.empty())
	{
		for(size_t i = 0; i < tempData.size(); i++)
		{
			Order tempOrder(tempData[i]);
			if(!tempOrder.isEmpty())
			{

				m_Orders.push_back(move(tempOrder));

			}
		}
	}
	//cout << "I HAVE " << m_Orders.size() << endl;
}
OrderManager::OrderManager(vector<vector<string> >& myData)
{
	if(!myData.empty())
	{
		for(size_t i = 0; i < myData.size(); i++)
		{
			Order tempOrder(myData[i]);
			if(!tempOrder.isEmpty())
			{

				m_Orders.push_back(move(tempOrder));

			}
		}
	}
}

void OrderManager::display(ostream& os) const
{
	for(size_t i = 0; i < m_Orders.size(); i++)
	{
		m_Orders[i].display(os);
	}
}

void OrderManager::graph(const char* myFName)
{
	fstream myFile(myFName, fstream::out | fstream::trunc);
	if(myFile.is_open())
	{
		myFile << "digraph tasks{ \n";
		for(size_t i = 0; i < m_Orders.size(); i++)
		{
			m_Orders[i].graph(myFile);
		}
		myFile << "}\n";
	}
	else
	{
		std::cout << "graph not created";
	}
}

bool OrderManager::validate(ItemManager& om)
{
	int err = 0;

	for (auto& o : m_Orders)
	{
		size_t size = o.itemsOrderedSize();
		for (size_t i = 0; i < size; i++)
		{
			if (om.find(o.itemsOrderedItem(i)) == nullptr)
			{
				err++;
				std::cerr << "Cannot find '" + o.itemsOrderedItem(i) << "' \n";
			}
		}
	}
	return err ? false : true;
}



ostream& operator << (ostream& os, Order& myOrder)
{
	myOrder.display(os);
	return os;
}

ostream& operator << (ostream& os, OrderManager& myManager)
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

	OrderManager tempMan(fileName, delim);
	OrderManager secondTemp(data, delim);
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







