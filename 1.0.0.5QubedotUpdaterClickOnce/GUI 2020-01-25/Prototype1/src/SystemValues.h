/* 
* SystemValues.h
*
* Created: 01.10.2018 10:24:28
* Author: Maxi
*/


#ifndef __SYSTEMVALUES_H__
#define __SYSTEMVALUES_H__

#include "Periphery/AnalogOutput.h"


class SystemValues
{
//variables
public:
protected:
private:
	uint16_t VoltageMax;
	uint16_t CurrentMax;
	
	AnalogOutput* ledCurrent;
	AnalogOutput* ledVoltage;


//functions
public:
	void setLedVoltage(uint16_t voltage);
	void setLedCurrentLimit(uint16_t current);
	SystemValues(uint16_t _CurrentMaxDefault, uint16_t _VoltageMaxDefault, AnalogOutput* _ledCurrent, AnalogOutput* _ledVoltage);
	~SystemValues();
protected:
private:
	SystemValues( const SystemValues &c );
	SystemValues& operator=( const SystemValues &c );

}; //SystemValues

#endif //__SYSTEMVALUES_H__
