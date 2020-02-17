/* 
* SystemValues.cpp
*
* Created: 01.10.2018 10:24:28
* Author: Maxi
*/


#include "SystemValues.h"

// default constructor
SystemValues::SystemValues(uint16_t _CurrentMaxDefault, uint16_t _VoltageMaxDefault, AnalogOutput* _ledCurrent, AnalogOutput* _ledVoltage)
{
	CurrentMax = _CurrentMaxDefault;
	VoltageMax = _VoltageMaxDefault;
	ledCurrent = _ledCurrent;
	ledVoltage = _ledVoltage;
	setLedVoltage(_VoltageMaxDefault);
	setLedCurrentLimit(_CurrentMaxDefault);
} //SystemValues

// default destructor
SystemValues::~SystemValues()
{
} //~SystemValues

void SystemValues::setLedVoltage(uint16_t voltage)
{
	// voltage given with 1 mV increment
	uint32_t value = ((uint32_t)voltage * 4095) / 11000;
	ledVoltage->setValue((uint16_t)value);
}

void SystemValues::setLedCurrentLimit(uint16_t current)
{
	// current given with 10 µA increment
	uint32_t value = ((uint32_t)current * 4095) / 50000;
	ledCurrent->setValue((uint16_t)value);
}