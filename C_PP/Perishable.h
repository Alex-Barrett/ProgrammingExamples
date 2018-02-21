#ifndef SICT_PERISHABLE_H
#define SICT_PERISHABLE_H

#include <iostream>
#include "NonPerishable.h"
#include "Date.h"

namespace sict
{
	class Perishable : public NonPerishable
	{
		Date mExpiry;

	protected:
		void setEmpty(); //set to empty safe state

	public:
		Perishable(); //default, pass P as type
		std::fstream& store(std::fstream&, bool = true)const; //store info in file, include expiry date
		std::fstream& load(std::fstream&); //read info from file
		std::ostream& write(std::ostream&, bool)const; //write info to screen
		std::istream& read(std::istream&); //read info from screen
		const Date& expiry()const; //return expiry date

	};

	Product* CreatePerishable();

}




#endif 
