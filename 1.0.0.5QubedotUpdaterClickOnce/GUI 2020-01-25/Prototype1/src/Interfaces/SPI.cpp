/*
 * SPI_Interface.cpp
 *
 * Created: 23.03.2018 14:53:47
 *  Author: fahrbach
 */ 

#include "Interfaces/SPI.h"

 SPI::SPI(SpiHal *spiHardware, Pin *ssPin)
{
	this->SpiHardware = spiHardware;

	// Configure Slave Select
	this->ssPin = ssPin;
	ssPin->setDirection();
	ssPin->setPinControl(PORT_OPC_TOTEM_gc);
	releaseSS();
}

SpiHal *SPI::getSpiHal()
{
	return SpiHardware;
}

void SPI::setSS()
{
	ssPin->clearOutput();
}

void SPI::releaseSS()
{
	ssPin->setOutput();
}
