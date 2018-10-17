#include <cstring>
#include<string>
#include <fstream>
#include <iomanip>
#include "NonPerishable.h"

using namespace std;

namespace sict
{
	
	NonPerishable::NonPerishable(char myType) // only set type
	{
		setEmpty();
		mType = myType;

	}

	NonPerishable::NonPerishable(const char* mySKU, const char* myName, const char* myUnit, int myQty, bool myTaxable, double myPrice, int myQtyNeeded) //set everything, set type to default
	{
		setEmpty();
		if (mySKU != nullptr && myName != nullptr && myUnit != nullptr)
		{
			strcpy(mSKU, mySKU);
			name(myName);
			strcpy(mUnit, myUnit);
			mQuantity = myQty;
			mTaxable = myTaxable;
			mPrice = myPrice;
			mQtyNeeded = myQtyNeeded;
		}
	}

	NonPerishable::NonPerishable(const NonPerishable& myCopy) //copy constructor
	{
		if (this != &myCopy)
		{
			setEmpty();
			*this = myCopy;
		}
		else
		{
			setEmpty();
		}
	}

	NonPerishable::~NonPerishable()
	{
		delete[] mName;
	}

	double NonPerishable::total_cost() const //return cost of item * quantity in stock
	{
		return cost() * mQuantity;
	}

	void NonPerishable::quantity(int myQuantity) // update quantity
	{
		mQuantity = myQuantity;
	}

	bool NonPerishable::isEmpty() const //check if current is in safe empty state
	{
		return (mName == nullptr);
	}

	int NonPerishable::qtyNeeded() const //return min quantity
	{
		return mQtyNeeded;
	}

	int NonPerishable::quantity() const //return current in stock
	{
		return mQuantity;
	}

	std::fstream& NonPerishable::store(std::fstream& myFile, bool myLine) const //Push all items to provided file
	{
		myFile << mType << "," << mSKU << "," << mName << "," << mPrice << "," << mTaxable << "," << mQuantity << "," << mUnit << "," << mQtyNeeded;
		if (myLine)
		{
			myFile << endl;
		}
		return myFile;
	}

	std::fstream& NonPerishable::load(std::fstream& myFile) //read items from provided file
	{
		NonPerishable temp;
		string tempName;

		myFile.getline(temp.mSKU, max_sku_length, ',');
		getline(myFile, tempName, ',');
		temp.name(tempName.c_str()); 
		myFile >> temp.mPrice;
		myFile.ignore();
		myFile >> temp.mTaxable;
		myFile.ignore();
		myFile >> temp.mQuantity;
		myFile.ignore();
		myFile.getline(temp.mUnit, max_unit_length, ',');
		myFile >> temp.mQtyNeeded;
		

		*this = temp;

		return myFile;
	}

	std::ostream& NonPerishable::write(std::ostream& myOutput, bool myLine) const //write item to the screen, if myLine - display linear format, else - display on each line
	{
		if (this->isClear())
		{
			if (myLine)
			{
				
				myOutput.setf(ios::left);
				myOutput.precision(2);
				myOutput.setf(ios::fixed);

				myOutput << setw(max_sku_length) << this->mSKU << '|';
				myOutput.width(20);
				myOutput << this->mName << '|';
				myOutput.width(7);
				myOutput.unsetf(ios::left);
				myOutput << this->cost() << '|';
				myOutput.width(4);
				myOutput << this->mQuantity << '|';
				myOutput.setf(ios::left);
				myOutput.width(10);
				myOutput << this->mUnit << '|';
				myOutput.unsetf(ios::left);
				myOutput.width(4);
				myOutput << this->mQtyNeeded;
				myOutput << '|';
			}
			else
			{
				myOutput << "Sku: " << this->mSKU << endl;
				myOutput << "Name: " << this->mName << endl;
				myOutput << "Price: " << this->mPrice << endl;
				if (this->mTaxable)
				{
					myOutput << "Price after tax: " << this->cost() << endl;
				}
				else
				{
					myOutput << "Price after tax: N/A" << endl;
				}
				myOutput << "Quantity On Hand: " << this->mQuantity << " " << this->mUnit << endl;
				myOutput << "Quantity Needed: " << this->mQtyNeeded;
			}
		}
		else
		{

			myOutput << mState;
		}
		return myOutput;
	}

