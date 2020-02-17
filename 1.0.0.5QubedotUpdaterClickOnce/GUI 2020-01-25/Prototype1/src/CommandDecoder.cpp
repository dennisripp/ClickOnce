/* 
* CommandDecoder.cpp
*
* Created: 17.09.2018 17:01:42
* Author: Maxi
*/

#include "CommandDecoder.h"

// constructor
CommandDecoder::CommandDecoder()
{
}


//_Data == Commands falls FFFFFFXX
uint8_t CommandDecoder::Decode4ByteMessage(uint8_t* _Data)
{
	return *_Data;
}

bool CommandDecoder::isCommand(uint8_t* _Data)
{
	if (*_Data < MaxCommandCount)
		return true;
	else
		return false;
}

// default destructor
CommandDecoder::~CommandDecoder()
{

}

