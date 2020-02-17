/*
 * Pixel.cpp
 *
 * Created: 01.06.2018 10:35:30
 *  Author: micha
 */ 

#include "Pixel.h"

 Pixel::Pixel(NCV7608 *_driver, uint8_t _channel)
{
	driver = _driver;
	channel = _channel;
}

void Pixel::setEnable(bool on)
{
	driver->setDriverEnable(channel, on);
}

void Pixel::toggle()
{
	driver->toggleChannel(channel);
}

bool Pixel::getEnable()
{
	return driver->getDriverEnable(channel);
}

void Pixel::setDiagnostics(bool on)
{
	driver->setDiagnosticsEnable(channel, on);
}

bool Pixel::getDiagnostics()
{
	return driver->getDiagnosticsStatus(channel);
}
