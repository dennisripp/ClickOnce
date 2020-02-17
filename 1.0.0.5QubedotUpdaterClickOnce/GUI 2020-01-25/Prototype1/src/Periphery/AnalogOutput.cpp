/*
 * AnalogOutput.cpp
 *
 * Created: 01.06.2018 11:47:04
 *  Author: micha
 */ 

#include "AnalogOutput.h"

 AnalogOutput::AnalogOutput(DAC *_dac, uint8_t _channel)
{
	dac = _dac;
	channel = _channel;
}

AnalogOutput::AnalogOutput(DAC *_dac, uint8_t _channel, uint8_t offsetCal, uint8_t gainCal)
{
	dac = _dac;
	channel = _channel;
	dac->setCalibration(channel, offsetCal, gainCal);
}


void AnalogOutput::setValue(uint16_t value)
{
	dac->setValue(channel, value);
}
