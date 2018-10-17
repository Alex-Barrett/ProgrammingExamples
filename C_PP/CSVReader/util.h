/*
 * util.h
 *
 *  Created on: Mar 31, 2018
 *      Author: alex
 */

#ifndef UTIL_H_
#define UTIL_H_

#include <iostream>
#include <vector>

void printByRange(std::ostream&, std::vector<std::vector<std::string> >& );
void printByLoop(std::ostream&, std::vector<std::vector<std::string> >&);
void printByIterator(std::ostream&, std::vector<std::vector<std::string> >&);
std::string& trim(std::string& , char myDelim = ' ');
void csvReader(const char*, char, std::vector<std::vector<std::string> >& );
bool checkName(const std::string&);
bool checkNum(const std::string&);




#endif /* UTIL_H_ */