	std::istream& NonPerishable::read(std::istream& myInput) //read input from user for new item entry
	{
		NonPerishable temp;
		char tempName[max_name_length + 1];
		char tempTax = 'n';


		cout << " Sku: ";
		myInput.getline(temp.mSKU, max_sku_length + 1);

		if(!myInput.fail())
		{
			cout << " Name: ";
			myInput.getline(tempName, max_name_length + 1);
			temp.name(tempName);

			if (!myInput.fail())
			{
				cout << " Unit: ";
				myInput.getline(temp.mUnit, max_unit_length + 1);

				if (!myInput.fail())
				{
					cout << " Taxed? (y/n): ";
					myInput >> tempTax;
					
					if (tempTax == 'Y' || tempTax == 'y')
					{
						temp.mTaxable = true;
					}
					else if (tempTax == 'N' || tempTax == 'n')
					{
						temp.mTaxable = false;
					}
					else
					{
						setEmpty();
						mState.message("Only (Y)es or (N)o are acceptable");
						myInput.setstate((std::ios::failbit));
					}

					if (!myInput.fail())
					{
						cout << " Price: ";
						myInput >> temp.mPrice;

						if (!myInput.fail())
						{
							cout << "Quantity On hand: ";
							myInput >> temp.mQuantity;
							if (!myInput.fail())
							{
								cout << "Quantity Needed: ";
								myInput >> temp.mQtyNeeded;
								if (myInput.fail())
								{
									setEmpty();
									mState.message("Invalid Quantity Needed Entry");
								}
								else
								{
									temp.mType = this->mType;
									*this = temp;
									this->mState.clear();
								}
							}
							else
							{
								setEmpty();
								mState.message("Invalid Quantity Entry");
							}
						}
						else
						{
							setEmpty();
							mState.message("Invalid Price Entry");
						}
					}
					
				}
			}
		}

		return myInput;
	}

	void NonPerishable::name(const char* myName) //update name
	{
		if (myName != nullptr)
		{
			delete[] mName;
			mName = new char[strlen(myName) + 1];
			strcpy(mName, myName);
		}
		else
		{
			delete[] mName;// consider setting to null
			mName = nullptr;
		}
	}

	const char * NonPerishable::name() const // return name
	{
		if (mName != nullptr && strlen(mName) > 0)
		{
			return mName;
		}
		else
		{
			return nullptr;
		}
	}

	double NonPerishable::cost() const //return cost, apply tax if applicable
	{
		double toReturn = mPrice;
		if (mTaxable)
		{
			toReturn += mPrice * ((double)TAXRATE / 100);
		}
		return toReturn;
	}

	void NonPerishable::message(const char * myMessage) //update the error message of this product
	{
		if (myMessage != nullptr)
		{
			mState.message(myMessage);
		}
	}

	bool NonPerishable::isClear() const //check if in error state
	{
		return mState.isClear();
	}

	void NonPerishable::setEmpty() //set to safe empty state
	{
		mType = 'N';
		strcpy(mSKU, "");
		mName = nullptr;
		strcpy(mUnit, "");
		mQuantity = 0;
		mQtyNeeded = 0;
		mPrice = 0;
		mTaxable = true;
		mState.clear();
		
	}

	NonPerishable& NonPerishable::operator=(const NonPerishable& myCopy)
	{
		if (this != &myCopy)
		{
			mType = myCopy.mType;
			strcpy(mSKU, myCopy.mSKU);
			name(myCopy.mName);
			strcpy(mUnit, myCopy.mUnit);
			mQuantity = myCopy.mQuantity;
			mQtyNeeded = myCopy.mQtyNeeded;
			mPrice = myCopy.mPrice;
			mTaxable = myCopy.mTaxable;
			mState.message(myCopy.mState.message());
		}
		return *this;
	}

	bool NonPerishable::operator==(const char* mySKU) const
	{
		
		return (!strcmp(mSKU, mySKU));
	}

	bool NonPerishable::operator>(const char* mySKU) const
	{
		return (strcmp(mSKU, mySKU) < 0);
	}

	int NonPerishable::operator+=(int myUnits)
	{
		if (myUnits > 0)
		{
			mQuantity += myUnits;
		}
		return mQuantity;
	}

	bool NonPerishable::operator>(const Product& myProduct) const
	{
		return (strcmp(mName, myProduct.name()) < 0);
	}

	std::ostream& operator<<(std::ostream& myOutput, const Product& myProduct)
	{
		myProduct.write(myOutput, true);
		return myOutput;
	}

	std::istream& operator>>(std::istream& myInput, Product& myProduct)
	{
		myProduct.read(myInput);
		return myInput;
	}

	double operator+=(double& myCost, const Product& myProduct)
	{
		myCost += myProduct.total_cost();
		return myCost;
	}

	Product * CreateProduct()
	{
		NonPerishable* temp = nullptr;
		temp = new NonPerishable;
		return temp;
	}

}