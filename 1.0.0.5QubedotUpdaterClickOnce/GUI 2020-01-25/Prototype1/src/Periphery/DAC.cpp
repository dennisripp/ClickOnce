/*
 * AnalogOutput.cpp
 *
 * Created: 01.06.2018 11:18:40
 *  Author: micha
 */ 

#include "DAC.h"

 DAC::DAC(DAC_t *_dac, DAC_REFSEL_t convRef, bool leftAdjust)
{
	dac = _dac;
	
	dac_enable(dac);
	
	dac->CTRLB = ( dac->CTRLB & ~DAC_CHSEL_gm ) | DAC_CHSEL_DUAL_gc;
	dac->CTRLC = ( dac->CTRLC & ~( DAC_REFSEL_gm | DAC_LEFTADJ_bm ) ) | convRef | ( leftAdjust ? DAC_LEFTADJ_bm : 0x00 );
	dac->CTRLA |= DAC_CH1EN_bm | DAC_CH0EN_bm | DAC_ENABLE_bm;
	
	setValue(0, 0);
	setValue(1, 0);
}

void DAC::setValue(uint8_t channel, uint16_t value)
{
	if (channel == 0)
	{
		dac->CH0DATA = value;
	}
	else
	{
		dac->CH1DATA = value;
	}
}

void DAC::setCalibration(uint8_t channel, uint8_t offsetCal, uint8_t gainCal)
{
	if (channel == 0)
	{
		dac->CH0OFFSETCAL = offsetCal;
		dac->CH0GAINCAL = gainCal;
	}
	else
	{
		dac->CH1OFFSETCAL = offsetCal;
		dac->CH1GAINCAL = gainCal;
	}
}
