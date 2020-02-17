/*
 * Pin.cpp
 *
 * Created: 24.01.2018 11:15:06
 *  Author: fahrbach
 */ 

#include "Pin.h"

 Pin::Pin(PORT_t *port, uint8_t pinMask)
{
	this->port = port;
	this->pinMask = pinMask;
	
	switch (pinMask)
	{
		case PIN0_bm:
			pinControl = &port->PIN0CTRL;
			break;
		case PIN1_bm:
			pinControl = &port->PIN1CTRL;
			break;
		case PIN2_bm:
			pinControl = &port->PIN2CTRL;
			break;
		case PIN3_bm:
			pinControl = &port->PIN3CTRL;
			break;
		case PIN4_bm:
			pinControl = &port->PIN4CTRL;
			break;
		case PIN5_bm:
			pinControl = &port->PIN5CTRL;
			break;
		case PIN6_bm:
			pinControl = &port->PIN6CTRL;
			break;
		case PIN7_bm:
			pinControl = &port->PIN7CTRL;
			break;
	}
}

void Pin::setPinControl(uint8_t outputPullConfig)
{
	*pinControl = outputPullConfig;
}

void Pin::setDirection()
{
	port->DIRSET = pinMask;
}

void Pin::clearDirection()
{
	port->DIRCLR = pinMask;
}

void Pin::setOutput()
{
	port->OUTSET = pinMask;
}

void Pin::clearOutput()
{
	port->OUTCLR = pinMask;
}

void Pin::setInputSenseConfig(PORT_ISC_t inputSenseConfig)
{
	*pinControl = (*pinControl & ~PORT_ISC_gm) | inputSenseConfig;
}

void Pin::setInvertEnable(bool enable)
{
	*pinControl = (*pinControl & ~PORT_INVEN_bm) | (enable ? PORT_INVEN_bm : 0);
}

PORT_ISC_t Pin::getInputSenseConfig()
{
	return (PORT_ISC_t)(*pinControl & PORT_ISC_gm);
}

uint8_t Pin::getInput()
{
	return (port->IN & pinMask) > 0;
}
