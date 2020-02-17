/*
 * AnalogInput.h
 *
 * Created: 31.05.2018 15:03:45
 *  Author: micha
 */ 


#ifndef ANALOGINPUT_H_
#define ANALOGINPUT_H_

class AnalogInput
{
	public:
		virtual int32_t getOversampledValue(uint16_t oversampleCount) = 0;
};



#endif /* ANALOGINPUT_H_ */