/*
 * job.h
 *
 *  Created on: Apr 19, 2018
 *      Author: alex
 */

#ifndef JOB_H_
#define JOB_H_



#include "order.h"


class Job : public Order {

	std::vector<bool> installed;

public:
	Job()
	{
	}

	Job(Order& o)
	{
		for (size_t i = 0; i < itemsOrderedSize(); ++i)
		{
			installed.push_back(false);
		}
	}

	bool getInstalled(size_t i) { return installed[i]; }
	void setInstalled(size_t i, bool v) { installed[i] = v; }

	bool complete()
	{
		for(auto e : installed)
			if (!e)
			{
				return false;
			}
		return true;
	}
};




#endif /* JOB_H_ */
