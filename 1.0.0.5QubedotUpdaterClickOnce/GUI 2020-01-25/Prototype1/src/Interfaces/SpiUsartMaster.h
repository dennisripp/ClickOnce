/*
 * SpiUartMaster.h
 *
 * Created: 05.06.2018 18:28:45
 *  Author: micha
 */ 


#ifndef SPIUARTMASTER_H_
#define SPIUARTMASTER_H_

#include "SpiHal.h"
#include "Periphery/Pin.h"

class SpiUsartMaster :public SpiHal
{
	public:
		SpiUsartMaster(USART_t *_module, PORT_t *_port, SPI_MODE_t mode, uint32_t clock);
		SpiUsartMaster(USART_t *_module, PORT_t *_port);
		void configure(SPI_MODE_t mode, uint32_t clock);
		uint8_t transceive(uint8_t transmitData);
		uint8_t transceiveAsync(uint8_t transmitData);
		uint8_t waitUntilTransmissionComplete();
	private:
		USART_t *module;
		PORT_t *port;
		bool transmissionComplete;
};

#endif /* SPIUARTMASTER_H_ */