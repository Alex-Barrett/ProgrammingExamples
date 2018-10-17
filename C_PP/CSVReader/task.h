/*
 * task.h
 *
 *  Created on: Apr 14, 2018
 *      Author: alex
 */

#ifndef TASK_H_
#define TASK_H_

#include <string>
#include <vector>
#include <iostream>
#include <fstream>

class Task
{
	std::string m_Name;
	int m_Slots = 1;
	std::string m_Pass;
	std::string m_Fail;

public:
	Task(){};
	//Task(Task&);
	Task(std::vector<std::string>&);
	bool isEmpty() const;
	std::string& getTask(){return m_Name;}
	std::string& getPass(){return m_Pass;}
	std::string& getFail(){return m_Fail;}
	void display(std::ostream&) const;
	void graph(std::fstream&);
};

class TaskManager
{
	std::vector<Task> m_Tasks;
public:
	TaskManager(const char*, char );
	TaskManager(std::vector<std::vector<std::string> >&);
	void display(std::ostream&) const;
	void graph(const char*);
	Task* find(std::string);
	bool validate();
	size_t count() { return m_Tasks.size(); }
	Task& getTask(size_t i) { return m_Tasks[i]; }
};

std::ostream& operator << (std::ostream& os, Task& myTask);
std::ostream& operator << (std::ostream& os, TaskManager& myManager);


#endif /* TASK_H_ */
