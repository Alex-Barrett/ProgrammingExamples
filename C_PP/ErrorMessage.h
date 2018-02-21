// Final Project Milestone 2
// Version 1.0
// Date	
// Author	
//
//
// Revision History
// -----------------------------------------------------------
// Name               Date                 Reason

#include <iostream>
#ifndef SICT_ERRORMESSAGE_H
#define SICT_ERRORMESSAGE_H

namespace sict
{
	class ErrorMessage
	{
		char* mError;

		void setEmpty();

	public:
		explicit ErrorMessage(const char* errorMessage = nullptr);
		ErrorMessage(const ErrorMessage& em) = delete;
		virtual ~ErrorMessage();

		void clear();
		void message(const char* str);
		bool isClear() const;
		const char* message()const;

		ErrorMessage& operator=(const ErrorMessage& em) = delete;
		
	};

	std::ostream& operator<<(std::ostream& myOutput, const ErrorMessage& myMessage);
	
}
#endif