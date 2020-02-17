/*
 * NCV7608.cpp
 *
 * Created: 01.06.2018 09:47:45
 *  Author: micha
 */ 

#include "NCV7608.h"

 NCV7608::NCV7608(SPI *spi)
{
	this->spi = spi;
	spi->getSpiHal()->configure(SPI_MODE_1_gc, 2000000);
	diagnosticsEnableCurrent = 0xFF;
	driverEnableCurrent = 0xFF;
	
	for (uint8_t channelIndex = 0; channelIndex < 8; channelIndex++)
	{
		setDriverEnable(channelIndex, false);
		setDiagnosticsEnable(channelIndex, false);
	}
	
	writeChanges();
}

void NCV7608::setDriverEnable(uint8_t channel, bool on)
{
	driverEnableNew &= ~(1 << channel);
	driverEnableNew |= ((uint8_t)on << channel);
}

void NCV7608::setDriverEnable(uint8_t channelMask)
{
	driverEnableNew = channelMask;
}

bool NCV7608::getDriverEnable(uint8_t channel)
{
	return (driverEnableCurrent & (1 << channel));
}

void NCV7608::toggleChannel(uint8_t channel)
{
	if (getDriverEnable(channel))
		setDriverEnable(channel, false);
	else
		setDriverEnable(channel, true);
}

void NCV7608::setDiagnosticsEnable(uint8_t channel, bool on)
{
	diagnosticsEnableNew &= ~(1 << channel);
	diagnosticsEnableNew |= ((uint8_t)on << channel);
}

bool NCV7608::getDiagnosticsStatus(uint8_t channel)
{
	return (diagnosticsEnableCurrent & (1 << channel));
}

void NCV7608::writeChanges()
{
	if (driverEnableNew != driverEnableCurrent || diagnosticsEnableNew != diagnosticsEnableCurrent)
	{
		uint16_t transmitData = ((uint16_t) diagnosticsEnableNew << 8) | driverEnableNew;
		uint16_t receiveData;
		spi->transceive(&transmitData, &receiveData);
		driverEnableCurrent = driverEnableNew;
		diagnosticsEnableCurrent = diagnosticsEnableNew;
		diagnosticsStatus = (receiveData & 0b0000000111111110) >> 1;
	}
}