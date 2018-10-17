// Final Project Milestone 2
// Version 1.0
// Date	
// Author	
//
// Revision History
// -----------------------------------------------------------
// Name               Date                 Reason

#include <cstring>
#include "ErrorMessage.h"
#define _CRT_SECURE_NO_WARNINGS


namespace sict
{
	ErrorMessage::ErrorMessage(const char * errorMessage)
	{
		setEmpty();
		if (errorMessage != nullptr)
		{
			message(errorMessage);
		}

	}

	ErrorMessage::~ErrorMessage()
	{
		delete[] mError;
	}

	void ErrorMessage::clear()
	{
		delete[] mError;
		setEmpty();
	}

	void ErrorMessage::setEmpty()
	{
		mError = nullptr;
	}

	void ErrorMessage::message(const char* str)
	{
		if (str != nullptr)
		{
			clear();
			mError = new char[strlen(str) + 1];
			strcpy(mError, str);
		}
		
	}

	bool ErrorMessage::isClear() const
	{
		return (mError == nullptr);
	}

	const char * ErrorMessage::message() const
	{
		return mError;
	}

	std::ostream& operator<<(std::ostream& myOutput, const ErrorMessage& myMessage)
	{
		if (!myMessage.isClear())
		{
			myOutput << myMessage.message();
		}

		return myOutput;
	}
}
