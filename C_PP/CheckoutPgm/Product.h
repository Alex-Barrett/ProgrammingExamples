#ifndef SICT_PRODUCT_H //Header guard
#define SICT_PRODUCT_H


#include<iostream>
#include <fstream>

namespace sict
{
	class Product //base abstract class
	{
	public:
		virtual std::fstream& store(std::fstream&, bool = true)const = 0; //store items in file
		virtual std::fstream& load(std::fstream& file) = 0; //load items from file
		virtual std::ostream& write(std::ostream& os, bool linear) const = 0; //write items to screen
		virtual std::istream& read(std::istream& is) = 0; //read items from screen
		virtual bool operator==(const char*) const = 0 ;
		virtual double total_cost() const = 0; //total cost with/without tax
		virtual const char* name() const = 0; //return name
		virtual void quantity(int) = 0; //update quantity
		virtual int qtyNeeded() const = 0; //minimum quantity
		virtual int quantity() const = 0; //return quantity
		virtual int operator+=(int) = 0;
		virtual bool operator>(const Product&) const = 0;

	};

	std::ostream& operator<<(std::ostream&, const Product&);
	std::istream& operator >> (std::istream&, Product&);
	double operator+=(double&, const Product&);
	Product* CreateProduct();
	Product* CreatePerishable();
}
#endif
