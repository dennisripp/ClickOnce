/* 
* CommunicationControl.h
*
* Created: 26.09.2018 21:34:51
* Author: Maxi
*/


#ifndef __COMMUNICATIONCONTROL_H__
#define __COMMUNICATIONCONTROL_H__

#include "usb.h"
#include "CommandDecoder.h"
#include "SystemState.h"

class CommunicationControl
{
//variables
public:
protected:
private:
	uint32_t* Data;
	usb* usb_c;
	CommandDecoder* commandDecoder;
	SystemState* systemState;
	PixelArray* pixelArray;

//functions
public:
	CommunicationControl(usb* _usb_c, CommandDecoder* _commandDecoder, SystemState* _systemState, PixelArray* _pixelArray);
	~CommunicationControl();
	void USB_Communication();
protected:
private:
	CommunicationControl( const CommunicationControl &c );
	CommunicationControl& operator=( const CommunicationControl &c );
	void ExecuteTask();
	void DSP();
	void DSF();
	void SetVoltageMax();
	void SetCurrentMax();

}; //CommunicationControl

#endif //__COMMUNICATIONCONTROL_H__
