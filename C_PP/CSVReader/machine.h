/*
 * machine.h
 *
 *  Created on: Apr 19, 2018
 *      Author: alex
 */

#ifndef MACHINE_H_
#define MACHINE_H_


#include <string>
#include <queue>
#include "task.h"
#include "Item.h"
#include "job.h"

class Machine : public Task {
	int incoming = 0;
	std::queue<Job> jobQ;
	bool bInstaller = false;
	bool bRemover = false;

public:
	Machine() {}
	Machine(Task& t) : Task(t) {}

	void incIncoming() { incoming++; }
	bool isSource() { return incoming == 0; }
	bool isSink() { return getPass().empty() && getFail().empty(); }

	void addJob(Job&& j) { jobQ.push(std::move(j)); }
	size_t jobCount() { return jobQ.size(); }
	Job&& getJob() { Job j = std::move(jobQ.front()); jobQ.pop(); return std::move(j); }
	void classify(ItemManager& im)
	{
		for (size_t i = 0; i < im.count(); ++i)
		{
			std::string installer = im.getItem(i).getInstaller();
			if (installer == getTask())
			{
				bInstaller = true;
			}
		}

		if (bInstaller && bRemover)
		{
			throw std::string(" machine ") + getTask() + " is both an installer and a remover - Fix item\n";
		}
	}
	bool isInstaller() { return bInstaller; }
	bool isRemover() { return bRemover; }
};




#endif /* MACHINE_H_ */
