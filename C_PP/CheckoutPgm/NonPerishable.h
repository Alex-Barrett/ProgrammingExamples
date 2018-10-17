#ifndef SICT_NONPERISHABLE_H
#define SICT_NONPERISHABLE_H


#include<iostream>
#include<fstream>
#include "Product.h"
#include "ErrorMessage.h"

namespace sict
{
	const int max_sku_length = 7;
	const int max_unit_length = 10;
	const int max_name_length = 75;
	const int TAXRATE = 13;

	class NonPerishable : public Product //use abstract for base
	{
		char mType;
		char mSKU[max_sku_length + 1];
		char* mName;
		char mUnit[max_unit_length + 1];
		int mQuantity;
		int mQtyNeeded;
		double mPrice;
		bool mTaxable;
		ErrorMessage mState; //current error

	protected:
		void name(const char*); //update name
		const char* name()const; //return name
		double cost()const; //return cost
		void message(const char*); //update error message
		bool isClear()const; //check if error is clear
		void setEmpty(); //set to default empty

	public:
		NonPerishable(char = 'N'); //set empty, default type is N
		NonPerishable(const char*, const char*, const char*, int = 0, bool = true, double = 0, int = 0); //set everything, default type is N
		NonPerishable(const NonPerishable&); // copy constructor
		~NonPerishable();

		double total_cost()const; //all items * cost of individual item
		void quantity(int); //update quantity
		bool isEmpty()const; //check if item is empty
		int qtyNeeded()const; //min quantity
		int quantity()const; //return quantity in stock
		std::fstream& store(std::fstream&, bool = true)const; //store info in file
		std::fstream& load(std::fstream&); //read info from file
		std::ostream& write(std::ostream&, bool)const; //write info to screen
		std::istream& read(std::istream&); //read info from screen
		
		NonPerishable& operator=(const NonPerishable&);
		bool operator==(const char*)const;
		bool operator>(const char*)const;
		int operator+=(int);
		bool operator>(const Product&)const;

	};

	std::ostream& operator<<(std::ostream&, const Product&);
	std::istream& operator >> (std::istream&, Product&);
	double operator+=(double&, const Product&);
	Product* CreateProduct();

}



#endif
