/*
 * order.h
 *
 *  Created on: Apr 14, 2018
 *      Author: alex
 */

#ifndef ORDER_H_
#define ORDER_H_

#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include "Item.h"

class Order
{
	std::string m_Name;
	std::string m_ProdName;
	std::vector<std::string> m_Items;

public:
	Order(){};
	Order(std::vector<std::string>&);
	bool isEmpty() const;
	void display(std::ostream&) const;
	void graph(std::fstream&);
	size_t itemsOrderedSize() { return m_Items.size();}
	std::string& itemsOrderedItem(size_t i) { return m_Items[i];}
	std::string& getCustomer() { return m_Name; }
	std::string& getProduct() { return m_ProdName; }
};

class OrderManager
{
	std::vector<Order> m_Orders;
public:
	OrderManager(const char*, char);
	OrderManager(std::vector<std::vector<std::string> >&);
	void display(std::ostream&) const;
	void graph(const char*);
	bool validate(ItemManager&);
	size_t count() { return m_Orders.size(); }
	Order& getOrder(size_t i) { return m_Orders[i]; }
};

std::ostream& operator << (std::ostream& os, Order& myOrder);
std::ostream& operator << (std::ostream& os, OrderManager& myManager);


#endif /* ORDER_H_ */
