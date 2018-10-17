/*
 * Item.h
 *
 *  Created on: Apr 14, 2018
 *      Author: alex
 */

#ifndef MS3_ITEM_H_
#define MS3_ITEM_H_

#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include "task.h"

class Item
{
	std::string m_Name;
	std::string m_InstallTask;
	std::string m_RemoveTask;
	long m_SeqCode = 0;
	std::string m_Description;

public:
	Item(std::vector<std::string>&);
	bool isEmpty() const;
	std::string& getItem(){return m_Name;}
	std::string& getInstaller(){return m_InstallTask;}
	std::string& getRemover(){return m_RemoveTask;}
	void display(std::ostream& os) const;
	void graph(std::fstream&);
};

class ItemManager
{
	std::vector<Item> m_Items;
public:
	ItemManager(const char*, char);
	ItemManager(std::vector<std::vector<std::string> >&);
	void display(std::ostream& = std::cout) const;
	void graph(const char*);
	Item* find(std::string);
	bool validate(TaskManager& myMan);
	size_t count() { return m_Items.size(); }
	Item& getItem(size_t i) { return m_Items[i]; }
};

std::ostream& operator << (std::ostream& os, Item& myItem);
std::ostream& operator << (std::ostream& os, ItemManager& myManager);

#endif /* MS3_ITEM_H_ */
