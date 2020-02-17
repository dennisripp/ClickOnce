/* 
* CommandDecoder.h
*
* Created: 17.09.2018 17:01:42
* Author: Maxi
*/


#ifndef __COMMANDDECODER_H__
#define __COMMANDDECODER_H__

#include "PixelArray.h"
#include "SystemState.h"
#include "usb.h"


class CommandDecoder
{
//variables
public:		
protected:
private:

//functions
public:
	uint8_t Decode4ByteMessage(uint8_t* _Data);
	bool isCommand(uint8_t* _Data);
	CommandDecoder();
	~CommandDecoder();
protected:
private:
	enum Command : uint8_t {
		DIRECTSETPIXEL,
		DIRECTSETFRAME,
		SETVOLTAGEMAX,
		SETCURRENTMAX,
		ANIMATION,
		RETURN,
		LOADFRAMES,
		SELECTANIMATION,
		START,
		STAY,
		MaxCommandCount
	};

}; //CommandDecoder

#endif //__COMMANDDECODER_H__
