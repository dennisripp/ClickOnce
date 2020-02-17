/*
 * AnalogInputDifferentialEnded.h
 *
 * Created: 31.05.2018 14:23:16
 *  Author: micha
 */ 


#ifndef ANALOGINPUTDE_H_
#define ANALOGINPUTDE_H_

#include <sysclk.h>
#include "adc.h"
#include "AnalogInput.h"

class AnalogInputDE :public AnalogInput
{
	public:
		AnalogInputDE(ADC_t *_adc, enum adcch_positive_input pinP, enum adcch_negative_input pinN, uint8_t _adc_ch);
		int32_t getOversampledValue(uint16_t oversampleCount);
	private:
		int16_t getValue();
		ADC_t *adc;
		uint8_t adc_ch;
};

#endif /* ANALOGINPUTDE_H_ */