/*
 * AnalogInput.cpp
 *
 * Created: 12.06.2017 15:09:34
 *  Author: student
 */ 

#include "AnalogInputSE.h"

AnalogInputSE::AnalogInputSE(ADC_t *_adc, enum adcch_positive_input pin, uint8_t _adc_ch)
{
	sysclk_enable_peripheral_clock(_adc);
	
	adc = _adc;
	adc_ch = _adc_ch;
	calibration = 0;

	struct adc_config adc_conf;
	struct adc_channel_config adcch_conf;
	
	adc_read_configuration(adc, &adc_conf);
	adcch_read_configuration(adc, adc_ch, &adcch_conf);
	
	adc_set_conversion_parameters(&adc_conf, ADC_SIGN_OFF, ADC_RES_12, ADC_REF_VCC);
	adc_set_conversion_trigger(&adc_conf, ADC_TRIG_MANUAL, 1, 0);
	adc_set_clock_rate(&adc_conf, 2000000UL);

	adcch_set_input(&adcch_conf, pin, ADCCH_NEG_NONE, 1);

	adc_write_configuration(adc, &adc_conf);
	adcch_write_configuration(adc, adc_ch, &adcch_conf);
	
	adc_enable(adc);
}

void AnalogInputSE::calibrate(uint16_t oversampleCount)
{
	// calibrate 0 V
	calibration = 0;
	calibration = getOversampledValue(oversampleCount) * (1000000 / oversampleCount);
}

void AnalogInputSE::setCalibration(uint32_t _calibration)
{
	calibration = _calibration;
}

uint32_t AnalogInputSE::getCalibration()
{
	return calibration;
}

int16_t AnalogInputSE::getValue()
{
	adc_start_conversion(adc, adc_ch);
	adc_wait_for_interrupt_flag(adc, adc_ch);
	return adc_get_result(adc, adc_ch);
}

int32_t AnalogInputSE::getOversampledValue(uint16_t oversampleCount)
{
	int32_t value = 0;
	for (uint16_t i = 0; i < oversampleCount; i++)
	{
		value += getValue();
	}
	value -= (uint64_t)calibration * (uint64_t)oversampleCount / (uint64_t)1000000;
	return value;
}