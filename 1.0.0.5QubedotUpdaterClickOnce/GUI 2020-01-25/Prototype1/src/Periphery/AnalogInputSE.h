/*
 * AnalogInput.h
 *
 * Created: 12.06.2017 15:05:00
 *  Author: student
 */ 


#ifndef ANALOGINPUTSE_H_
#define ANALOGINPUTSE_H_

#include <sysclk.h>
#include "adc.h"
#include "AnalogInput.h"

class AnalogInputSE :public AnalogInput
{
	public:
		AnalogInputSE(ADC_t *_adc, enum adcch_positive_input pin, uint8_t _adc_ch);
		void calibrate(uint16_t oversampleCount);
		void setCalibration(uint32_t _calibration);
		uint32_t getCalibration();
		int32_t getOversampledValue(uint16_t oversampleCount);
	private:
		int16_t getValue();
		ADC_t *adc;
		uint8_t adc_ch;
		int32_t calibration;
};

#endif /* ANALOGINPUTSE_H_ */