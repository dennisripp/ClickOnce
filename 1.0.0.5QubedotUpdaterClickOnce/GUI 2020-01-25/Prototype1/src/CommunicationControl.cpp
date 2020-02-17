/* 
* CommunicationControl.cpp
*
* Created: 26.09.2018 21:34:51
* Author: Maxi
*/


#include "CommunicationControl.h"

// default constructor
CommunicationControl::CommunicationControl(usb* _usb_c, CommandDecoder* _commandDecoder, SystemState* _systemState, PixelArray* _pixelArray)
{
	pixelArray = _pixelArray;
	systemState = _systemState;
	usb_c = _usb_c;
	commandDecoder = _commandDecoder;
	Data = new uint32_t;
}

// default destructor
CommunicationControl::~CommunicationControl()
{
}

void CommunicationControl::USB_Communication()
{
	bool isCommand;
	uint8_t* Instruktion;

	usb_c->receive(Instruktion);
	isCommand = commandDecoder->isCommand(Instruktion);
	if(isCommand)
	{
		uint8_t command = commandDecoder->Decode4ByteMessage(Instruktion);
		systemState->RecieveNewCommand(command);
		ExecuteTask();
	}
}

void CommunicationControl::ExecuteTask(){
	switch(systemState->GetCurrentState()){
		case SystemState::ST_DSP: DSP(); break;
		case SystemState::ST_DSF: DSF(); break;
		case SystemState::ST_SetVoltageMax: SetVoltageMax(); break;
		case SystemState::ST_SetCurrentMax: SetCurrentMax(); break;
		case SystemState::ST_ANIMATION_INIT: break;
		case SystemState::ST_LOAD_PRESET: break;
		case SystemState::ST_SELECT_ANIMATION: break;
		case SystemState::ST_ANIMATION_START: break;
		default: break;
	}
}

void CommunicationControl::DSP()
{
	uint8_t* Data_tmp = new uint8_t;
	usb_c->receive(Data_tmp);
	bool on = (*Data_tmp >> 0) & 0x01;
	uint8_t col = (*Data_tmp >> 1) & 0x07;
	uint8_t row = (*Data_tmp >> 4) & 0x07;
	pixelArray->print_pixel(col, row, on);
}

void CommunicationControl::DSF()
{
	uint64_t* Data_tmp = new uint64_t;
	usb_c->receive(Data_tmp);
	pixelArray->PrintFrame(*Data_tmp);
}

void CommunicationControl::SetVoltageMax(){	
	uint16_t* Data_tmp = new uint16_t;
	usb_c->receive(Data_tmp);
	SystemValues* tmp_values = systemState->GetSystemValues();
	tmp_values->setLedVoltage(*Data_tmp);
}

void CommunicationControl::SetCurrentMax(){
	uint16_t* Data_tmp = new uint16_t;
	usb_c->receive(Data_tmp);
	SystemValues* tmp_values = systemState->GetSystemValues();
	tmp_values->setLedCurrentLimit(*Data_tmp);
}