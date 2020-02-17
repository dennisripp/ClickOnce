/* 
* SystemState.cpp
*
* Created: 24.09.2018 16:40:26
* Author: Maxi
*/


#include "SystemState.h"

// default constructor
SystemState::SystemState(PixelArray* _pixelArray, SystemValues* _systemValues)
{
	pixelArray = _pixelArray;
	systemValues = _systemValues;
	
	CurrentState = 0;
	NextState = 0;	
}

// default destructor
SystemState::~SystemState()
{
}

void SystemState::RecieveNewCommand(uint8_t _command){
	command = _command;
	GetNextState();
}

void SystemState::GetNextState(){
	if(command == RETURN){
		NextState = ST_IDLE;
	} else
	if(command == STAY)
	{
		NextState = CurrentState;
	} else {
		switch(CurrentState << 8 | command){
			case T_DSP: NextState = ST_DSP; break;
			case T_DSF: NextState = ST_DSF; break;
			case T_SetVoltageMax: NextState = ST_SetVoltageMax; break;
			case T_SetCurrentMax: NextState = ST_SetCurrentMax; break;
			case T_AnimationInit: NextState = ST_ANIMATION_INIT; break;
			case T_LoadFrames: NextState = ST_LOAD_PRESET; break;
			case T_SelectAnimation: NextState = ST_SELECT_ANIMATION; break;
			case T_AnimationStart1: NextState = ST_ANIMATION_START; break;
			case T_AnimationStart2: NextState = ST_ANIMATION_START; break;
			default: NextState = CurrentState; break;
		}
	}
	command = 0;
	SetNextState();
}
	
void SystemState::SetNextState(){
	CurrentState = NextState;
}

uint8_t SystemState::GetCurrentState(){
	return CurrentState;
}

void SystemState::WriteData(uint32_t* Data_tmp){
	Data = *Data_tmp;
}

SystemValues* SystemState::GetSystemValues(){
	return systemValues;
}