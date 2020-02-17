/*
 * AnalogOutput.h
 *
 * Created: 01.06.2018 11:16:47
 *  Author: micha
 */ 


#ifndef DAC_H_
#define DAC_H_

#include <dac.h>

class DAC
{
	public:
		DAC(DAC_t *_dac, DAC_REFSEL_t convRef, bool leftAdjust);
		void setValue(uint8_t channel, uint16_t value);
		void setCalibration(uint8_t channel, uint8_t offsetCal, uint8_t gainCal);
	private:
		DAC_t *dac;
};

#endif /* DAC_H_ */