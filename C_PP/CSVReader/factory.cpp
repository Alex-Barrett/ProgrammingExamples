#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include "util.h"
#include "Item.h"
#include "order.h"
#include "task.h"
#include "job.h"
#include "machine.h"

using namespace std;


class Factory
{

	vector<Machine> m_Machines;

public:
	Factory(ItemManager& im, OrderManager& om, TaskManager& tm)
	{
		for (size_t i = 0; i < tm.count(); ++i)
		{
			m_Machines.push_back(tm.getTask(i));
		}
		for (auto& m : m_Machines)
		{
			cout << m.getTask() << "\n";
		}

		for (auto& m : m_Machines)
		{
			auto find = [&](string name) -> Machine*
			{
				for (size_t i = 0; i < m_Machines.size(); ++i)
				{
					if (m_Machines[i].getTask() == name)
					{
						return &m_Machines[i];
					}
				}
				return nullptr;
			};

			string pass = m.getPass();
			if (!pass.empty()) {
				find(pass) -> incIncoming();
			}
			string fail = m.getFail();
			if (!fail.empty())
			{
				find(fail) -> incIncoming();
			}
		}
		for (auto& m : m_Machines)
		{
			cout << m.getTask();
			if (m.isSource())
			{
				cout << " <--SOURCE";
			}
			if (m.isSink())
			{
				cout << " <--SINK";
			}
			cout << "\n";
		}

		int sourceNode = -1;
		for (size_t i = 0; i < m_Machines.size(); ++i)
		{
			if (m_Machines[i].isSource())
			{
				if (sourceNode == -1)
				{
					sourceNode = i;
				}
				else
				{
					cerr << "nodes " << sourceNode << " and " << i << " are both source nodes\n";
					throw string("multiple source nodes. Fix task data and resubmit!");
				}
			}
		}
		if (sourceNode == -1)
		{
			throw string("missing source node. Fix task data and resubmit!");
			cout << "sourceNode = " << sourceNode << " (" << m_Machines[sourceNode].getTask() << ")\n";
		}

		for (size_t i = 0; i < om.count(); ++i)
		{
			m_Machines[sourceNode].addJob(move(Job(om.getOrder(i))));
		}

		auto activeMachines = [&]
		{
			int ret = 0;
			for (auto& m : m_Machines) {
				if (m.jobCount())
				{
					ret += m.jobCount();
					cout << m.getTask() << " has " << m.jobCount() << " jobs.\n";
				}
			}
			return ret;
		};

		int time = 0;

		while (true) {
			if (activeMachines() == 0)
			{
				break;
			}

			for (auto& m : m_Machines) {
				if (m.jobCount() == 0) {
					continue;
				}

				Job job = move(m.getJob());

				if (m.isSink()){
					if (job.complete()){
						cout << "job " << job.getCustomer() << "/" << job.getProduct() << " finished at machine " << m.getTask() << "\n";
					}
					else
					{
						cout << "job " << job.getCustomer() << "/" << job.getProduct() << " not complete. Rerouting back to sourceNode\n";
						m_Machines[sourceNode].addJob(move(job));
					}
				}

				bool didSomething = false;
				if (m.isInstaller())
				{
					for (size_t i = 0; i < job.itemsOrderedSize(); ++i) {
						if (job.getInstalled(i) == true)
						{
							continue;
						}
						string item = job.itemsOrderedItem(i);
						string installer = im.find(item)->getInstaller();
						if (installer == m.getTask())
						{
							job.setInstalled(i, true);
							didSomething = true;
							break;
						}
					}
				}

				if (m.isRemover())
				{
					for (size_t i = job.itemsOrderedSize() - 1; i >= 0; --i) {
						if (job.getInstalled(i) == false)
						{
							continue;
						}
						string item = job.itemsOrderedItem(i);
						string remover = im.find(item)->getRemover();
						if (remover == m.getTask())
						{
							job.setInstalled(i, false);
							didSomething = true;
							break;
						}
					}
				}

				auto route = [&](string name) {
					for (auto& m : m_Machines) {
						if (m.getTask() == name)
						{
							m.addJob(move(job));
							break;
						}
					}

				};

				string pass = m.getPass();
				string fail = m.getFail();

				if (!didSomething){
					route(pass);
					continue;
				}

				if (fail.empty()){
					route(pass);
					continue;
				}

				if (rand() & 1){
					route(pass);
					continue;
				}
				else
				{
					route(fail);
					continue;
				}

			}
		}
		cout << "ALL DONE\n";
	}
};


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
		char* itemFile = argv[1];
		char* orderFile = argv[2];
		char* taskFile = argv[3];

		csvReader(taskFile, delim, taskData);
		csvReader(itemFile, delim, itemData);
		csvReader(orderFile, delim, orderData);

		TaskManager tempTask(taskData);
		ItemManager tempItem(itemData);
		OrderManager tempOrder(orderData);
		//cout << tempTask;
		//cout << tempItem;
		//cout << tempOrder;
		try{
			Factory(tempItem, tempOrder, tempTask);
		}catch(string& err){cerr << err << '\n';}
		//catch(string err){cout << err << '\n';}
		catch(...){cerr << "Unknown Error\n";}


		genGraph(tempTask, taskFile);
		genGraph(tempItem, itemFile);
		genGraph(tempOrder, orderFile);

		bool good = true;
			if (!tempItem.validate(tempTask))
			{
				cout << "item data not valid" << "\n";
				good = false;
			}
			if (!tempTask.validate())
			{
				cout << "task data not valid" << "\n";
				good = false;
			}
			if (!tempOrder.validate(tempItem))
			{
				cout << "order data not valid" << "\n";
				good = false;
			}



	return 0;
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











