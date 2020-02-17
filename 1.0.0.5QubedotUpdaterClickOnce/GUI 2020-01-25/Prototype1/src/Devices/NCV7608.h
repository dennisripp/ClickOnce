/*
 * NCV7608.h
 *
 * Created: 01.06.2018 09:07:06
 *  Author: micha
 */ 


#ifndef NCV7608_H_
#define NCV7608_H_

#include "Interfaces/SPI.h"

class NCV7608
{
	public:
		NCV7608(SPI *spi);
		void setDriverEnable(uint8_t channel, bool on);
		void setDriverEnable(uint8_t channelMask);
		bool getDriverEnable(uint8_t channel);
		void toggleChannel(uint8_t channel);
		void setDiagnosticsEnable(uint8_t channel, bool on);
		bool getDiagnosticsStatus(uint8_t channel);
		void writeChanges();
	private:
		SPI *spi;
		uint8_t driverEnableCurrent;
		uint8_t driverEnableNew;
		uint8_t diagnosticsEnableCurrent;
		uint8_t diagnosticsEnableNew;
		uint8_t diagnosticsStatus;
};


#endif /* NCV7608_H_ */