#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "util.h"
#include "Item.h"
#include "order.h"
#include "task.h"

using namespace std;

template<class T>
void genGraph(T& myMan, const char* myFName)
{

	string graphName = string(myFName) + ".gv";
	cout << "Creating graph: " << graphName << "\n";

	try
	{
		myMan.graph(graphName.c_str());
		string cmd = string("dot -Tpng ") + myFName + ".gv > " + myFName + ".gv.png";
		system(cmd.c_str());
		cout << "Graph complete! File name: " << graphName << ".gv.png\n";
	}
	catch(...){cout << "Unable to graph: " << graphName;}
}

int app(int argc, char* argv[])
{
	if (argc != 5)
	{
		throw string("usage: ") + argv[0] + "csv-item-file-name / csv-order-file-name / csv-task-file-name / csv-separator-character";
	}


		vector< vector<string> > taskData;
		vector< vector<string> > itemData;
		vector< vector<string> > orderData;

		char delim = argv[4][0];
		char* taskFile = argv[1];
		char* itemFile = argv[2];
		char* orderFile = argv[3];

		csvReader(taskFile, delim, taskData);
		csvReader(itemFile, delim, itemData);
		csvReader(orderFile, delim, orderData);

		TaskManager tempTask(taskData);
		ItemManager tempItem(itemData);
		OrderManager tempOrder(orderData);
		cout << tempTask;
		cout << tempItem;
		cout << tempOrder;


	if (tempItem.validate(tempTask) && tempTask.validate() && tempOrder.validate(tempItem))
	{
		cout << "No errors!" << "\n";
	}
	else
	{
		cout << "Error" << "\n";
		char in;
		cout << "Create graphs anyway?[y/n]: ";
		cin >> in;
		if(in != 'y' && in != 'Y')
		{
			return 1;
		}
	}

	genGraph(tempTask, taskFile);
	genGraph(tempItem, itemFile);
	genGraph(tempOrder, orderFile);

}



int main(int argc, char* argv[])
{
	try {
		app(argc, argv);
	}
	catch (const string& e) {
		cerr << "It threw: " + e + "\n";
	}
}



