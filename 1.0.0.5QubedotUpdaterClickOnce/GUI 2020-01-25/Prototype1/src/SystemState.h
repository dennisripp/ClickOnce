/* 
* SystemState.h
*
* Created: 24.09.2018 16:40:26
* Author: Maxi
*/


#ifndef __SYSTEMSTATE_H__
#define __SYSTEMSTATE_H__

#include "PixelArray.h"
#include "SystemValues.h"

class SystemState
{
//variables
public:	
	enum Command : uint8_t {
		DIRECTSETPIXEL = 0,
		DIRECTSETFRAME = 1,
		SETVOLTAGEMAX = 2,
		SETCURRENTMAX = 3,
		ANIMATION = 4,
		RETURN = 5,
		LOADFRAMES = 6,
		SELECTANIMATION = 7,
		START = 8,
		STAY		
	};
	
	enum Communication_States : uint8_t {
		ST_IDLE = 0,
		ST_DSP,		// direct set pixel
		ST_DSF,		// direct set frame
		ST_SetVoltageMax,
		ST_SetCurrentMax,
		ST_ANIMATION_INIT,
		ST_LOAD_PRESET,
		ST_SELECT_ANIMATION,
		ST_ANIMATION_START,
		ST_MAX_STATES
	};
protected:
private:
	uint32_t Data;
	uint16_t Timer;
	uint16_t CurrentMax;
	uint16_t VoltageMax;
	uint8_t command;
	uint8_t CurrentState;
	uint8_t NextState;

	PixelArray* pixelArray;
	SystemValues* systemValues;
		
	enum Transition : uint16_t {
		T_DSP = ST_IDLE << 8 | DIRECTSETPIXEL,
		T_DSF = ST_IDLE << 8 | DIRECTSETFRAME,
		T_SetVoltageMax = ST_IDLE << 8 | SETVOLTAGEMAX,
		T_SetCurrentMax = ST_IDLE << 8 | SETCURRENTMAX,
		T_AnimationInit = ST_IDLE << 8 | ANIMATION,
		T_LoadFrames = ST_ANIMATION_INIT << 8 | LOADFRAMES,
		T_SelectAnimation = ST_ANIMATION_INIT << 8 | SELECTANIMATION,
		T_AnimationStart1 = ST_SELECT_ANIMATION << 8 | START,
		T_AnimationStart2 = ST_LOAD_PRESET << 8 | START
	};

//functions
public:
	SystemState(PixelArray* _pixelArray, SystemValues* _systemValues);
	~SystemState();
	void RecieveNewCommand(uint8_t _command);
	void ExecuteTask();
	void WriteData(uint32_t* Data_tmp);
	uint8_t GetCurrentState();
	SystemValues* GetSystemValues();
	
protected:
private:
	void GetNextState();
	void SetNextState();
	SystemState( const SystemState &c );
	SystemState& operator=( const SystemState &c );

};

#endif //__SYSTEMSTATE_H__
