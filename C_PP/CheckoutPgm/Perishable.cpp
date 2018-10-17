#include "Perishable.h"

using namespace std;

namespace sict
{



	Perishable::Perishable() : NonPerishable('P') //call base constructor, pass P as type
	{
		setEmpty();
	}
	
	void Perishable::setEmpty() //set object to empty state
	{
		mExpiry.setEmpty();
	}

	std::fstream& Perishable::store(std::fstream& myFile, bool myLine)const //write product info to provided file, include expiry date
	{
		NonPerishable::store(myFile, false);
		myFile << ",";
		mExpiry.write(myFile);
		if(myLine == true)
		{
			myFile << endl;
			
		}
		
		return myFile;
	}

	std::fstream& Perishable::load(std::fstream& myFile) //load information about product from provided file
	{
		NonPerishable::load(myFile);
		myFile.ignore();
		mExpiry.read(myFile);
		myFile.get();

		return myFile;
	}

	std::ostream& Perishable::write(std::ostream& myOutput, bool myLine) const //write product info to screen
	{
		if (!this->isEmpty())
		{
			NonPerishable::write(myOutput, myLine);

			if (myLine)
			{
				if (isClear())
				{
					mExpiry.write(myOutput);
				}
			}
			else
			{
				if (isClear())
				{
					myOutput << endl << "Expiry date: ";
					mExpiry.write(myOutput);
				}
			}
		}
		return myOutput;
	}

	std::istream& Perishable::read(std::istream& myInput) //read product info from user input
	{
		Date tempDate;
		NonPerishable::read(myInput);
		if (isClear())
		{
			cout << "Expiry date (YYYY/MM/DD): ";
			tempDate.read(myInput);
		}
		else
		{
			myInput.setstate(std::ios::failbit);
		}

		if (tempDate.bad())
		{
			myInput.setstate(std::ios::failbit);

			switch (tempDate.errCode())
			{
			case CIN_FAILED:
				this->message("Invalid Date Entry");
				break;
			case YEAR_ERROR:
				this->message("Invalid Year in Date Entry");
				break;
			case MON_ERROR:
				this->message("Invalid Month in Date Entry");
				break;
			case DAY_ERROR:
				this->message("Invalid Day in Date Entry");
				break;
			default:
				break;
			}
		}
		else
		{
			mExpiry = tempDate;
			
		}
		return myInput;
	}

	const Date& Perishable::expiry() const //return expiry date
	{
		return mExpiry;
	}

	Product* CreatePerishable()
	{
		Perishable* temp = nullptr;
		temp = new Perishable;
		return temp;
	}


}