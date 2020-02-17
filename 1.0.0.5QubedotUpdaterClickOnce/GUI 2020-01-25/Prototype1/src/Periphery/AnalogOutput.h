/*
 * AnalogOutput.h
 *
 * Created: 01.06.2018 11:43:55
 *  Author: micha
 */ 


#ifndef ANALOGOUTPUT_H_
#define ANALOGOUTPUT_H_

#include "DAC.h"

class AnalogOutput
{
	public:
		AnalogOutput(DAC *_dac, uint8_t _channel);
		AnalogOutput(DAC *_dac, uint8_t _channel, uint8_t offsetCal, uint8_t gainCal);
		void setValue(uint16_t value);
	private:
		DAC *dac;
		uint8_t channel;
};

#endif /* ANALOGOUTPUT_H_ */