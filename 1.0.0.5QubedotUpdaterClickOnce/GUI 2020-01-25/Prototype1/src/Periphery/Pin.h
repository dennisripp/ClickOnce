/*
 * Pin.h
 *
 * Created: 24.01.2018 11:02:30
 *  Author: fahrbach
 */ 


#ifndef PIN_H_
#define PIN_H_

#include "asf.h"

class Pin
{
	public:
		Pin(PORT_t *port, uint8_t pinMask);
		void setPinControl(uint8_t outputPullConfig);
		void setDirection();
		void clearDirection();
		void setOutput();
		void clearOutput();
		void setInputSenseConfig(PORT_ISC_t inputSenseConfig);
		void setInvertEnable(bool enable);
		PORT_ISC_t getInputSenseConfig();
		uint8_t getInput();
	private:
		PORT_t *port;
		uint8_t pinMask;
		register8_t *pinControl;
};

#endif /* PIN_H_ */