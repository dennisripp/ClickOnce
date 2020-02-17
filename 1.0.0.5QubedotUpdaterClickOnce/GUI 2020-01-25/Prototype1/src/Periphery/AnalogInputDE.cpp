/*
 * AnalogInputDE.cpp
 *
 * Created: 31.05.2018 14:33:36
 *  Author: micha
 */ 

#include "AnalogInputDE.h"

AnalogInputDE::AnalogInputDE(ADC_t *_adc, enum adcch_positive_input pinP, enum adcch_negative_input pinN, uint8_t _adc_ch)
{
	sysclk_enable_peripheral_clock(_adc);
	
	adc = _adc;
	adc_ch = _adc_ch;

	struct adc_config adc_conf;
	struct adc_channel_config adcch_conf;
	
	adc_read_configuration(adc, &adc_conf);
	adcch_read_configuration(adc, adc_ch, &adcch_conf);
	
	adc_set_conversion_parameters(&adc_conf, ADC_SIGN_ON, ADC_RES_12, ADC_REF_BANDGAP);
	adc_set_conversion_trigger(&adc_conf, ADC_TRIG_MANUAL, 1, 0);
	adc_set_clock_rate(&adc_conf, 2000000UL);

	adcch_set_input(&adcch_conf, pinP, pinN, 1);

	adc_write_configuration(adc, &adc_conf);
	adcch_write_configuration(adc, adc_ch, &adcch_conf);
	
	adc_enable(adc);
}

int16_t AnalogInputDE::getValue()
{
	adc_start_conversion(adc, adc_ch);
	adc_wait_for_interrupt_flag(adc, adc_ch);
	return adc_get_result(adc, adc_ch);
}

int32_t AnalogInputDE::getOversampledValue(uint16_t oversampleCount)
{
	int32_t value = 0;
	for (uint16_t i = 0; i < oversampleCount; i++)
	{
		value += getValue();
	}
	return value;
}