// Final Project Milestone 1
//
// Version 1.0
// Date
// Author
// Description
//
//
//
//
// Revision History
// -----------------------------------------------------------
// Name               Date                 Reason
/////////////////////////////////////////////////////////////////
#include <iostream>
#include <fstream>

#ifndef SICT_DATE_H
#define SICT_DATE_H

#define NO_ERROR 0
#define CIN_FAILED 1
#define YEAR_ERROR 2
#define MON_ERROR 3
#define DAY_ERROR 4

namespace sict 
{
	const int min_year = 2000;
	const int max_year = 2030;

  class Date 
  {
	  

	  int mYear;
	  int mMonth;
	  int mDay;
	  int mComparator;
	  int mError;

      int mdays(int, int)const;
	  void setComparator();
	  void errCode(int errorCode);
	  bool isEmpty()const;
 
  public:
	  Date();
	  Date(int myYear, int myMonth, int myDay);
	  int errCode()const;
	  bool bad()const;
	  void setEmpty();

	  std::istream& read(std::istream& istr);
	  std::ostream& write(std::ostream& ostr) const;
	  

	  bool operator==(const Date& rhs)const;
	  bool operator!=(const Date& rhs)const;
	  bool operator<(const Date& rhs)const;
	  bool operator>(const Date& rhs)const;
	  bool operator<=(const Date& rhs)const;
	  bool operator>=(const Date& rhs)const;
 
  };

  std::istream& operator >> (std::istream& myInput, Date& myDate);
  std::ostream& operator << (std::ostream& myOutput, Date& myDate);
  



}
#endif