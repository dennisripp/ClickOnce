/*
 * SpiNative.h
 *
 * Created: 05.06.2018 18:25:40
 *  Author: micha
 */ 


#ifndef SPINATIVE_H_
#define SPINATIVE_H_

#include "SpiHal.h"

class SpiNativeMaster :public SpiHal
{
	public:
		SpiNativeMaster(SPI_t *_module, PORT_t *_port, SPI_MODE_t mode, uint32_t clock);
		SpiNativeMaster(SPI_t *_module, PORT_t *_port);
		void configure(SPI_MODE_t mode, uint32_t clock);
		uint8_t transceive(uint8_t transmitData);
		uint8_t transceiveAsync(uint8_t transmitData);
		uint8_t waitUntilTransmissionComplete();
	private:
		SPI_t *module;
		PORT_t *port;
		bool transmissionComplete;
};

#endif /* SPINATIVE_H_ */