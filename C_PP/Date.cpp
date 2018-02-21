
#include "Date.h"

namespace sict 
{

  int Date::mdays(int mon, int year)const 
  {
    int days[] = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31, -1 };
    int month = mon >= 1 && mon <= 12 ? mon : 13;
    month--;
    return days[month] + int((month == 1)*((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0));
   }

  void Date::setComparator()
  {
	  mComparator = mYear * 372 + mMonth * 13 + mDay;
  }

  void Date::errCode(int errorCode)
  {
	  if (errorCode >= 0 && errorCode <= 4)
	  {
		  mError = errorCode;
	  }
  }

  int Date::errCode()const
  {
	  return mError;
  }

  Date::Date()
  {
	  setEmpty();
  }

  Date::Date(int myYear, int myMonth, int myDay)
  {
	  if (myYear >= min_year && myYear <= max_year)
	  {
		  if (myMonth >= 1 && myMonth <= 12)
		  {
			  if (myDay >= 1 && myDay <= mdays(myMonth, myYear))
			  {
				  mYear = myYear;
				  mMonth = myMonth;
				  mDay = myDay;
				  mError = NO_ERROR;
				  setComparator();
			  }
			  else
			  {
				  setEmpty();
			  }
		  }
		  else
		  {
			  setEmpty();
		  }
	  }
	  else
	  {
		  setEmpty();
	  }
  }

  void Date::setEmpty()
  {
	  mError = NO_ERROR;
	  mYear = 0;
	  mMonth = 0;
	  mDay = 0;
	  mComparator = 0;
  }

  bool Date::isEmpty()const
  {
	  bool toReturn = false;

	  if (mComparator == 0)
	  {
		  toReturn = true;
	  }

	  return toReturn;
  }

  bool Date::bad()const
  {
	  bool toReturn = false;
	  if (mError != NO_ERROR)
	  {
		  toReturn = true;
	  }
	  return toReturn;
  }

  std::istream& Date::read(std::istream& istr)
  {
	  int tempYear;
	  int tempMonth;
	  int tempDay;
	  char tempChar;

	  istr >> tempYear >> tempChar >> tempMonth >> tempChar >> tempDay;
	  
	  if (!istr.fail())
	  {
		  if (tempYear >= min_year && tempYear <= max_year)
		  {
			  if (tempMonth >= 1 && tempMonth <= 12)
			  {
				  if (tempDay >= 1 && tempDay <= mdays(tempMonth, tempYear))
				  {
					  this->mYear = tempYear;
					  this->mMonth = tempMonth;
					  this->mDay = tempDay;
					  setComparator();
					  istr.clear();
				  }
				  else
				  {
					  this->mError = DAY_ERROR;
				  }
			  }
			  else
			  {
				  this->mError = MON_ERROR;
			  }
		  }
		  else
		  {
			  this->mError = YEAR_ERROR;
		  }
	  }
	  else
	  {
		  this->mError = CIN_FAILED;
	  }
	  return istr;
  }

  std::ostream& Date::write(std::ostream& ostr) const
  {

	  ostr << mYear << "/";
	  ostr.fill('0');
	  ostr.width(2);
	  ostr << mMonth;
	  ostr << "/";
	  ostr.width(2);
	  ostr << mDay;
	  ostr.fill(' ');
	  return ostr;
  }

  bool Date::operator==(const Date& rhs)const
  {
	  bool toReturn = false;
	  if (!this->isEmpty() && !rhs.isEmpty() && this->mComparator == rhs.mComparator)
	  {
		  toReturn = true;
	  }
	  return toReturn;
  }

  bool Date::operator!=(const Date& rhs)const
  {
	  return !(*this == rhs);
  }

  bool Date::operator<(const Date& rhs)const
  {
	  bool toReturn = false;
	  if (!this->isEmpty() && !rhs.isEmpty() && this->mComparator < rhs.mComparator)
	  {
		  toReturn = true;
	  }
	  return toReturn;
  }

  bool Date::operator>(const Date& rhs)const
  {
	  return (!(*this == rhs) && !(*this < rhs));
  }

  bool Date::operator<=(const Date& rhs)const
  {
	  return (*this == rhs || *this < rhs);
  }

  bool Date::operator>=(const Date& rhs)const
  {
	  return (*this == rhs || *this > rhs);
  }

  std::istream& operator >> (std::istream& myInput, Date& myDate)
  {
	  myDate.read(myInput);
	  return myInput;
  }

  std::ostream& operator << (std::ostream& myOutput, Date& myDate)
  {
	  myDate.write(myOutput);
	  return myOutput;
  }

}
